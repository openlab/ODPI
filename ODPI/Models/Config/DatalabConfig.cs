using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODPI.Model.Config
{
    public class DatalabConfig : IOdpiAppConfig
    {
        public string BlobAccountName { get; set; }
        public string BlobAccountKey { get; set; }
        public string Dns { get; set; }
        public string RecapPublicKey { get; set; }
        public string RecapPrivateKey { get; set; }
        public string BingCredential { get; set; }
        public string BuildSettingsString()
        {
            string template = @"<Setting name=""Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
                                <Setting name=""Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
                                <Setting name=""serviceUri"" value=""http://{2}.cloudapp.net:8080/v1/"" />
                                <Setting name=""DataConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
                                <Setting name=""DiagnosticsConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
                                <!--
                                    Please replace the keys below with your private ones obtained from http://recaptcha.net/whyrecaptcha.html.
                                -->
                                <Setting name=""RecaptchaPrivateKey"" value=""{3}"" />
                                <Setting name=""RecaptchaPublicKey"" value=""{4}"" />
                                <Setting name=""bingCredential"" value=""{5}"" />
                            </ConfigurationSettings>
                            </Role>
                            <Role name=""DataBrowser.WorkerRole"">
                            <Instances count=""1"" />
                            <ConfigurationSettings>
                                <Setting name=""DiagnosticsConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
                                <Setting name=""DataConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
                                <Setting name=""serviceUri"" value=""http://{2}.cloudapp.net:8080/v1/"" />";

            return string.Format(template, BlobAccountName, BlobAccountKey, Dns, RecapPublicKey, RecapPrivateKey, BingCredential);
        }

        public void BuildFromData(dynamic data)
        {
            BlobAccountName = data.storagename;
            BlobAccountKey = data.storagekey;
            Dns = data.dns;
            RecapPrivateKey = data.recappriv;
            RecapPublicKey = data.recappub;
            BingCredential = data.bingCredential;
        }

        public string Template
        {
            get { return "DataLab"; }
        }
    }
}