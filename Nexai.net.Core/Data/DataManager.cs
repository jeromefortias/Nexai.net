using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Nexai.net.Core.Config.Models;

namespace Nexai.net.Core.Data
{
    public class DataManager
    {
        private ConnectionSettings _connectionSettings;
        private ElasticClient _elasticClient;
        private string _prefix;
        private string _layer;
        public DataManager(DataBaseServer dataServer) 
        {
            try
            { 
                _connectionSettings = new ConnectionSettings(new Uri(dataServer.connString));
                _prefix = dataServer.prefix;
                _layer = dataServer.layer;
                _elasticClient = new ElasticClient(_connectionSettings);
            }
            catch(Exception ex) 
            {
                throw;
            }
        }
    }
}
