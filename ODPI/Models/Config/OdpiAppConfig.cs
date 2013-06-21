using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ODPI.Model.Config
{
    public interface IOdpiAppConfig
    {
        string BuildSettingsString();

        void BuildFromData(dynamic data);

        string Template { get; }

    }
}