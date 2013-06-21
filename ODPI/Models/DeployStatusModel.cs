using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODPI.Models
{
    public class DeployStatusModel
    {
        public DeployStatusModelStatus Status { get; set; }
        public string Stage { get; set; }
        public string LogMessage { get; set; }
        public string Data { get; set; }
        public string StackTrace { get; set; }
    }

    public enum DeployStatusModelStatus
    {
        Ok = 0,
        Error = 1,
        Inprogress = 2
    }
}