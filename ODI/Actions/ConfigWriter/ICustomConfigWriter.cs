using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ODI.Models;

namespace ODI.Actions.ConfigWriter
{
    public interface ICustomConfigWriter
    {
        string Format(string xml, dynamic data, OdiApp app);
    }
}
