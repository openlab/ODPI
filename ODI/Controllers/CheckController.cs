using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ODI.Models;
using ODI.Service;
using System.Net;
using System.Xml.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ODI.Controllers
{
    public class CheckController : Controller
    {
        [HttpPost]
        public JsonResult Register()
        {
            var ret = new CheckMessage()
            {
                Status = CheckMessageStatus.Ok,
                Data = Guid.NewGuid().ToString()
            };
            return Json(ret);
        }

        [HttpPost]
        public JsonResult InitAzure(string id, string subId, string name, string key)
        {
            CheckMessage ret;
            string errorMessage = null;

            if (string.IsNullOrEmpty(subId))
                errorMessage = ODI.Resources.Controllers.CheckResource.ErrorSubscriptionId;
            else if (string.IsNullOrEmpty(name))
                errorMessage = ODI.Resources.Controllers.CheckResource.ErrorStorageAccountName;
            else if (string.IsNullOrEmpty(key))
                errorMessage = ODI.Resources.Controllers.CheckResource.ErrorPrimaryAccessKey;

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var creds = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", name, key);
                    CloudStorageAccount sa = CloudStorageAccount.Parse(creds);
                    CloudBlobClient bc = sa.CreateCloudBlobClient();
                    bc.ListContainers();
                }
                catch (Exception e)
                {
                    errorMessage = string.Format(ODI.Resources.Controllers.CheckResource.StorageAccountException, e.Message);
                }
            }

            string xml = null;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    xml = AzureManagement.GetHostedServices(subId, CertificateBuilder.GetPrivateKey(id));
                    XNamespace xmlns = "http://schemas.microsoft.com/windowsazure";
                    var doc = XDocument.Parse(xml);

                    var services = from hs in doc.Descendants(xmlns + "HostedService")
                                   select hs.Element(xmlns + "ServiceName").Value;

                    if (services.Count() > 0)
                    {

                        ret = new CheckMessage()
                        {
                            Status = CheckMessageStatus.Ok,
                            Data = new
                            {
                                Services = services.ToArray<string>()
                            }
                        };

                        return Json(ret);
                    }
                    else
                    {
                        errorMessage = ODI.Resources.Controllers.CheckResource.ErrorNoHostedServiceFound;
                    }
                }
                catch (WebException we)
                {
                    var resp = we.Response as HttpWebResponse;

                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        errorMessage = ODI.Resources.Controllers.CheckResource.ErrorCouldNotConnectToAzure;
                    }
                    if (resp.StatusCode == HttpStatusCode.Forbidden)
                    {
                        errorMessage = ODI.Resources.Controllers.CheckResource.ErrorCouldNotMakeSecureConnection;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ODI.Resources.Controllers.CheckResource.UnknownError + ex.Message;
                }




                
            }

            
           
            ret = new CheckMessage()
            {
                Status = CheckMessageStatus.Error,
                Message = errorMessage
            };

            return Json(ret);
        }



    }
}
