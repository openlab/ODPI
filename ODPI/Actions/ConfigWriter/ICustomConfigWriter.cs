using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ODPI.Models;

namespace ODPI.Actions.ConfigWriter
{
    public interface ICustomConfigWriter
    {
        string Format(string xml, dynamic data, OdpiApp app);
    }
}
