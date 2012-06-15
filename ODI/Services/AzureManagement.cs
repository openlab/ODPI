using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ODI.Service
{
    public class AzureManagement
    {

        public static string Deploy(string subscriptionId, string serviceName, byte[] privCert, string packageUrl, string cscfgFile)
        {
            var name = Guid.NewGuid().ToString();
            var template ="<CreateDeployment xmlns=\"http://schemas.microsoft.com/windowsazure\"><Name>{0}</Name>" +
                          "<PackageUrl>{1}</PackageUrl><Label>{2}</Label><Configuration>{3}</Configuration>" +
                          "<StartDeployment>true</StartDeployment></CreateDeployment>";
           // var template = "<ChangeConfiguration xmlns=\"http://schemas.microsoft.com/windowsazure\">" +
           //     "<Configuration>{0}</Configuration></ChangeConfiguration>";
            var req = string.Format(template, 
                                        name, //"ODI Deployment", 
                                        packageUrl,  
                                        Convert.ToBase64String(Encoding.UTF8.GetBytes(name)),
                                        Convert.ToBase64String(File.ReadAllBytes(cscfgFile)));

            var reqDoc = XDocument.Parse(req);
            var reqFormatted = Convert.ToBase64String(Encoding.UTF8.GetBytes(reqDoc.ToString(SaveOptions.DisableFormatting)));

            string ret = "";
            X509Certificate2 certificate = null;

            // Request and response variables.
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            // Stream variables.
            Stream responseStream = null;
            StreamReader reader = null;


            //certificate = certCollection[0];
            certificate = new X509Certificate2(privCert, "password");

            // Create the request.
            var url = String.Format("https://management.core.windows.net/{0}/services/hostedservices/{1}/deploymentslots/production", 
                subscriptionId, serviceName);

           // var url = string.Format("https://management.core.windows.net/{0}/services/hostedservices/{1}/deploymentslots/production/?comp=config",
           //     subscriptionId, serviceName);

            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("x-ms-version", "2011-10-01");
            httpWebRequest.ContentType = "application/xml";

            // Add the certificate to the request.
            httpWebRequest.ClientCertificates.Add(certificate);

            // Add request body to the web request.
            byte[] requestBytes = Encoding.UTF8.GetBytes(req);
            httpWebRequest.ContentLength = requestBytes.Length;
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(requestBytes, 0, requestBytes.Length);
                stream.Close();
            }

            try
            {
                // Make the call using the web request.
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                // Display the web response status code.
                Console.WriteLine("Response status code: " + httpWebResponse.StatusCode);

                // Display the request ID returned by Windows Azure.
                if (null != httpWebResponse.Headers)
                {
                    ret =  httpWebResponse.Headers["x-ms-request-id"];
                }

                // Parse the web response.
                responseStream = httpWebResponse.GetResponseStream();
                reader = new StreamReader(responseStream);
                // Display the raw response.
                var output = reader.ReadToEnd();
                Console.WriteLine("Response output: " + output);
                //ret = reader.ReadToEnd();

                // Close the resources no longer needed.
                httpWebResponse.Close();
                responseStream.Close();
                reader.Close();
            }
            catch (WebException we)
            {
                var rs = we.Response.GetResponseStream();
                var val = (new StreamReader(rs)).ReadToEnd();
            }
            return ret;
        }

        public static string GetStatusUpdate(string subscriptionId, byte[] privCert, string token)
        {
            var ret = "";
            try
            {
                var requestUri = new Uri("https://management.core.windows.net/"
                                 + subscriptionId
                                 + "/operations/"
                                 + token);
                ret = PerformServiceCall(requestUri, privCert);
            }
            catch (Exception e)
            {
                ret = "<error><exception>" + e.Message + "</exception><stacktrace>" + e.StackTrace + "</stacktrace></error>";
            }
            return ret;
        }

        public static string GetStorageServices(string subscriptionId, byte[] privCert)
        {
            var ret = "";
            try
            {
                var requestUri = new Uri("https://management.core.windows.net/"
                                 + subscriptionId
                                 + "/services/storageservices");
                ret = PerformServiceCall(requestUri, privCert);
            }
            catch (Exception e)
            {
                ret = "<exception>" + e.Message + "</exception>";
            }
            return ret;
        }

        public static string GetHostedServices(string subscriptionId, byte[] privCert)
        {
            var ret = "";
            
            var requestUri = new Uri("https://management.core.windows.net/"
                                + subscriptionId
                                + "/services/hostedservices");
            ret = PerformServiceCall(requestUri, privCert );
           
            return ret;
        }

        private static string PerformServiceCall(Uri requestUri, byte[] privCert)
        {
            string ret = "";
            X509Certificate2 certificate = null;

            // Request and response variables.
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            // Stream variables.
            Stream responseStream = null;
            StreamReader reader = null;



            //certificate = certCollection[0];
            certificate = new X509Certificate2(privCert, "password");

            // Create the request.
            

            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);

            // Add the certificate to the request.
            httpWebRequest.ClientCertificates.Add(certificate);

            // Specify the version information in the header.
            httpWebRequest.Headers.Add("x-ms-version", "2011-10-01");

            // Make the call using the web request.
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            // Display the web response status code.
            Console.WriteLine("Response status code: " + httpWebResponse.StatusCode);

            // Display the request ID returned by Windows Azure.
            if (null != httpWebResponse.Headers)
            {
                Console.WriteLine("x-ms-request-id: "
                + httpWebResponse.Headers["x-ms-request-id"]);
            }

            // Parse the web response.
            responseStream = httpWebResponse.GetResponseStream();
            reader = new StreamReader(responseStream);
            // Display the raw response.
            Console.WriteLine("Response output:");
            ret = reader.ReadToEnd();

            // Close the resources no longer needed.
            httpWebResponse.Close();
            responseStream.Close();
            reader.Close();
            return ret;
        }

        public static string GetDeployStatusUpdate(string subscriptionId, byte[] privCert, string ss)
        {
            var ret = "";
            try
            {
                var requestUri = new Uri("https://management.core.windows.net/"
                                 + subscriptionId
                                 + "/services/hostedservices/"
                                 + ss
                                 + "/deploymentslots/production");
                ret = PerformServiceCall(requestUri, privCert);
            }
            catch (Exception e)
            {
                ret = "<exception>" + e.Message + "</exception>";
            }
            return ret;
        }
    }
}