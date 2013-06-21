using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ODPI.Models;
using ODPI.Service;
using JsonFx.Json;
using System.Xml.Linq;
using ODPI.Services;

namespace ODPI.Controllers
{
    public class DeployController : Controller
    {
        [HttpPost]
        public JsonResult Build(string key, string data)
        {
            try
            {
                PackageBuilder.BuildPackage((new JsonReader()).Read(data), key);
            }
            catch (Exception ex)
            {
                return Json( new DeployStatusModel()
                {
                    Status = DeployStatusModelStatus.Error,
                    Stage = ODPI.Resources.Controllers.DeployResource.Error,
                    LogMessage = ex.Message
                });
            }

            var ret = new DeployStatusModel()
            {
                Status = DeployStatusModelStatus.Ok,
                Stage = ODPI.Resources.Controllers.DeployResource.BuildingApplication,
                LogMessage = ODPI.Resources.Controllers.DeployResource.ApplicationBuiltSuccessfully
            };
            return Json(ret);
        }

        [HttpPost]
        public JsonResult Upload(string azure, string data)
        {
            string d = null;
            try
            {
                dynamic json = (new JsonReader()).Read(data);
                dynamic az = (new JsonReader()).Read(azure);
                var app = OdpiAppRepo.Apps.Where(a => a.Name == json.name).FirstOrDefault();
                var file = CloudBackedStore.RootDir + "\\" + az.key + "\\Temp\\" + app.Name + "\\" + app.PackageName;
                d = CloudBackedStore.PutDeploy((new JsonReader()).Read(azure), file);
            }
            catch (Exception ex)
            {
                return Json(new DeployStatusModel()
                {
                    Status = DeployStatusModelStatus.Error,
                    Stage = ODPI.Resources.Controllers.DeployResource.Error,
                    LogMessage = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }

            var ret = new DeployStatusModel()
            {
                Status = DeployStatusModelStatus.Ok,
                Stage = ODPI.Resources.Controllers.DeployResource.UploadingApplication,
                LogMessage = ODPI.Resources.Controllers.DeployResource.BeginningToUploadTheApplication,
                Data = d
            };
            return Json(ret);
        }

        [HttpPost]
        public JsonResult Deploy(string azure, string data)
        {
            string dt = null;
            try
            {
                dynamic json = (new JsonReader()).Read(data);
                dynamic az = (new JsonReader()).Read(azure);
                
                var app = OdpiAppRepo.Apps.Where(a => a.Name == json.name).FirstOrDefault();
                var cfgName = CloudBackedStore.RootDir + "\\" + az.key + "\\Temp\\" + app.Name + "\\" + "ServiceConfiguration.cscfg";

                dt = AzureManagement.Deploy(az.subscriptionId, az.service, CertificateBuilder.GetPrivateKey(az.key), az.file, cfgName); 
            }
            catch (Exception ex)
            {
                return Json(new DeployStatusModel()
                {
                    Status = DeployStatusModelStatus.Error,
                    Stage = ODPI.Resources.Controllers.DeployResource.Error,
                    LogMessage = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
            var ret = new DeployStatusModel()
            {
                Status = DeployStatusModelStatus.Ok,
                Stage = ODPI.Resources.Controllers.DeployResource.DeployingApplication,
                LogMessage = ODPI.Resources.Controllers.DeployResource.BeginningToUploadTheApplication,
                Data = dt
            };
            return Json(ret);
        }

        [HttpPost]
        public JsonResult Status(string azure, string token)
        {
            string status = null;
            string xml = "";
            try
            {
                dynamic az = (new JsonReader()).Read(azure);
                xml = AzureManagement.GetStatusUpdate(az.subscriptionId, CertificateBuilder.GetPrivateKey(az.key), token);
                XNamespace ns = "http://schemas.microsoft.com/windowsazure";
                var doc = XDocument.Parse(xml);
                var op = doc.Element(ns + "Operation");
                status = op.Element(ns + "Status").Value;
                if (status.ToLower().StartsWith("failed"))
                {
                    return Json(new DeployStatusModel()
                    {
                        Status = DeployStatusModelStatus.Error,
                        Stage = ODPI.Resources.Controllers.DeployResource.Error,
                        LogMessage = xml
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new DeployStatusModel()
                {
                    Status = DeployStatusModelStatus.Error,
                    Stage = ODPI.Resources.Controllers.DeployResource.Error,
                    LogMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Data = "xml: + " + xml + "\njson: " + azure
                });
            }

            var ret = new DeployStatusModel()
            {
                Status = status == "Succeeded" ? DeployStatusModelStatus.Ok : DeployStatusModelStatus.Inprogress,
                Stage = ODPI.Resources.Controllers.DeployResource.DeployingApplication,
                LogMessage = StatusMessageConverter.Convert(status)
            };
            return Json(ret);
        }

        [HttpPost]
        public JsonResult DeployStatus(string azure, string token)
        {
            string status = null;
            try
            {
                dynamic az = (new JsonReader()).Read(azure);
                var xml = AzureManagement.GetDeployStatusUpdate(az.subscriptionId, CertificateBuilder.GetPrivateKey(az.key), token);
                XDocument doc = XDocument.Parse(xml);
                XNamespace ns = "http://schemas.microsoft.com/windowsazure";
                status = doc.Descendants(ns + "RoleInstance").FirstOrDefault().Element(ns + "InstanceStatus").Value;
                if (status.ToLower().StartsWith("failed"))
                {
                    return Json(new DeployStatusModel()
                    {
                        Status = DeployStatusModelStatus.Error,
                        Stage = ODPI.Resources.Controllers.DeployResource.Error,
                        LogMessage = xml
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new DeployStatusModel()
                {
                    Status = DeployStatusModelStatus.Error,
                    Stage = ODPI.Resources.Controllers.DeployResource.Error,
                    LogMessage = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }

            var ret = new DeployStatusModel()
            {
                Status = status == "ReadyRole" ? DeployStatusModelStatus.Ok : DeployStatusModelStatus.Inprogress,
                Stage = ODPI.Resources.Controllers.DeployResource.DeployingApplication,
                LogMessage = StatusMessageConverter.Convert(status)
            };
            return Json(ret);
        }

        [HttpPost]
        public JsonResult PostDeploy(string azure, string data, string service)
        {
            string dt = null;
            try
            {
                dynamic json = (new JsonReader()).Read(data);
                var app = OdpiAppRepo.Apps.Where(a => a.Name == json.name).FirstOrDefault();

                if (app.PostAction != null)
                {
                    app.PostAction.PerformAction(json);
                }

                dt = string.Format(app.SiteUrl, service);
            }
            catch (Exception ex)
            {
                return Json(new DeployStatusModel()
                {
                    Status = DeployStatusModelStatus.Error,
                    Stage = ODPI.Resources.Controllers.DeployResource.Error,
                    LogMessage = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }

            //TODO: actually run the post deploy portion of the code
            var ret = new DeployStatusModel()
            {
                Status = DeployStatusModelStatus.Ok,
                Stage = ODPI.Resources.Controllers.DeployResource.RunningTheApplicationPostInstall,
                LogMessage = ODPI.Resources.Controllers.DeployResource.CompletedTheApplicationPostInstall,
                Data = dt
            };
            return Json(ret);
        }

        [HttpPost]
        public JsonResult Manual(string key, string data)
        {
            if (string.IsNullOrEmpty(key))
                key = Guid.NewGuid().ToString();

            string file = null;
            try
            {
                file = PackageBuilder.BuildPackageZip((new JsonReader()).Read(data), key);
            }
            catch (Exception ex)
            {
                return Json(new DeployStatusModel()
                {
                    Status = DeployStatusModelStatus.Error,
                    Stage = ODPI.Resources.Controllers.DeployResource.Error,
                    LogMessage = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }

            file = key + "\\" + file.Substring(file.LastIndexOf("\\"));

            var ret = new DeployStatusModel()
            {
                Status = DeployStatusModelStatus.Ok,
                Stage = ODPI.Resources.Controllers.DeployResource.BuildingApplication,
                LogMessage = ODPI.Resources.Controllers.DeployResource.ApplicationBuiltSuccessfully,
                Data = file
            };
            return Json(ret);
        }

        public FileResult Download(string file)
        {
            var fileDir = CloudBackedStore.RootDir + "\\" + file;
            return File(fileDir, "application/", file.Substring(file.LastIndexOf("\\")));
        }


    }
}
