using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

public class DebugFlagMutator : 
    IMutateIncomingTransportMessages,
    IMutateOutgoingTransportMessages, 
    INeedInitialization
{
    public static bool Debug => debug.Value;

    static ThreadLocal<bool> debug = new ThreadLocal<bool>();

    public void Customize(BusConfiguration configuration)
    {
        configuration.RegisterComponents(c => c.ConfigureComponent<DebugFlagMutator>(DependencyLifecycle.InstancePerCall));
    }


    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        string debugFlag = context.Headers.ContainsKey("Debug") ? context.Headers["Debug"] : "false";
        if (debugFlag != null && debugFlag.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            debug.Value = true;
        }
        else
        {
            debug.Value = false;
        }
        return Task.FromResult(0);
    }

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        context.OutgoingHeaders["Debug"]= Debug.ToString();
        return Task.FromResult(0);
    }
}