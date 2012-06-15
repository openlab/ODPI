using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODI.Model.Config
{
    public class SqlAzureConfig : IOdiAppConfig
    {
        public string DbName { get; set; }
        public string DbHost { get; set; }
        public string DbUserName { get; set; }
        public string DbUserPassword { get; set; }

        public string BuildSettingsString()
        {
            string template = @" <Setting name=""sql_azure_database"" value=""{0}"" />
                                      <Setting name=""sql_azure_username"" value=""{1}"" />
                                      <Setting name=""sql_azure_password"" value=""{2}"" />
                                      <Setting name=""sql_azure_host"" value=""{3}"" />";

            return string.Format(template, DbName, DbUserName, DbUserPassword, DbHost);
        }

        public void BuildFromData(dynamic data)
        {
            DbName = data.dbname;
            DbHost = data.dbhost;
            DbUserName = data.dbusername;
            DbUserPassword = data.dbpassword;
        }

        public string Template { get { return "SqlAzure"; } }

        
    }
}