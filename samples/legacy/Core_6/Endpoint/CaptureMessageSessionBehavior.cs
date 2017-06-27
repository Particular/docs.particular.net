using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

class CaptureMessageSessionBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public static AsyncLocal<IMessageProcessingContext> CurrentSession = new AsyncLocal<IMessageProcessingContext>();

    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        CurrentSession.Value = context;
        await next().ConfigureAwait(false);
        CurrentSession.Value = null;
    }
}