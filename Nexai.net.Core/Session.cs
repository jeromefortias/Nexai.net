namespace Nexai.net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Nexai.net.Core.Data;
    using Nexai.net.Core.Config.Models;

    public class Session
    {
        private static Session instance ;
        public DateTime startAt;
        private static object locker = new object();
        private DataBaseServer _dataBaseServerAdmin;
        private DataBaseServer _dataBaseServerData;
        private DataBaseServer _dataBaseServerAnalysis;
        private DataBaseServer _dataBaseServerCorpus;
        private ConfigCommon _configCommon;

        public Session(ConfigCommon config) 
        { 
            _configCommon = config;
            startAt = DateTime.Now;
            try
            {
                List<DataBaseServer> servers = new List<DataBaseServer>();
                servers = _configCommon.dataServers;
                var AdminServers = servers.Where(servers => servers.layer == "admin");
                foreach ( DataBaseServer server in AdminServers )
                {
                    _dataBaseServerAdmin = server;
                }
                var DataServers = servers.Where(servers => servers.layer == "data");
                foreach (DataBaseServer server in DataServers)
                {
                    _dataBaseServerData = server;
                }

                var AnalysisServers = servers.Where(servers => servers.layer == "analysis");
                foreach (DataBaseServer server in AnalysisServers)
                {
                    _dataBaseServerAnalysis = server;
                }

                var CorpusServers = servers.Where(servers => servers.layer == "corpus");
                foreach (DataBaseServer server in CorpusServers)
                {
                    _dataBaseServerAnalysis = server;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Session getInstance(ConfigCommon config) 
        {
            try
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new Session(config);
                        }
                    }
                }
                return instance;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
