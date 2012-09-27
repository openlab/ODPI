using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ODI.Service
{
    public class CloudBackedStore
    {
        private static LocalResource localStorage = null;
        private static CloudStorageAccount storageAccount = null;
        private static CloudBlobClient blobClient = null;

        public static void Initialize()
        {
            //First get a reference to the local file structure.
            localStorage = RoleEnvironment.GetLocalResource("scratchpad");

            //Set up the Storage Account and Client
#if DEBUG
            var acc = "UseDevelopmentStorage=true";
#else
            //points to microsoft azure
           var acc = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", RoleEnvironment.GetConfigurationSettingValue("StorageName"), RoleEnvironment.GetConfigurationSettingValue("AccountKey")); 
#endif

            storageAccount = CloudStorageAccount.Parse(acc);
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public static string RootDir { get { return localStorage.RootPath; } }

        public static bool DirectoryExists(string Dir)
        {
            return Directory.Exists( RootDir + Dir);
        }

        public static string PutDeploy(dynamic data, string file)
        {
            var creds = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", data.storageName, data.storageKey);
            CloudStorageAccount sa = CloudStorageAccount.Parse(creds);
            CloudBlobClient bc = sa.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = bc.GetContainerReference("odi-deploy");
            blobContainer.CreateIfNotExist();

            var blob = blobContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + ".cspkg");
            blob.UploadFile(file);

            return blob.Uri.ToString();
        }

        public static void Grab(string dir, string file, string container, string blobUrl)
        {
            if (!Directory.Exists(RootDir + dir))
            {
                Directory.CreateDirectory(RootDir + dir);
            }
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);
            var blob = blobContainer.GetBlobReference(blobUrl);
            blob.DownloadToFile(RootDir + dir + file);
        }

        public static void CreateDirectory(string dir)
        {
            Directory.CreateDirectory(RootDir + dir);
        }
    }
}