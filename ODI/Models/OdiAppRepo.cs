using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODI.Model.Config;
using ODI.Actions.PostDeploy;
using ODI.Actions.Validate;
using ODI.Resources.Models;

namespace ODI.Models
{
    public class OdiAppRepo
    {
        public static List<OdiApp> Apps { get ; set;  }

        static OdiAppRepo()
        {
            try
            {
                OdiAppRepoResource.Culture = new System.Globalization.CultureInfo(HttpContext.Current.Request.UserLanguages.FirstOrDefault());
            }
            catch { }

            Apps = new List<OdiApp>();

            Apps.Add(new OdiApp()
            {
                Id = 1,
                DisplayOrder = 1,
                Name = ODI.Resources.Models.OdiAppRepoResource.DataPublic,
                ConfigHelpText = ODI.Resources.Models.OdiAppRepoResource.DataPublicConfigHelpText,
                Description = ODI.Resources.Models.OdiAppRepoResource.DataPublicDescription,
                Configs = new IOdiAppConfig[] { new SqlAzureConfig() },
                Validations = new IValidateAction[] { new PhpSqlAzureValidation() },
                PackageName = "datapub_0.1.cspkg",
                ConfName = "datapub_0.1.cscfg",
                SiteUrl = "http://{0}.cloudapp.net/install.php"
            });
            Apps.Add(new OdiApp()
            {
                Id = 2,
                DisplayOrder = 2,
                Name = ODI.Resources.Models.OdiAppRepoResource.Drupal,
                ConfigHelpText = ODI.Resources.Models.OdiAppRepoResource.DrupalConfigHelpText,
                Description = ODI.Resources.Models.OdiAppRepoResource.DrupalDescription,
                Configs = new IOdiAppConfig[] { new SqlAzureConfig() },
                Validations = new IValidateAction[] { new PhpSqlAzureValidation() },
                PackageName = "drupal_0.2.cspkg",
                ConfName = "drupal_0.2.cscfg",
                SiteUrl = "http://{0}.cloudapp.net/install.php"
            });

            Apps.Add(new OdiApp()
            { 
                Id = 3, 
                DisplayOrder = 3, 
                Name = ODI.Resources.Models.OdiAppRepoResource.Openturf,
                ConfigHelpText = ODI.Resources.Models.OdiAppRepoResource.OpenturfConfigHelpText,
                Description = ODI.Resources.Models.OdiAppRepoResource.OpenturfDescription,
                Configs = new IOdiAppConfig[] { new OpenTurfConfig(), new TableStorageConfig() { Key = "BlobStorageEndpoint" } },
                PackageName = "openturf_0.2.cspkg",
                ConfName = "openturf_0.2.cscfg",
                PostAction = new OpenTurfPostDeploy(),
                Validations = new IValidateAction[] { new SqlAzureValidation(), new TableStorageValidation() },
                RequiredFiles = new string[] 
                { 
                    "open_turf_create_tables.0.1.sql"
                    ,
                    "open_turf_create_views.0.1.sql"  
                },
                SiteUrl = "http://{0}.cloudapp.net"
            });

            Apps.Add(new OdiApp()
            {
                Id = 4,
                DisplayOrder = 4,
                Name = ODI.Resources.Models.OdiAppRepoResource.OpenIntel,
                ConfigHelpText = ODI.Resources.Models.OdiAppRepoResource.OpenIntelConfigHelpText,
                Description = ODI.Resources.Models.OdiAppRepoResource.OpenIntelDescription,
                Configs = new IOdiAppConfig[] { new OpenIntelConfig() },
                Validations = new IValidateAction[] { new SqlAzureValidation(), new TableStorageValidation() },
                PostAction = new OpenIntelPostDeploy(),
                PackageName = "openintel_0.2.cspkg",
                ConfName = "openintel_0.1.cscfg",
                RequiredFiles = new string[]
                {
                    "open_intel_create_tables.0.1.sql",
                    "OI_Sample.mapx",
                    "MapFiles.xml",
                    "ISC.MapDotNetServer.Common.dll",
                    "ISC.MapDotNetServer.Common.Maps.dll",
                    "ISC.MapDotNetServer.Common.Maps.xml",
                    "ISC.MapDotNetServer.Common.xml",
                    "MoveMap.exe",
                    "MoveMap.pdb"
                },
                SiteUrl = "http://{0}.cloudapp.net"
            });

            Apps.Add(new OdiApp()
            {
                Id = 5,
                DisplayOrder = 5,
                Name = ODI.Resources.Models.OdiAppRepoResource.DataLab,
                ConfigHelpText = ODI.Resources.Models.OdiAppRepoResource.DataLabConfigHelpText,
                Description = ODI.Resources.Models.OdiAppRepoResource.DataLabDescription,
                Configs = new IOdiAppConfig[] { new DatalabConfig() },
                PackageName = "datalab_0.1.cspkg",
                ConfName = "datalab_0.1.cscfg",
                PostAction = new DatalabPostDeploy(),
                Validations = new IValidateAction[] { new TableStorageValidation() },
                RequiredFiles = new string[]
                {
                    "BurlingtonParks.cfg",
                    "BurlingtonParks.csv",
                    "DataLoader.dll",
                    "DataLoader.pdb",
                    "DataLoaderUtility.exe",
                    "DataLoaderUtility.exe.config.template",
                    "DataLoaderUtility.pdb",
                    "LumenWorks.Framework.IO.dll",
                    "Microsoft.WindowsAzure.StorageClient.dll",
                    "Microsoft.WindowsAzure.StorageClient.xml"
                },
                SiteUrl = "http://{0}.cloudapp.net"
            });
        }
     
    }
}