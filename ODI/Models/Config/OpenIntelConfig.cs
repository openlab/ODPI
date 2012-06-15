using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODI.Model.Config
{
    public class OpenIntelConfig : IOdiAppConfig
    {
        public string DbName { get; set; }
        public string DbHost { get; set; }
        public string DbUserName { get; set; }
        public string DbUserPassword { get; set; }
        public string BingServiceKey { get; set; }
        public string StorageName { get; set; }
        public string StorageKey { get; set; }

        public string BuildSettingsString()
        {
            string template = @"<Setting name=""OIConnectionString"" value=""Server=tcp:{0};Database={1};User ID={2};Password={3};Trusted_Connection=False;Encrypt=True;"" />
                                <Setting name=""BingServiceKey"" value=""{4}"" />
                                <Setting name=""mapdotnet.adminservicesettings.AzureStorageFactoryAccountName"" value=""{5}"" />
                                <Setting name=""mapdotnet.adminservicesettings.AzureStorageFactoryAccessKey"" value=""{6}"" />
                                ";

            return string.Format(template, DbHost, DbName, DbUserName, DbUserPassword, BingServiceKey,StorageName, StorageKey);
        }

        public void BuildFromData(dynamic data)
        {
            DbName = data.dbname;
            DbHost = data.dbhost;
            DbUserName = data.dbusername;
            DbUserPassword = data.dbpassword;
            BingServiceKey = data.bingservicekey;
            StorageName = data.storagename;
            StorageKey = data.storagekey;
        }

        public string Template { get { return "OpenIntel"; } }
    }
}