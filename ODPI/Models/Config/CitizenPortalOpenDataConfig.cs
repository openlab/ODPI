using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODPI.Model.Config;

namespace ODPI.Model.Config
{
    public class CitizenPortalOpenDataConfig : IOdpiAppConfig
    {
        public string BaseDataServiceUrl { get; set; }
        public string InstitutionName { get; set; }
        public string LogoUri { get; set; }
        public string WelcomeText { get; set; }
        public string InstitutionDecription { get; set; }
        public string InstitutionImage { get; set; }

        public string BuildSettingsString()
        {
            string template = @"<Setting name=""BaseDataServiceUrl"" value=""{0}"" />
                              <Setting name=""InstitutionName"" value=""{1}"" />
                              <Setting name=""LogoUri"" value=""{2}"" />
                              <Setting name=""WelcomeText"" value=""{3}"" />
                              <Setting name=""InstitutionDescription"" value=""{4}"" />
                              <Setting name=""InstitutionImage"" value=""{5}"" />
      
                                ";

            return string.Format(template, BaseDataServiceUrl, InstitutionName, LogoUri, WelcomeText, InstitutionDecription, InstitutionImage);
        }

        public void BuildFromData(dynamic data)
        {
            BaseDataServiceUrl = data.basedataserviceurl;
            InstitutionName = data.institutionname;
            LogoUri = data.logouri;
            WelcomeText = data.welcometext;
            InstitutionDecription = data.institutiondescription;
            InstitutionImage = data.institutionimage;
        }

        public string Template
        {
            get { return "CitizenPortalOpenData"; }
        }
    }
}