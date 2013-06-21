using ODPI.Actions.ConfigWriter;
using ODPI.Actions.PostDeploy;
using ODPI.Actions.Validate;
using ODPI.Model.Config;

namespace ODPI.Models
{
    public class OdpiApp
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PackageName { get; set; }
        public string ConfName { get; set; }
        public IOdpiAppConfig[] Configs { get; set; }
        public string[] RequiredFiles { get; set; }
        public IPostDeployAction PostAction { get; set; }
        public IValidateAction[] Validations { get; set; }
        public string SiteUrl { get; set; }
        public ICustomConfigWriter CustomConfigWriter { get; set; }
        public string ConfigHelpText { get; set; }
    }
}