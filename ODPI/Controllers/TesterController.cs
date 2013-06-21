using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ODPI.Models;

namespace ODPI.Controllers
{
    public class TesterController : Controller
    {
        //
        // GET: /Tester/

        public ActionResult DatalabPostDeploy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DatalabPostDeploy(string storagename, string storagekey)
        {
            var app = OdpiAppRepo.Apps.Where(a => a.Name == "DataLab").FirstOrDefault();

            app.PostAction.PerformAction(new { storagename = storagename, storagekey = storagekey });
            

            ViewBag.Message = app.Name + " " + ODPI.Resources.Controllers.TesterResource.PostDeployCompleted;
            return View();
        }

    }
}
