namespace Nexai.net.Core.Config.Models
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ConfigCommon
    {
        /// <summary>
        /// Name of the machine
        /// </summary>
        public string machineName { get; set; }
        /// <summary>
        /// Name of the app instance
        /// </summary>
        public string nodeName { get; set; }
        /// <summary>
        /// Binary name
        /// </summary>
        public string appName { get; set; }
        /// <summary>
        /// Binary version
        /// </summary>
        public string appVersion { get; set; }
        /// <summary>
        /// Default user OR a hardcoded username
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// Explaination of the role of the agent
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// List of servers
        /// </summary>
        public List<DataBaseServer> dataServers { get; set; }
        public bool testMode { get; set; }
    }
}
