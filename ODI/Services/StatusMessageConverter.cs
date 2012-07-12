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
            Converter.Add( "BusyRole", ODI.Resources.Services.StatusMessageConverterResource.DeploymentBusy);
            Converter.Add( "StartingVM", ODI.Resources.Services.StatusMessageConverterResource.StartingTheVirtualMachine);
            Converter.Add( "CreatingVM", ODI.Resources.Services.StatusMessageConverterResource.CreatingTheVirtualMachine);
            Converter.Add( "RoleStateUnknown", ODI.Resources.Services.StatusMessageConverterResource.ThedeploymentStatusIsUnknown);
            Converter.Add( "Succeeded", ODI.Resources.Services.StatusMessageConverterResource.UploadToBlobStorageSuccessful);
            Converter.Add("InProgress", ODI.Resources.Services.StatusMessageConverterResource.TheUploadIsInProgress);
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