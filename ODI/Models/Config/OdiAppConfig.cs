using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ODI.Model.Config
{
    public interface IOdiAppConfig
    {
        string BuildSettingsString();

        void BuildFromData(dynamic data);

        string Template { get; }

    }
}