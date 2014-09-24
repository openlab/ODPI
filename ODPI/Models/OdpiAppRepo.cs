using ODPI.Actions.PostDeploy;
using ODPI.Actions.Validate;
using ODPI.Model.Config;
using ODPI.Resources.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODPI.Models
{
    public class OdpiAppRepo
    {
        public static List<OdpiApp> Apps { get ; set;  }

        public static void Refresh()
        {
            Initialize();
        }

        private static void Initialize()
        {
            try
            {
                OdpiAppRepoResource.Culture = new System.Globalization.CultureInfo(HttpContext.Current.Request.UserLanguages.FirstOrDefault());
            }
            catch
            {
                OdpiAppRepoResource.Culture = new System.Globalization.CultureInfo("en");
            }

            Apps = new List<OdpiApp>();

            Apps.Add(new OdpiApp()
            {
                Id = 1,
                DisplayOrder = 1,
                Name = ODPI.Resources.Models.OdpiAppRepoResource.DataLab,
                ConfigHelpText = ODPI.Resources.Models.OdpiAppRepoResource.DataLabConfigHelpText,
                Description = ODPI.Resources.Models.OdpiAppRepoResource.DataLabDescription,
                Configs = new IOdpiAppConfig[] { new DatalabConfig() },
                PackageName = "2014-09_OGDI_DataLab.cspkg",
                ConfName = "2014-09_OGDI_DataLab.cscfg",
                PostAction = new DatalabPostDeploy(),
                Validations = new IValidateAction[] { new TableStorageValidation() },
                RequiredFiles = new string[]
                {
                },
                SiteUrl = "http://{0}.cloudapp.net"
            });

            Apps.Add(new OdpiApp()
            {
                Id = 3,
                DisplayOrder = 3,
                Name = ODPI.Resources.Models.OdpiAppRepoResource.Openturf,
                ConfigHelpText = ODPI.Resources.Models.OdpiAppRepoResource.OpenturfConfigHelpText,
                Description = ODPI.Resources.Models.OdpiAppRepoResource.OpenturfDescription,
                Configs = new IOdpiAppConfig[] { new OpenTurfConfig(), new TableStorageConfig() { Key = "BlobStorageEndpoint" } },
                PackageName = "2014-09_ODAF_OpenTurf.cspkg",
                ConfName = "2014-09_ODAF_OpenTurf.cscfg",
                PostAction = new OpenTurfPostDeploy(),
                Validations = new IValidateAction[] { new SqlAzureValidation(), new TableStorageValidation() },
                RequiredFiles = new string[] 
                { 
                    "open_turf_create_tables.0.1.sql"
                },
                SiteUrl = "http://{0}.cloudapp.net"
            });

            Apps.Add(new OdpiApp()
            {
                Id = 6,
                DisplayOrder = 6,
                Name = ODPI.Resources.Models.OdpiAppRepoResource.CitizenPortalOpenData,
                ConfigHelpText = ODPI.Resources.Models.OdpiAppRepoResource.CitizenPortalOpenDataConfigHelpText,
                Description = ODPI.Resources.Models.OdpiAppRepoResource.CitizenPortalOpenDataDescription,
                Configs = new IOdpiAppConfig[] { new CitizenPortalOpenDataConfig() },
                PackageName = "2014-09_Citizen_Portal.cspkg",
                ConfName = "2014-09_Citizen_Portal.cscfg",
                Validations = new IValidateAction[] { },
                RequiredFiles = new string[] 
                { 
                },
                SiteUrl = "http://{0}.cloudapp.net"
            });
        }

        static OdpiAppRepo()
        {
            Initialize();
        }
     
    }
}