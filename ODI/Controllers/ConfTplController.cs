using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ODI.Controllers
{
    public class ConfTplController : Controller
    {
        //
        // GET: /ConfTpl/

        public ActionResult SqlAzure()
        {
            return View();
        }

        public ActionResult BlobStorage()
        {
            return View();
        }

        public ActionResult OpenIntel()
        {
            return View();
        }

        public ActionResult OpenTurf()
        {
            return View();
        }

        public ActionResult DataLab()
        {
            return View();
        }
    }
}
