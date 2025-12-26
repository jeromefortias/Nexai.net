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


    
    [VGrainCategory("InboundConnector")]
    //[VGrainMetaData()]
    public interface IDialogInboundVGrain : IVGrain
    {
        Task<string> InboundProcessing(string completionId, IExecutionContext Context );

    }
}
