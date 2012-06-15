using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODI.Services
{
    public static class StatusMessageConverter
    {
        private static Dictionary<string, string> Converter = new Dictionary<string, string>();

        static StatusMessageConverter()
        {
            Converter.Add( "BusyRole", "Deployment Busy" );
            Converter.Add( "StartingVM", "Starting the Virtual Machine for Application");
            Converter.Add( "CreatingVM", "Creating the Virtual Machine for Application");
            Converter.Add( "RoleStateUnknown", "The deployment status is unknown (this is an expected message)");
            Converter.Add( "Succeeded", "Upload to Blob Storage was Successfull");
            Converter.Add("InProgress", "The upload is in progress");
        }

        public static string Convert(string key)
        {
            var ret = "";

            if (!Converter.TryGetValue(key, out ret))
                ret = key;

            return ret;
        }
    }
}