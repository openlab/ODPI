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

        public string BuildSettingsString()
        {
            string template = @"
      <Setting name=""serviceUri"" value=""http://{2}.cloudapp.net:8080/v1/"" />
      <Setting name=""DataConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
      <Setting name=""DiagnosticsConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
      <Setting name=""Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
      <Setting name=""RecaptchaPrivateKey"" value=""{3}"" />
      <Setting name=""RecaptchaPublicKey"" value=""{4}"" />
    </ConfigurationSettings>
  </Role>
  <Role name=""DataBrowser.WorkerRole"">
    <Instances count=""1"" />
    <ConfigurationSettings>
      <Setting name=""DiagnosticsConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
      <Setting name=""DataConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"" />
      <Setting name=""serviceUri"" value=""http://{2}.cloudapp.net:8080/v1/"" />
          ";

            return string.Format(template, BlobAccountName, BlobAccountKey, Dns, RecapPublicKey, RecapPrivateKey);
        }

        public void BuildFromData(dynamic data)
        {
            BlobAccountName = data.storagename;
            BlobAccountKey = data.storagekey;
            Dns = data.dns;
            RecapPrivateKey = data.recappriv;
            RecapPublicKey = data.recappub;
        }

        public string Template
        {
            get { return "DataLab"; }
        }
    }
}