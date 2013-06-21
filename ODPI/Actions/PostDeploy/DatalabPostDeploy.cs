using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using ODPI.Service;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ODPI.Actions.PostDeploy
{
    public class DatalabPostDeploy : IPostDeployAction
    {

        private const string CONFIG_TEMPLATE = 
            @"<?xml version=""1.0""?>
                <configuration>
                  <appSettings>
                    <add key=""table metadata entity set"" value=""TableMetadata""/>
                    <add key=""entity metadata entity set"" value=""EntityMetadata""/>
                    <add key=""processor params entity set"" value=""ProcessorParams""/>
    
                    <!--windows azure storage settings-->
                    <add key=""DataConnectionString"" value=""DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}""/>
                    <add key=""LoadThreadsCount"" value=""1""/>
                  </appSettings>
                  <system.diagnostics>
                    <trace autoflush=""true"" indentsize=""4"">
                      <listeners>
                        <add name=""fileListener"" type=""System.Diagnostics.TextWriterTraceListener"" initializeData=""Error.log""/>
                        <remove name=""Default""/>
                      </listeners>
                    </trace>
                  </system.diagnostics>
                  <system.net>
                    <connectionManagement>
                      <add address=""*"" maxconnection=""48""/>
                    </connectionManagement>
                    <settings>
                      <!-- expect100Continue=""false"", it will not help to increase speed -->
                      <!--<servicePointManager expect100Continue=""false"" useNagleAlgorithm=""false""/>  -->
                      <servicePointManager useNagleAlgorithm=""false""/>
                    </settings>
                  </system.net>
                <startup><supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.0""/></startup></configuration>";

        private const string ENDPOINTS_TABLENAME = "AvailableEndpoints";

        public void PerformAction(dynamic data)
        {
            //First create the AvailableEndpoints Table.
            CreateAvailableEndpoints(data);

            //fill out the exe.config template
            var dir = Guid.NewGuid();
            setupExe(dir.ToString());


            var exeDir = CloudBackedStore.RootDir + "Datalab\\" + dir; ;

            buildConfig(data, exeDir);
            //Run the dataloader to load the brampton data
            ProcessStartInfo start = new ProcessStartInfo();
            //start.FileName = CloudBackedStore.RootDir + CertDir + "\\makecert.exe";
            start.FileName = exeDir + "\\DataLoaderUtility.exe";
            start.Arguments = "/type=csv /fsname=BurlingtonParks /target=tables /mode=create /sourceorder";
            //start.Arguments = string.Format(command, CloudBackedStore.RootDir + "\\" + dir + "\\" + CertDir);
            //start.WorkingDirectory = CloudBackedStore.RootDir + CertDir;
            start.WorkingDirectory = exeDir;
            //start.Verb = "runas";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            using (Process process = Process.Start(start))
            {
                do
                {
                    Thread.Sleep(1000);
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                }
                while (!process.HasExited);

                //var output = process.StandardOutput.ReadToEnd();
                //var error = process.StandardError.ReadToEnd();
            }


            return;
        }

        private void CreateAvailableEndpoints(dynamic data)
        {
            if (string.IsNullOrEmpty(data.alias))
                return;

            string alias = data.alias;
            string description = data.description;
            string disclaimer = data.disclaimer;

            var esa = new AvailableEndpoint
            {
                PartitionKey = alias,
                RowKey = "",
                alias = alias,
                description = description,
                disclaimer = disclaimer,
                storageaccountname = data.storagename,
                storageaccountkey = data.storagekey
            };

            CloudStorageAccount ta = CloudStorageAccount.Parse(string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", data.storagename, data.storagekey));
            var ctx = new TableServiceContext(ta.TableEndpoint.AbsoluteUri, ta.Credentials);
            var tableClient = ta.CreateCloudTableClient();
            tableClient.CreateTableIfNotExist(ENDPOINTS_TABLENAME);
            ctx.AddObject(ENDPOINTS_TABLENAME, esa);
            ctx.SaveChanges();
        }

        private void buildConfig(dynamic data, string dir)
        {
            var xml = string.Format(CONFIG_TEMPLATE, data.storagename, data.storagekey);
            var fileName = dir + "\\DataLoaderUtility.exe.config";

            using (TextWriter tw = new StreamWriter(fileName))
            {
                tw.Write(xml);
                tw.Close();
            }
        }

        private void setupExe(string dir)
        {
            var compDir = CloudBackedStore.RootDir + "Components";
            var exeDir = CloudBackedStore.RootDir + "Datalab\\" + dir;

            if (!Directory.Exists(exeDir))
            {
                Directory.CreateDirectory(exeDir);
            }

            copyFromComponentsToDir(compDir, exeDir, new string[]
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
                });
        }

        private void copyFromComponentsToDir(string compDir, string exeDir, string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                var f = files[i];

                File.Copy(compDir + "\\" + f, exeDir + "\\" + f);
            }
        }

        public class AvailableEndpoint : TableServiceEntity
        {
            public AvailableEndpoint()
            {
            }

            // Properties are all lowercase in the Azure table because this was the 
            // naming convention we picked for consistency due to varying data source.
            public string alias { get; set; }
            public string description { get; set; }
            public string disclaimer { get; set; }
            public string storageaccountname { get; set; }
            public string storageaccountkey { get; set; }
        }
    }
}