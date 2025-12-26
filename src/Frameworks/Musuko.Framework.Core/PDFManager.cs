using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musuko.Framework.Core
{
    using Musuko.Core.Data;
    using Musuko.Framework.DataModels;
    using Musuko.Framework.DataModels.Corpus; //Musuko.Framework.DataModels
    using Musuko.Framework.DataModels.LLM;
    using Musuko.Framework.DataModels.Symbolic;
    /// <summary>
    /// Provides specialized functionality for PDF document processing and management.
    /// Extends SessionManager to add PDF-specific operations.
    /// </summary>
    public class PDFManager : SessionManager
    {
        Config _config;
        
        /// <summary>
        /// Initializes a new instance of the PDFManager with the specified configuration.
        /// </summary>
        /// <param name="config">The configuration object containing application settings and repository connections.</param>
        /// <exception cref="Exception">Thrown when repository initialization fails.</exception>
        public PDFManager(Config config) : base(config)
        {
            _config = config;
            try
            {
                if (_config.DataRepository != null)
                {
                    ElasticSearchServerParameters = _config.DataRepository;
                    ElasticSearchServer = new ElasticRepository(ElasticSearchServerParameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
