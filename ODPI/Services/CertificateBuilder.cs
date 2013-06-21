using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace ODPI.Service
{
    public static class CertificateBuilder
    {
        private static string command = "-r -pe -a sha1 -n \"CN=ODPI Certificate\" -ss My -len 2048 -sp \"Microsoft Enhanced RSA and AES Cryptographic Provider\" -sy 24 {0}\\oidcertificate.cer";

        public static void Initialize()
        {
            if (!CloudBackedStore.DirectoryExists(CertDir))
            {
                var url = string.Format("http://{0}.blob.core.windows.net/components/", RoleEnvironment.GetConfigurationSettingValue("StorageName"));
                CloudBackedStore.Grab(CertDir, "\\makecert.exe", "components", url + "makecert.exe");
            }
        }

        public static string MakeCertificate(string dir)
        {

            try
            {

                if (!CloudBackedStore.DirectoryExists(dir + "\\" + CertDir))
                    CloudBackedStore.CreateDirectory(dir + "\\" + CertDir);
                
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = CloudBackedStore.RootDir + CertDir + "\\makecert.exe";
                start.Arguments = string.Format(command, CloudBackedStore.RootDir + "\\" + dir + "\\" + CertDir );
                start.WorkingDirectory = CloudBackedStore.RootDir + CertDir;
                //start.Verb = "runas";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process process = Process.Start(start))
                {
                    process.WaitForExit();

                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                }

                //extract private key
            }
            catch(Exception e)
            {
                var inner = e.InnerException;
            }

            return CloudBackedStore.RootDir + "\\" + dir + "\\" + CertDir + "\\oidcertificate.cer";
        }

        public static byte[] GetPrivateKey(string dir)
        {
            var fileName = CloudBackedStore.RootDir + "\\" + dir + "\\" + CertDir + "\\oidcertificate.cer";
            var cert = new X509Certificate2(fileName);
            var priv = cert.Export(X509ContentType.Pfx, "password");
            return priv;
        }

        public static string CertDir { get { return "Certificates"; } }
        public static string MakeCertFile { get { return CloudBackedStore.RootDir + CertDir + "\\makecert.exe"; } }
    }
}