using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODI.Actions.PostDeploy
{
    public interface IPostDeployAction
    {
        void PerformAction(dynamic data);
    }
}
