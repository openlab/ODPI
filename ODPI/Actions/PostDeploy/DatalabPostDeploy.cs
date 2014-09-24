using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using ODPI.Service;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ODPI.Actions.PostDeploy
{
    public class DatalabPostDeploy : IPostDeployAction
    {
        private const string ENDPOINTS_TABLENAME = "AvailableEndpoints";

        public void PerformAction(dynamic data)
        {
            //First create the AvailableEndpoints Table.
            CreateAvailableEndpoints(data);
        }

        private void CreateAvailableEndpoints(dynamic data)
        {
            if (string.IsNullOrEmpty(data.alias))
                return;

            string alias = data.alias;
            string description = data.description;
            string disclaimer = data.disclaimer;

            var esa = new AvailableEndpoint
            {
                PartitionKey = alias,
                RowKey = "",
                alias = alias,
                description = description,
                disclaimer = disclaimer,
                storageaccountname = data.storagename,
                storageaccountkey = data.storagekey
            };

            CloudStorageAccount ta = CloudStorageAccount.Parse(string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", data.storagename, data.storagekey));
            var ctx = new TableServiceContext(ta.TableEndpoint.AbsoluteUri, ta.Credentials);
            var tableClient = ta.CreateCloudTableClient();
            tableClient.CreateTableIfNotExist(ENDPOINTS_TABLENAME);
            ctx.AddObject(ENDPOINTS_TABLENAME, esa);
            ctx.SaveChanges();
        }

        private void copyFromComponentsToDir(string compDir, string exeDir, string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                var f = files[i];

                File.Copy(compDir + "\\" + f, exeDir + "\\" + f);
            }
        }

        public class AvailableEndpoint : TableServiceEntity
        {
            public AvailableEndpoint()
            {
            }

            // Properties are all lowercase in the Azure table because this was the 
            // naming convention we picked for consistency due to varying data source.
            public string alias { get; set; }
            public string description { get; set; }
            public string disclaimer { get; set; }
            public string storageaccountname { get; set; }
            public string storageaccountkey { get; set; }
        }
    }
}