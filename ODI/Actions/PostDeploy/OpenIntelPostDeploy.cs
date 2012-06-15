using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using ODI.Service;
using ODI.Models;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Diagnostics;

namespace ODI.Actions.PostDeploy
{
    public class OpenIntelPostDeploy : IPostDeployAction
    {

        private const string command = "{0} \"Data Source={1}; Initial Catalog={2}; User ID={3}; Password={4}\"";
        public void PerformAction(dynamic data)
        {

            var app = OdiAppRepo.Apps.Where(a => a.Name == "OpenIntel").FirstOrDefault();

            // Create a connection string for the sample database
            SqlConnectionStringBuilder connStringBuilder;
            connStringBuilder = new SqlConnectionStringBuilder();
            connStringBuilder.DataSource = data.dbhost;
            connStringBuilder.InitialCatalog = data.dbname;
            connStringBuilder.Encrypt = true;
            connStringBuilder.TrustServerCertificate = false;
            connStringBuilder.UserID = data.dbusername;
            connStringBuilder.Password = data.dbpassword;

            // Connect to the master database and create the sample database
            using (SqlConnection conn = new SqlConnection(connStringBuilder.ToString()))
            {
                using (SqlCommand command = conn.CreateCommand())
                {

                    conn.Open();


                    foreach (var sql in app.RequiredFiles.Where(f => f.EndsWith("sql")))
                    {
                        FileInfo file = new FileInfo(CloudBackedStore.RootDir + "\\" + PackageBuilder.ComponetDir + "\\" + sql);
                        string script = file.OpenText().ReadToEnd();

                        foreach (var statement in script.Split(';'))
                        {
                            if (!string.IsNullOrWhiteSpace(statement))
                            {
                                command.CommandText = statement;
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    conn.Close();
                }
            }

            putMapFilesInStorage(data, app);
        }

        private void putMapFilesInStorage(dynamic data, OdiApp app)
        {
            var creds = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", data.storagename, data.storagekey);
            CloudStorageAccount sa = CloudStorageAccount.Parse(creds);
            CloudBlobClient bc = sa.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = bc.GetContainerReference("mapfiles");
            
            blobContainer.CreateIfNotExist();
            BlobContainerPermissions permissions = blobContainer.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            blobContainer.SetPermissions(permissions);

            foreach (var file in app.RequiredFiles.Where(f => f.EndsWith("xml") || f.EndsWith("mapx")))
            {
                if (file.EndsWith("mapx"))
                    MoveMap(file, data);
                var blob = blobContainer.GetBlockBlobReference(file);
                blob.UploadFile(CloudBackedStore.RootDir + "\\" + PackageBuilder.ComponetDir + "\\" + file);
            }
        }

        private void MoveMap(string file, dynamic data)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = CloudBackedStore.RootDir + "\\" + PackageBuilder.ComponetDir + "\\MoveMap.exe";
            start.Arguments = string.Format(string.Format( command, file, data.dbhost, data.dbname, data.dbusername, data.dbpassword ));
            start.WorkingDirectory = CloudBackedStore.RootDir + PackageBuilder.ComponetDir;
            //start.Verb = "runas";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            using (Process process = Process.Start(start))
            {
                process.WaitForExit();

                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new ApplicationException("Error: " + error + ", Output: " + output);
                }
            }
        }
    }
}