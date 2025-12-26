namespace Musuko.Dialog
{
    using Democrite.Framework.Core;
    using Democrite.Framework.Core.Abstractions;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Musuko.Framework.DataModels.LLM;
    using Musuko.Framework.DataModels;
    using Musuko.Framework.Core.Helpers;
    using Action = Musuko.Framework.DataModels.LLM.Action;
    using Musuko.Framework.Core;

    internal class EmotionnalInboundProcessorVGrain : VGrainBase<IEmotionnalInboundProcessorVGrain>, IEmotionnalInboundProcessorVGrain
    {
        private Config _config;
        private SessionManager _session;
        private Completion _completion;

        public EmotionnalInboundProcessorVGrain(ILogger<IEmotionnalInboundProcessorVGrain> logger) : base(logger)
        {
            /// Initialization of the vgrain
        }

        public bool Activation()
        {
            return true;
        }

        public async Task<string> InboundEmotionnalProcessing(string completionId, IExecutionContext Context)
        {
            if(Activation() == false)
            {
                return completionId;
            }
            _config = ConfigurationManager.GetFromFile<Config>("config.json");
            _session = new SessionManager(_config);
            _session.LogSave("EMOTIONNALINBOUND - InboundEmotionnalProcessing", _config.AppName, "INFO");
            _completion = _session.CompletionLoad(completionId);

            if (_completion != null)
            {
                Console.WriteLine($"EMOTIONNALINBOUND - DialogInboundVGrain: Completion loaded with Id {completionId}");                
                DoProcess();
                _session.CompletionSave(_completion);
                _session.LogSave($"EMOTIONNALINBOUND - Completion with Id {completionId} saved", _config.AppName, "DONE");
            }
            else
            {
                _session.LogSave($"EMOTIONNALINBOUND - Completion with Id {completionId} not found", _config.AppName, "ERROR");
            }
            return completionId;
        }

        private void DoProcess()
        {
            try
            {
                Thread.Sleep(_config.LatencyMs);
                _completion.actions.Add(new Action
                {
                    typeOfAction = "EmotionnalInboundProcessor",
                    timeOfAction = DateTime.Now,
                    ActionBy = _config.AppName
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
