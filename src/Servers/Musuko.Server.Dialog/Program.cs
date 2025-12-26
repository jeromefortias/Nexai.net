namespace Musuko.Dialog
{
    using Democrite.Framework.Builders;
    using Democrite.Framework.Core.Abstractions;
    using Musuko.Framework.Core.Helpers;
    using Musuko.Framework.DataModels;
    using Musuko.Framework.DataModels.LLM;
    using Musuko.Framework.DataModels.Corpus;
    using Musuko.Framework.DataModels.Symbolic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Musuko.Framework.Core;

    /// <summary>
    /// Main entry point for the Dialog Server application.
    /// This server handles AI dialog/chat processing using the Democrite framework for distributed agent processing.
    /// It provides REST API endpoints for chat completions, entity management, cache operations, and more.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Session manager instance for handling data persistence and retrieval operations.
        /// </summary>
        static private SessionManager _session;
        
        /// <summary>
        /// Application configuration loaded from config.json.
        /// </summary>
        static private Config _config;

        /// <summary>
        /// Main entry point for the Dialog Server application.
        /// Initializes the web API server with Democrite-based dialog processing pipeline.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            // Load configuration from config.json file
            Config config = ConfigurationManager.GetFromFile<Config>("config.json");
            Console.WriteLine($"Starting {config.AppName} - {config.AppDescription}");
            Console.WriteLine($"Documentation: {config.AppDocumentationUrl}");
            Console.WriteLine("");
            string AppName = config.AppName ?? "APPx";

            // Initialize session manager and configuration
            _config = config;
            _session = new SessionManager(_config);
            _session.LogSave("Application started", AppName, "Info");

            // Create ASP.NET Core web application builder
            var builder = WebApplication.CreateBuilder(args); 

            // Configure Swagger/OpenAPI for API documentation
            builder.Services.AddSwaggerGen(s => s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Musuko - Localhost.ai", Version = "v1" })).AddEndpointsApiExplorer();

            // Build the main dialog processing sequence using Democrite framework
            // This sequence defines the pipeline for processing chat/dialog requests
            // Each step is processed by a virtual grain (VGrain) that handles a specific aspect of dialog processing
            var DialogProcessSeq = Sequence.Build("JFO", fixUid: new Guid("D1F7C7EB-77F7-488A-91D7-77E4D5D16448"), metadataBuilder: m =>
                                         {
                                             m.Description("Dialog Sequence")
                                              .AddTags("chatbot", "nexai");
                                         })
                                         .RequiredInput<string>()  // Sequence expects a string input (completion ID)
                                         // Step 1: Inbound processing - handles incoming dialog requests
                                         .Use<IDialogInboundVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.InboundProcessing(i, ctx)).Return
                                         // Step 2: Context processing - analyzes and enriches context
                                         .Use<IContextProcessorVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.DoProcess(i, ctx)).Return
                                         // Step 3: Emotional inbound processing - analyzes emotional content of input
                                         .Use<IEmotionnalInboundProcessorVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.InboundEmotionnalProcessing(i, ctx)).Return
                                         // Step 4: Cache processing - checks and manages dialog cache
                                         .Use<IDialogCacheProcessorVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.DoProcess(i, ctx)).Return
                                         // Step 5: Pre-processing - prepares request before LLM call
                                         .Use<IDialogPreProcessorVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.PreProcessing(i, ctx)).Return
                                         // Step 6: Main dialog processing - calls LLM and generates response
                                         .Use<IDialogProcessorVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.Processing(i, ctx)).Return
                                         // Step 7: Post-processing - processes LLM response
                                         .Use<IDialogPostProcessorVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.PostProcessing(i, ctx)).Return
                                         // Step 8: Emotional outbound processing - adds emotional layer to response
                                         .Use<IEmotionnalOutboundVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.OutbountEmotionnalProcessing(i, ctx)).Return
                                         // Step 9: Outbound processing - finalizes and formats response
                                         .Use<IDialogOutboundVGrain>().Configure("JFO-TABLE").Call((g, i, ctx) => g.InboundProcessing(i, ctx)).Return
                                         .Build();

            // Build research sequence for search/pre-search operations
            // This sequence handles research and search-related operations
            var ResearchSeq = Sequence.Build("JFO", fixUid: new Guid("D1F7C7EB-77F7-488A-91D7-77E4D5D16450"), metadataBuilder: m =>
            {
                m.Description("Research Sequence")
                 .AddTags("research", "nexai");
            })
                                         .RequiredInput<string>()  // Sequence expects a string input
                                         // Pre-search processing - prepares search queries
                                         .Use<IPreSearchVGrain>().Configure("JFO-TABLE2").Call((g, i, ctx) => g.DoPreProcess(i, ctx)).Return
                                         .Build();

            // Configure Democrite framework for distributed agent processing
            // Democrite provides a virtual actor model (similar to Orleans) for building distributed systems
            builder.Host.UseDemocriteNode(b =>
            {
                b.WizardConfig()
                .NoCluster()  // Run in single-node mode (no clustering)
                .ConfigureLogging(c => c.AddConsole())  // Enable console logging
                .AddInMemoryDefinitionProvider(d => d.SetupSequences(DialogProcessSeq));  // Register dialog processing sequence
                b.ManualyAdvancedConfig().UseDashboard(options =>
                {
                    options.HostSelf = true;  // Host dashboard inside the silo
                    options.Port = 9090;      // Dashboard port (default is 8080)
                });
            });
            var app = builder.Build();
            app.UseSwagger();  // Enable Swagger middleware


            #region "API Endpoints"

            /// <summary>
            /// POST /v1/chat/completions
            /// Main endpoint for chat completion requests (OpenAI-compatible API).
            /// Processes chat messages through the dialog processing sequence.
            /// </summary>
            app.MapPost("v1/chat/completions", async (Request d, [FromServices] IDemocriteExecutionHandler handler) =>
            {
                Console.WriteLine("Received request for /v1/chat/completions");
                //d.model = "mistral-small3.1";  // Uncomment to override model selection
                
                // Check if request already contains a system message
                bool hassystem = d.messages.Exists(m => m.role == "system");
                Console.WriteLine($"System prompt present: {hassystem}");

                // If no system message exists, add a default one
                if (!hassystem)
                {
                    Message systemMessage = new Message
                    {
                        role = "system",
                        content = "be short"  // Default system prompt
                    };
                    d.messages.Insert(0, systemMessage);
                }
                
                // Initialize completion object and save to session
                Completion c = LanguageModelManager.InitializeCompletion();
                c.language = _config.Language;
                c.request = d;
                var id = _session.CompletionSave(c);
                
                // Execute the dialog processing sequence using Democrite
                // The sequence processes the completion ID through all configured grains
                var result = await handler.Sequence<string>(DialogProcessSeq.Uid)
                                       .SetInput(id)
                                       .RunAsync<string>();
                var content = result.SafeGetResult();
                
                // Load the processed completion and return the response
                Response rep = new Response();
                try
                {
                    rep = _session.CompletionLoad(content).response;
                }
                catch (Exception ex)
                {
                    _session.LogSave($"Error in /aiassistant: {ex.Message}", AppName, "ERROR");
                }
                return rep;
            });

            /// <summary>
            /// PUT /entities/save
            /// Saves or updates an entity (person, location, organization, etc.).
            /// </summary>
            app.MapPut("/entities/save", (Entity p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";                
                try
                {
                    result = _session.EntitySave(p);         
                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /entities/save
            /// Alternative endpoint for saving entities (same functionality as PUT).
            /// </summary>
            app.MapPost("/entities/save", (Entity p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";
                try
                {
                    result = _session.EntitySave(p);
                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /entities/search
            /// Searches for entities by name or criteria.
            /// </summary>
            app.MapPost("/entities/search", (SearchId p) =>
            {

                List<Entity> result = new List<Entity>();
                try
                {
                    result = _session.EntitySearchByName(p.Criteria);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /entities/load
            /// Loads a specific entity by its ID.
            /// </summary>
            app.MapPost("/entities/load", (SearchId p) =>
            {
                Entity result = new Entity();
                try
                {
                    result = _session.EntityLoad(p.Id);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /caches/load
            /// Loads a cached dialog completion by ID.
            /// Caches store prompt-completion pairs for reuse.
            /// </summary>
            app.MapPost("/caches/load", (SearchId p) =>
            {
                Cache result = new Cache();
                try
                {
                    result = _session.CacheLoad(p.Id);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /caches/search
            /// Searches for cached completions by value/criteria.
            /// </summary>
            app.MapPost("/caches/search", (SearchId p) =>
            {
                List<Cache> result = new List<Cache>();
                try
                {
                    result = _session.CacheSearchByValue(p.Criteria);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /caches/save
            /// Saves a prompt-completion pair to the cache.
            /// </summary>
            app.MapPost("/caches/save", (Cache p) =>
            {
                string result = "";
                try
                {
                    result = _session.CacheSave(p);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /cv/save
            /// Saves a CV (curriculum vitae) document.
            /// </summary>
            app.MapPost("/cv/save", (Cv p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";
                try
                {
                    result.ReturnedId = _session.CvSave(p);
                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });
            
            /// <summary>
            /// POST /cv/load
            /// Loads a CV document by ID.
            /// </summary>
            app.MapPost("/cv/load", (SearchId p) =>
            {
                Cv result = new Cv();
                try
                {
                    result = _session.CvLoad(p.Id);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });
            
            /// <summary>
            /// POST /cv/search
            /// Searches for CV documents by criteria.
            /// </summary>
            app.MapPost("/cv/search", (SearchId p) =>
            {
                List<Cv> result = new List<Cv>();
                try
                {
                    result = _session.CvSearch(p.Criteria);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Task.FromResult(result);
            });
            
            /// <summary>
            /// POST /cv/delete
            /// Deletes a CV document by ID.
            /// </summary>
            app.MapPost("/cv/delete", (SearchId p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";
                try
                {
                    result.ReturnedId = _session.CvDelete(p.Id);
                    result.Message = "CV deleted successfully";
                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /cv/rewrite
            /// Rewrites/regenerates a CV document (currently placeholder implementation).
            /// </summary>
            app.MapPost("/cv/rewrite", (CvRewriteRequest c) =>
            {
                Cv result = new Cv()  ;
                string myid = result.Id;
                try
                {
                    Cv existingCv = _session.CvLoad(c.id);                    
                    result = existingCv;
                    result.Id = myid;
                    result.Title = "VOILOUUUUU";  // Placeholder - needs proper implementation
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /news
            /// Returns news items (currently returns mock data).
            /// TODO: Implement a real news fetcher to retrieve actual news from external sources.
            /// </summary>
            app.MapPost("/news", (NewsParamater mode) =>
            {
                ///TODO : implement a real news fetcher
                // Currently returns hardcoded news items for testing
                List<News> news = new List<News>()
                {
                    new News() { Title = "New version", Rating = 10, Url ="https://www.Musuko" },
                    new News() { Title = "Virus risk", Rating = 8, Url ="https://www.Musuko" },
                    new News() { Title = "Ma chaine Youtube", Rating = 5, Url ="https://www.Musuko" },
                    new News() { Title = "Moi sur LinkedIn", Rating = -10, Url ="https://www.linkedin.com/in/jerome-fortias/" },
                    new News() { Title = "Tu l'as vu ? ", Rating = -5, Url ="https://www.linkedin.com/in/jerome-fortias/" },
                    new News() { Title = "TEST TEST ", Rating = -5, Url ="https://www.linkedin.com/in/jerome-fortias/" },
                };
                return Task.FromResult(news);
            });

            /// <summary>
            /// POST /log
            /// Saves a log entry to the system.
            /// </summary>
            app.MapPost("/log", (Log p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";
                try
                {
                    result = _session.LogSave(p);

                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });

            // Symbolic processing endpoints - manage symbolic encoders, decoders, and processors
            // These components handle symbolic representation and transformation of data

            /// <summary>
            /// POST /symbolicencoder/load
            /// Loads a symbolic encoder by ID.
            /// Symbolic encoders convert data into symbolic representations.
            /// </summary>
            app.MapPost("/symbolicencoder/load", (SearchId p) =>
            {
                SymbolicEncoder result = new SymbolicEncoder();
                try
                {
                    result = _session.SymbolicEncoderLoad(p.Id);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /symbolicdecoder/load
            /// Loads a symbolic decoder by ID.
            /// Symbolic decoders convert symbolic representations back to data.
            /// </summary>
            app.MapPost("/symbolicdecoder/load", (SearchId p) =>
            {
                SymbolicDecoder result = new SymbolicDecoder();
                try
                {
                    result = _session.SymbolicDecoderLoad(p.Id);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /symbolicprocessor/load
            /// Loads a symbolic processor by ID.
            /// Symbolic processors perform operations on symbolic representations.
            /// </summary>
            app.MapPost("/symbolicprocessor/load", (SearchId p) =>
            {
                SymbolicProcessor result = new SymbolicProcessor();
                try
                {
                    result = _session.SymbolicProcessorLoad(p.Id);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /symbolicencoder/save
            /// Saves a symbolic encoder configuration.
            /// </summary>
            app.MapPost("/symbolicencoder/save", (SymbolicEncoder p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";
                try
                {
                    result.ReturnedId = _session.SymbolicEncoderSave(p);

                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /symbolicdecoder/save
            /// Saves a symbolic decoder configuration.
            /// </summary>
            app.MapPost("/symbolicdecoder/save", (SymbolicDecoder p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";
                try
                {
                    result.ReturnedId = _session.SymbolicDecoderSave(p);
                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /symbolicprocessor/save
            /// Saves a symbolic processor configuration.
            /// </summary>
            app.MapPost("/symbolicprocessor/save", (SymbolicProcessor p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";
                try
                {
                    result.ReturnedId = _session.SymbolicProcessorSave(p);
                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /symbolicencoder/search
            /// Searches for symbolic encoders by criteria.
            /// </summary>
            app.MapPost("/symbolicencoder/search", (SearchId p) =>
            {
                List<SymbolicEncoder> result = new List<SymbolicEncoder>();
                try
                {
                    result = _session.SymbolicEncoderSearch(p.Criteria);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /symbolicdecoder/search
            /// Searches for symbolic decoders by criteria.
            /// </summary>
            app.MapPost("/symbolicdecoder/search", (SearchId p) =>
            {
                List<SymbolicDecoder> result = new List<SymbolicDecoder>();
                try
                {
                    result = _session.SymbolicDecoderSearch(p.Criteria);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Task.FromResult(result);
            });  

            /// <summary>
            /// POST /symbolicprocessor/search
            /// Searches for symbolic processors by criteria.
            /// </summary>
            app.MapPost("/symbolicprocessor/search", (SearchId p) =>
            {
                List<SymbolicProcessor> result = new List<SymbolicProcessor>();
                try
                {
                    result = _session.SymbolicProcessorsSearch(p.Criteria);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Task.FromResult(result);
            });

            /// <summary>
            /// POST /note/save
            /// Saves a note to the system.
            /// </summary>
            app.MapPost("/note/save", (Note p) =>
            {
                ServiceReturn result = new ServiceReturn();
                result.ReturnedId = "";
                result.Message = "";
                try
                {
                    result.ReturnedId = _session.NoteSave(p);
                }
                catch (Exception ex)
                {
                    result.ReturnedId = "-1";
                    result.Message = ex.Message;
                }
                return Task.FromResult(result);
            });

            #endregion

            // Root endpoint redirects to Swagger UI for API documentation
            app.MapGet("/", (request) =>
            {
                request.Response.Redirect("swagger");
                return Task.CompletedTask;
            });
            
            // Enable Swagger UI for interactive API documentation
            app.UseSwaggerUI();
            
            // Start the web application
            app.Run();
        }
    }
}
