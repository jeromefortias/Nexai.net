namespace Nexai.net.Core.Config.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DataBaseServer
    {
        public string dbType { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string connString { get; set; }
        public string prefix { get; set; }
        public string layer { get; set; }
    }

}
