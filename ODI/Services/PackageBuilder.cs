using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using ODI.Models;
using Ionic.Zip;
using System.Text;

namespace ODI.Service
{
    public static class PackageBuilder
    {
        private static LocalResource localStorage = null;
        public static string ComponetDir { get { return "\\Components"; } }

        public static void Initialize()
        {
            //First get a reference to the local file structure.
            localStorage = RoleEnvironment.GetLocalResource("scratchpad");

            //Check to see if we have already grabbed the component files.
            if (!CloudBackedStore.DirectoryExists(ComponetDir))
            {
#if DEBUG
                var url = "http://127.0.0.1:10000/devstoreaccount1/components/";
#else
#error Add the url to your blob storage account here
                var url = "";
#endif
                CloudBackedStore.Grab( ComponetDir, "\\test.jpg", "components", url + "test.jpg");

                foreach (var app in OdiAppRepo.Apps)
                {
                    if( !string.IsNullOrEmpty(app.PackageName) )
                        CloudBackedStore.Grab(ComponetDir, "\\" + app.PackageName, "components", url + app.PackageName);

                    if (!string.IsNullOrEmpty(app.ConfName))
                        CloudBackedStore.Grab(ComponetDir, "\\" + app.ConfName, "components", url + app.ConfName);

                    var req = app.RequiredFiles;

                    if (req != null)
                    {
                        foreach (var file in req)
                        {
                            CloudBackedStore.Grab(ComponetDir, "\\" + file, "components", url + file);
                        }
                    }
                }

                
            }

        }

        public static void BuildPackage(dynamic data, string dir)
        {
            var app = OdiAppRepo.Apps.Where(a => a.Name == data.name).FirstOrDefault();

            //first build the configuration
            BuildCscfg(data, dir, app);
            var dirName = CloudBackedStore.RootDir + "\\" + dir + "\\Temp\\" + app.Name;
            if( !File.Exists(dirName + "\\" + app.PackageName))
                File.Copy(CloudBackedStore.RootDir + ComponetDir + "\\" + app.PackageName, dirName + "\\" + app.PackageName);

        }

        private static object BuildCscfg(dynamic data, string dir, OdiApp app)
        {
            var dirName = CloudBackedStore.RootDir + "\\" + dir + "\\Temp\\" + app.Name;
            CreateIfNotExists(dirName);

            var fileName = string.Format("{0}\\ServiceConfiguration.cscfg", dirName);
            // create a writer and open the file
            TextWriter tw = new StreamWriter(fileName);

            // write a line of text to the file
            var templateFile = CloudBackedStore.RootDir + ComponetDir + "\\" + app.ConfName;
            var xml = File.ReadAllText(templateFile);

            var sb = new StringBuilder();
            foreach (var conf in app.Configs)
            {
                conf.BuildFromData(data);
                sb.Append(conf.BuildSettingsString());
            }


            if (app.CustomConfigWriter == null)
                tw.WriteLine(string.Format(xml, sb.ToString()));
            else
                tw.WriteLine(app.CustomConfigWriter.Format(xml, data, app));
            

            // close the stream
            tw.Close();

            return fileName;
        }

        public static void CreateIfNotExists(string tmp)
        {
            if (!Directory.Exists(tmp))
                Directory.CreateDirectory(tmp);
        }


        public static string BuildPackageZip(dynamic data, string dir)
        {
            BuildPackage(data, dir);
            var dirName = CloudBackedStore.RootDir + "\\" + dir + "\\Temp\\" + data.name;
            var fileName = string.Format("{0}\\" + data.name + ".zip", CloudBackedStore.RootDir + "\\" + dir);
            using (var zip = new ZipFile(fileName))
            {
                zip.AddDirectory(dirName);
                zip.Save();
            }

            return fileName;
        }
    }
}