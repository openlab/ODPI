using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODI.Actions.Validate
{
    public interface IValidateAction
    {
        string Validate(dynamic data);
    }
}