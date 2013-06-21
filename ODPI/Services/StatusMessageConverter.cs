using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODPI.Services
{
    public static class StatusMessageConverter
    {
        private static Dictionary<string, string> Converter = new Dictionary<string, string>();

        static StatusMessageConverter()
        {
            Converter.Add( "BusyRole", ODPI.Resources.Services.StatusMessageConverterResource.DeploymentBusy);
            Converter.Add( "StartingVM", ODPI.Resources.Services.StatusMessageConverterResource.StartingTheVirtualMachine);
            Converter.Add( "CreatingVM", ODPI.Resources.Services.StatusMessageConverterResource.CreatingTheVirtualMachine);
            Converter.Add( "RoleStateUnknown", ODPI.Resources.Services.StatusMessageConverterResource.ThedeploymentStatusIsUnknown);
            Converter.Add( "Succeeded", ODPI.Resources.Services.StatusMessageConverterResource.UploadToBlobStorageSuccessful);
            Converter.Add("InProgress", ODPI.Resources.Services.StatusMessageConverterResource.TheUploadIsInProgress);
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