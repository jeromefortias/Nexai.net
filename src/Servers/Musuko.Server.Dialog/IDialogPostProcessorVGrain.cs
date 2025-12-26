using Democrite.Framework.Core.Abstractions;
using Democrite.Framework.Core.Abstractions.Attributes;
using Democrite.Framework.Core.Abstractions.Attributes.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musuko.Dialog
{
    using Musuko.Core.Models.LLM;
    [VGrainCategory("PostProcessor")]
    //[VGrainMetaData()]
    public interface IDialogPostProcessorVGrain : IVGrain
    {
        Task<string> PostProcessing(string completionId, IExecutionContext Context);
    }
}
