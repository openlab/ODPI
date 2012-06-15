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
                errorMessage = "You must provide the Subscription Id";
            else if (string.IsNullOrEmpty(name))
                errorMessage = "You must provide the Storage Account Name";
            else if (string.IsNullOrEmpty(key))
                errorMessage = "You must provide the Primary Access Key";

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
                    errorMessage = "Storage Account Exception :" + e.Message + "<br />Please recheck your credentials";
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
                        errorMessage = "Connected securly to Azure, however there are no Hosted Services could be found for this Subscription.  Please make sure you have at least one Hosted Service set up.";
                    }
                }
                catch (WebException we)
                {
                    var resp = we.Response as HttpWebResponse;

                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        errorMessage = "Could not connect to Azure, Please check that your Subscription Id is correct.";
                    }
                    if (resp.StatusCode == HttpStatusCode.Forbidden)
                    {
                        errorMessage = "Could not make a secure connection to Azure.  Please make sure that you have uploaded the certificate using the Management Console.";
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = "Unknown Error: " + ex.Message;
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
