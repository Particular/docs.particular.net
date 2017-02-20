using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

namespace Shared
{
    class HardcodeReplyToAddressToLogicalAddress : IBehavior<IOutgoingPhysicalMessageContext, IOutgoingPhysicalMessageContext>
    {
        private string instanceSpecificQueue;

        public HardcodeReplyToAddressToLogicalAddress(string instanceSpecificQueue)
        {
            this.instanceSpecificQueue = instanceSpecificQueue;
        }

        public Task Invoke(IOutgoingPhysicalMessageContext context, Func<IOutgoingPhysicalMessageContext, Task> next)
        {
            NoReplyToAddressOverride noOverride;
            if (instanceSpecificQueue != null && !context.Extensions.TryGet(out noOverride))
            {
                context.Headers[Headers.ReplyToAddress] = instanceSpecificQueue;
            }

            return next(context);
        }

        public struct NoReplyToAddressOverride { }
    }
}