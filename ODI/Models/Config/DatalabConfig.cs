using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODI.Model.Config
{
    public class DatalabConfig : IOdiAppConfig
    {
        public string Dns { get; set; }
        public string RecapPrivateKey { get; set; }
        public string RecapPublicKey { get; set; }
        public string BlobAccountName { get; set; }
        public string BlobAccountKey { get; set; }

        public string BuildSettingsString()
        {
            string template = @"<Setting name=""RecaptchaPrivateKey"" value=""{0}""/>
                                  <Setting name=""RecaptchaPublicKey"" value=""{1}""/>
                                  <Setting name=""DataConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={2};AccountKey={3}""/>
                                  <Setting name=""DiagnosticsConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={2};AccountKey={3}""/>
                                  <Setting name=""serviceUri"" value=""http://{4}.cloudapp.net:8080/v1/""/>
                                </ConfigurationSettings>
                              </Role>
                              <Role name=""DataBrowser.WorkerRole"">
                                <Instances count=""1""/>
                                <ConfigurationSettings>
                                  <Setting name=""serviceUri"" value=""http://{4}.cloudapp.net:8080/v1/"" />
                                  <Setting name=""DiagnosticsConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={2};AccountKey={3}"" />
                                  <Setting name=""DataConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={2};AccountKey={3}"" />
      
                                ";

            return string.Format(template, RecapPrivateKey, RecapPublicKey, BlobAccountName, BlobAccountKey, Dns );
        }

        public void BuildFromData(dynamic data)
        {
            Dns = data.dns;
            RecapPrivateKey = data.recappriv;
            RecapPublicKey = data.recappub;
            BlobAccountName = data.storagename;
            BlobAccountKey = data.storagekey;
        }

        public string Template
        {
            get { return "DataLab"; }
        }
    }
}