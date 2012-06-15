using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODI.Model.Config
{
    public class TableStorageConfig : IOdiAppConfig
    {
        public string Key { get; set; }
        public string BlobAccountName { get; set; }
        public string BlobAccountKey { get; set; }

        public string BuildSettingsString()
        {
            string template = "<Setting name=\"{0}\" value=\"DefaultEndpointsProtocol=https;AccountName={1};AccountKey={2}\" />";

            return string.Format(template, Key, BlobAccountName, BlobAccountKey);
        }

        public void BuildFromData(dynamic data)
        {
            BlobAccountName = data.storagename;
            BlobAccountKey = data.storagekey;
        }

        public string Template { get { return "BlobStorage"; } }
    }
}