using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ODI.Models;
using JsonFx.Json;

namespace ODI.Controllers
{
    public class AppController : Controller
    {
        //
        // GET: /App/

        public JsonResult Available()
        {
            var apps = OdiAppRepo.Apps;
           
            return Json(apps, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Validate(string data, string app)
        {
            dynamic json = (new JsonReader()).Read(data);
            var appToVal = OdiAppRepo.Apps.Where(a => a.Name == app).FirstOrDefault();
            var message = new CheckMessage() { Status = CheckMessageStatus.Ok, Message = "Validation OK" };

            foreach (var val in appToVal.Validations)
            {
                var ret = val.Validate(json);
                if (!string.IsNullOrEmpty(ret))
                {
                    message.Status = CheckMessageStatus.Error;
                    message.Message = ret;
                    break;
                }
            }

            return Json(message);
        }

    }
}
