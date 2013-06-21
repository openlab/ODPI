using System.Web.Mvc;

namespace ODPI.Controllers
{
    public class TplController : Controller
    {
        //
        // GET: /Tpl/

        public ActionResult Tasklist()
        {
            return View();
        }

        public ActionResult AvailAppItem()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        #region Config Templates
        public ActionResult AzureConfig()
        {
            return View();
        }

        public ActionResult Config()
        {
            return View();
        }

        public ActionResult ConfigItem()
        {
            return View();
        }
        #endregion Config Templates

        #region Deploy Templates

        public ActionResult Deploy()
        {
            return View();
        }

        public ActionResult DeployItem()
        {
            return View();
        }

        public ActionResult ManualItem()
        {
            return View();
        }

        #endregion Deploy Templates

    }
}
