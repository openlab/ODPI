using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODPI.Model.Config;

namespace ODPI.Model.Config
{
    public class CitizenPortalOpenDataConfig : IOdpiAppConfig
    {
        public string BlobAccountName { get; set; }
        public string BlobAccountKey { get; set; }
        public string CloudServiceName { get; set; }
        public string WAADDomainName { get; set; }
        public string BingCredential { get; set; }

        public string BuildSettingsString()
        {
            string template = @"<Setting name=""Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={2};AccountKey={3}"" />
                                <Setting name=""FederationMetadataLocation"" value=""https://login.windows.net/{1}.onmicrosoft.com/FederationMetadata/2007-06/FederationMetadata.xml"" />
                                <Setting name=""audienceUri"" value=""http://{0}.cloudapp.net/"" />
                                <Setting name=""trustedIssuerName"" value=""https://sts.windows.net/{1}.onmicrosoft.com.onmicrosoft.com/"" />
                                <Setting name=""issuer"" value=""https://login.windows.net/{1}.onmicrosoft.com/wsfed"" />
                                <Setting name=""realm"" value=""http://{0}.cloudapp.net/"" />
                                <Setting name=""connectionString"" value=""DefaultEndpointsProtocol=https;AccountName={2};AccountKey={3}"" />
                                <Setting name=""bingCredential"" value=""{4}"" />";

            return string.Format(template, CloudServiceName, WAADDomainName, BlobAccountName, BlobAccountKey, BingCredential);
        }

        public void BuildFromData(dynamic data)
        {
            CloudServiceName = data.cloudservicename;
            WAADDomainName = data.waaddomainname;
            BlobAccountName = data.storagename;
            BlobAccountKey = data.storagekey;
            BingCredential = data.bingCredential;
        }

        public string Template
        {
            get { return "CitizenPortalOpenData"; }
        }
    }
}