using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODI.Model.Config;
using ODI.Actions.PostDeploy;
using ODI.Actions.Validate;

namespace ODI.Models
{
    public class OdiAppRepo
    {
        public static List<OdiApp> Apps { get; set; }

        static OdiAppRepo()
        {
            Apps = new List<OdiApp>();

            Apps.Add(new OdiApp()
            { 
                Id = 1, 
                DisplayOrder = 1, 
                Name = "Data Public",
                ConfigHelpText = @"<p>To configure Data Public on Windows Azure, we will first need to get information on a SQL Azure database to use. 
You will need to provide us with various items below so we can configure the package for installing Data Public on Windows Azure.</p>",
                Description = "DataPublic: a web portal for Open Government initiatives.",
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
                Name = "Drupal",
                ConfigHelpText = @"<p>To configure Drupal on Windows Azure, we will first need to get information on a SQL Azure database to use. 
You will need to provide us with various items below so we can configure the package of installing Drupal on Windows Azure.</p>",
                Description = "Drupal is an open source content management platform powering millions of websites and applications. It’s built, used, and supported by an active and diverse community of people around the world.",
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
                Name = "OpenTurf",
                ConfigHelpText = @"<p>To configure OpenTurf on Windows Azure, we will first need to get information on a SQL Azure database to use, set some application settings, Twitter settings and blob storage info.</p>
<p>You will need to provide us with various items below so we can configure the package of installing OpenTurf on Windows Azure.</p>",
                Description = "This is an OpenData Framework that makes it easy to Add and Consume OpenData Endpoints into a reusable code base.",
                Configs = new IOdiAppConfig[] { new OpenTurfConfig(), new TableStorageConfig() { Key = "BlobStorageEndpoint" } },
                PackageName = "openturf_0.2.cspkg",
                ConfName = "openturf_0.1.cscfg",
                PostAction = new OpenTurfPostDeploy(),
                Validations = new IValidateAction[] { new SqlAzureValidation(), new TableStorageValidation() },
                RequiredFiles = new string[] 
                { 
                    "open_turf_create_tables.0.1.sql",
                    "open_turf_create_views.0.1.sql"
                },
                SiteUrl = "http://{0}.cloudapp.net"
            });
            Apps.Add(new OdiApp()
            {
                Id = 4,
                DisplayOrder = 4,
                Name = "OpenIntel",
                ConfigHelpText = @"<p>To configure OpenIntel on Windows Azure, we will first need to get information on a SQL Azure database to use and blob storage information. 
You will need to provide us with various items below so we can configure the package of installing OpenIntel on Windows Azure.</p>",
                Description = "Open Intel is an accelerator for governments and private companies to rapidly build open data portals and killer business intelligence solutions with an emphasis on spatial analysis, data interaction and visualization.",
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
                Name = "DataLab",
                ConfigHelpText = @"<p>To configure DataLab on Windows Azure, we will first need to get information on blob storage information plus other settings. 
You will need to provide us with various items below so we can configure the package of installing DataLab on Windows Azure.</p>",
                Description = "DataLab is a solution that makes it possible for agencies to publish government and public data more quickly and efficiently.",
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