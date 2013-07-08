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
                PackageName = "datalab_0.1.cspkg",
                ConfName = "datalab_0.1.cscfg",
                PostAction = new DatalabPostDeploy(),
                Validations = new IValidateAction[] { new TableStorageValidation() },
                RequiredFiles = new string[]
                {
                    //"BurlingtonParks.cfg",
                    //"BurlingtonParks.csv",
                    //"DataLoader.dll",
                    //"DataLoader.pdb",
                    //"DataLoaderUtility.exe",
                    //"DataLoaderUtility.exe.config.template",
                    //"DataLoaderUtility.pdb",
                    //"LumenWorks.Framework.IO.dll",
                    //"Microsoft.WindowsAzure.StorageClient.dll",
                    //"Microsoft.WindowsAzure.StorageClient.xml"
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
                PackageName = "openturf_0.2.cspkg",
                ConfName = "openturf_0.2.cscfg",
                PostAction = new OpenTurfPostDeploy(),
                Validations = new IValidateAction[] { new SqlAzureValidation(), new TableStorageValidation() },
                RequiredFiles = new string[] 
                { 
                    "open_turf_create_tables.0.1.sql"
                },
                SiteUrl = "http://{0}.cloudapp.net"
            });

            //Apps.Add(new OdpiApp()
            //{
            //    Id = 2,
            //    DisplayOrder = 2,
            //    Name = ODPI.Resources.Models.OdpiAppRepoResource.Drupal,
            //    ConfigHelpText = ODPI.Resources.Models.OdpiAppRepoResource.DrupalConfigHelpText,
            //    Description = ODPI.Resources.Models.OdpiAppRepoResource.DrupalDescription,
            //    Configs = new IOdpiAppConfig[] { new SqlAzureConfig() },
            //    Validations = new IValidateAction[] { new PhpSqlAzureValidation() },
            //    PackageName = "drupal_0.2.cspkg",
            //    ConfName = "drupal_0.2.cscfg",
            //    SiteUrl = "http://{0}.cloudapp.net/install.php"
            //});

            //Apps.Add(new OdpiApp()
            //{
            //    Id = 4,
            //    DisplayOrder = 4,
            //    Name = ODPI.Resources.Models.OdpiAppRepoResource.OpenIntel,
            //    ConfigHelpText = ODPI.Resources.Models.OdpiAppRepoResource.OpenIntelConfigHelpText,
            //    Description = ODPI.Resources.Models.OdpiAppRepoResource.OpenIntelDescription,
            //    Configs = new IOdpiAppConfig[] { new OpenIntelConfig() },
            //    Validations = new IValidateAction[] { new SqlAzureValidation(), new TableStorageValidation() },
            //    PostAction = new OpenIntelPostDeploy(),
            //    PackageName = "openintel_0.2.cspkg",
            //    ConfName = "openintel_0.1.cscfg",
            //    RequiredFiles = new string[]
            //    {
            //        "open_intel_create_tables.0.1.sql",
            //        "OI_Sample.mapx",
            //        "MapFiles.xml",
            //        "ISC.MapDotNetServer.Common.dll",
            //        "ISC.MapDotNetServer.Common.Maps.dll",
            //        "ISC.MapDotNetServer.Common.Maps.xml",
            //        "ISC.MapDotNetServer.Common.xml",
            //        "MoveMap.exe",
            //        "MoveMap.pdb"
            //    },
            //    SiteUrl = "http://{0}.cloudapp.net"
            //});

            //Apps.Add(new OdpiApp()
            //{
            //    Id = 5,
            //    DisplayOrder = 5,
            //    Name = ODPI.Resources.Models.OdpiAppRepoResource.DataPublic,
            //    ConfigHelpText = ODPI.Resources.Models.OdpiAppRepoResource.DataPublicConfigHelpText,
            //    Description = ODPI.Resources.Models.OdpiAppRepoResource.DataPublicDescription,
            //    Configs = new IOdpiAppConfig[] { new SqlAzureConfig() },
            //    Validations = new IValidateAction[] { new PhpSqlAzureValidation() },
            //    PackageName = "datapub_0.1.cspkg",
            //    ConfName = "datapub_0.1.cscfg",
            //    SiteUrl = "http://{0}.cloudapp.net/install.php"
            //});

            Apps.Add(new OdpiApp()
            {
                Id = 6,
                DisplayOrder = 6,
                Name = ODPI.Resources.Models.OdpiAppRepoResource.CitizenPortalOpenData,
                ConfigHelpText = ODPI.Resources.Models.OdpiAppRepoResource.CitizenPortalOpenDataConfigHelpText,
                Description = ODPI.Resources.Models.OdpiAppRepoResource.CitizenPortalOpenDataDescription,
                Configs = new IOdpiAppConfig[] { new CitizenPortalOpenDataConfig() },
                PackageName = "citizenportalopendata_1.0.cspkg",
                ConfName = "citizenportalopendata.1.cscfg",
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