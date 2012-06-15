using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODI.Models;
using System.Data.SqlClient;
using System.IO;
using ODI.Service;

namespace ODI.Actions.PostDeploy
{
    public class OpenTurfPostDeploy : IPostDeployAction
    {
        public void PerformAction(dynamic data)
        {
            var app = OdiAppRepo.Apps.Where(a => a.Name == "OpenTurf").FirstOrDefault();

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

                    //Now finally run the last command that set's up openturf so that it knows who it is.
                    var appSql = "Insert into OAuthClientApp (Guid, Name, Comment, ConsumerKey, ConsumerSecret, CallbackUrl, AppUrl, CreatedOn, oauth_service_name) " +
                                    "values ( 'D5B672D4-7B1C-46cc-8643-FBE8334F4ADF', 'Main Web Site', 'This is the main Silverlight application hosted on Azure', '{0}','{1}', '{2}', '{3}', getdate(), 'Twitter' );";

                    command.CommandText = string.Format(appSql, data.twitterconsumerkey, data.twitterconsumersecret, data.twittercallbackurl, data.twitterappurl);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}