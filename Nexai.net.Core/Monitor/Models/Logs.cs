using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Enumeration;

namespace Nexai.net.Core.Logs.Models
{
    public class Logs
    {
        public DateTime dateTime { get; set; }
        public string nodeName { get; set; }
        public string methodName { get; set; }
        public string type { get; set; }
        public int level { get; set; }
    }


    public static class LogTypes
    {
        public static readonly string Event = "Event";
        public static readonly string Log = "Log";
        public static readonly string Alert = "Alert";
    }

}
