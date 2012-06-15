using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODI.Model.Config;
using ODI.Actions.PostDeploy;
using ODI.Actions.Validate;
using ODI.Actions.ConfigWriter;

namespace ODI.Models
{
    public class OdiApp
    {

        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PackageName { get; set; }
        public string ConfName { get; set; }
        public IOdiAppConfig[] Configs { get; set; }
        public string[] RequiredFiles { get; set; }
        public IPostDeployAction PostAction { get; set; }
        public IValidateAction[] Validations { get; set; }
        public string SiteUrl { get; set; }
        public ICustomConfigWriter CustomConfigWriter { get; set; }
        public string ConfigHelpText { get; set; }

    }
}