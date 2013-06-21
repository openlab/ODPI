using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ODPI.Actions.Validate
{
    public class TableStorageValidation : IValidateAction
    {
        public string Validate(dynamic data)
        {
            string message = null;

            try
            {
                var creds = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", data.storagename, data.storagekey);
                CloudStorageAccount sa = CloudStorageAccount.Parse(creds);
                CloudBlobClient bc = sa.CreateCloudBlobClient();
                bc.ListContainers();
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return message;
        }

    }
}