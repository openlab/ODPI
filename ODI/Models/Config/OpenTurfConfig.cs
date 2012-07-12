using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODI.Model.Config
{
    public class OpenTurfConfig : IOdiAppConfig
    {
        public string DbName { get; set; }
        public string DbHost { get; set; }
        public string DbUserName { get; set; }
        public string DbUserPassword { get; set; }
        public string AppName { get; set; }
        public string BitlyLogin { get; set; }
        public string BitlyKey { get; set; }
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }
        public string TwitterCallbackUrl { get; set; }
        public string TwitterAppUrl { get; set; }
        public string AdminTwitterUser { get; set; }

        public string BuildSettingsString()
        {
            string template = @"<Setting name=""ODAF"" 
                                    value=""Server=tcp:{0};Database={1};User ID={2};Password={3};Trusted_Connection=False;Encrypt=True;"" />
                                    <Setting name=""DiagnosticsConnectionString"" value=""UseDevelopmentStorage=true"" />
                                    <Setting name=""AppName"" value=""{4}"" />
                                    <Setting name=""BitlyLogin"" value=""{5}"" />
                                    <Setting name=""BitlyAPIKey"" value=""{6}"" />
                                    <Setting name=""tokenExpiryMinutes"" value=""10080"" />
                                    <Setting name=""UseTokenExpiry"" value=""false"" />
                                    <Setting name=""AdminTwitterUser"" value=""{7}"" />
                                    <Setting name=""TwitterAppId"" value=""D5B672D4-7B1C-46cc-8643-FBE8334F4ADF"" />
                                    <Setting name=""DevMode"" value=""false"" />
                                ";

            return string.Format(template, DbHost, DbName, DbUserName, DbUserPassword, AppName, BitlyLogin, BitlyKey, AdminTwitterUser);
        }

        public void BuildFromData(dynamic data)
        {
            DbName = data.dbname;
            DbHost = data.dbhost;
            DbUserName = data.dbusername;
            DbUserPassword = data.dbpassword;
            AppName = data.appname;
            BitlyLogin = data.bitlylogin;
            BitlyKey = data.bitlykey;
            TwitterConsumerKey = data.twitterconsumerkey;
            TwitterConsumerSecret = data.twitterconsumersecret;
            TwitterCallbackUrl = data.twittercallbackurl;
            TwitterAppUrl = data.twitterappurl;
            AdminTwitterUser = data.admintwitteruser;
        }

        public string Template { get { return "OpenTurf"; } }

        
    }
}