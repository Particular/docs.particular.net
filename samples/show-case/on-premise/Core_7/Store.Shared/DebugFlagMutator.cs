using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.MessageMutator;

public class DebugFlagMutator :
    IMutateIncomingTransportMessages,
    IMutateOutgoingTransportMessages
{
    public static bool Debug => debug.Value;

    static ThreadLocal<bool> debug = new ThreadLocal<bool>();
    
    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var headers = context.Headers;
        var debugFlag = headers.ContainsKey("Debug") ? headers["Debug"] : "false";
        if (debugFlag != null && debugFlag.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            debug.Value = true;
        }
        else
        {
            debug.Value = false;
        }
        return Task.CompletedTask;
    }

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        context.OutgoingHeaders["Debug"] = Debug.ToString();
        return Task.CompletedTask;
    }
}