using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODPI.Model.Config
{
    public class OpenTurfConfig : IOdpiAppConfig
    {
        public string DbName { get; set; }
        public string DbHost { get; set; }
        public string DbUserName { get; set; }
        public string DbUserPassword { get; set; }
        public string AppName { get; set; }
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }
        public string Waaddomainname { get; set; }
        public string BingCredential { get; set; }
        public string StorageName { get; set; }
        public string StorageKey { get; set; }

        public string BuildSettingsString()
        {
            string template = @"
              <Setting name=""Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={9};AccountKey={10}"" />
              <Setting name=""DBConnectionString"" value=""Server=tcp:{0},1433;Database={1};User ID={2};Password={3};Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"" />
              <Setting name=""TwitterConsumerKey"" value=""{4}"" />
              <Setting name=""TwitterConsumerSecret"" value=""{5}"" />
              <Setting name=""FederationMetadataAddress"" value=""https://login.windows.net/{6}.onmicrosoft.com/federationmetadata/2007-06/federationmetadata.xml"" />
              <Setting name=""FederationWtrealm"" value=""http://{7}.cloudapp.net/"" />
              <Setting name=""BingCredential"" value=""{8}"" /> ";

            return string.Format(template, DbHost, DbName, DbUserName, DbUserPassword, TwitterConsumerKey, TwitterConsumerSecret, Waaddomainname, AppName, BingCredential, StorageName, StorageKey);
        }

        public void BuildFromData(dynamic data)
        {
            DbName = data.dbname;
            DbHost = data.dbhost;
            DbUserName = data.dbusername;
            DbUserPassword = data.dbpassword;
            AppName = data.appname;
            TwitterConsumerKey = data.twitterconsumerkey;
            TwitterConsumerSecret = data.twitterconsumersecret;
            Waaddomainname = data.waaddomainname;
            BingCredential = data.bingCredential;
            StorageName = data.storagename;
            StorageKey = data.storagekey;
        }

        public string Template { get { return "OpenTurf"; } }

        
    }
}