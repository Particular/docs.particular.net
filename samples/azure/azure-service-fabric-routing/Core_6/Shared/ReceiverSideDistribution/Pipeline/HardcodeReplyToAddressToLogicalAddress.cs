using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

namespace Shared
{
    class HardcodeReplyToAddressToLogicalAddress : IBehavior<IOutgoingPhysicalMessageContext, IOutgoingPhysicalMessageContext>
    {
        private LogicalAddress logicalAddress;

        public HardcodeReplyToAddressToLogicalAddress(LogicalAddress logicalAddress)
        {
            this.logicalAddress = logicalAddress;
        }

        public Task Invoke(IOutgoingPhysicalMessageContext context, Func<IOutgoingPhysicalMessageContext, Task> next)
        {
            NoReplyToAddressOverride noOverride;
            if (logicalAddress != default(LogicalAddress) && !context.Extensions.TryGet(out noOverride))
            {
                context.Headers[Headers.ReplyToAddress] = logicalAddress.ToString();
            }

            return next(context);
        }

        public struct NoReplyToAddressOverride { }
    }
}