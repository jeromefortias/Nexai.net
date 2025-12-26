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
    [VGrainCategory("DialogProcessor")]
    //[VGrainMetaData()]
    public interface IDialogProcessorVGrain : IVGrain
    {
        Task<string> Processing(string completionId, IExecutionContext Context);

    }
}
