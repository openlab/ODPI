using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODI.Models
{
    public class CheckMessage
    {
        public CheckMessageStatus Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }

    public enum CheckMessageStatus
    {
        Ok = 0,
        Error = 1
    }
}