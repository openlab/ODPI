using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ODPI.Models;

namespace ODPI.Actions.ConfigWriter
{
    public class DuplicateConfigWriter : ICustomConfigWriter
    {
        public string Format(string xml, dynamic data, OdpiApp app)
        {
            var sb = new StringBuilder();
            foreach (var conf in app.Configs)
            {
                conf.BuildFromData(data);
                sb.Append(conf.BuildSettingsString());
            }

            return string.Format(xml, sb.ToString(), sb.ToString());
        }
    }
}