using System;
using System.Threading;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

public class DebugFlagMutator :
    IMutateTransportMessages,
    INeedInitialization
{
    public static bool Debug => debug.Value;

    public void MutateIncoming(TransportMessage transportMessage)
    {
        var headers = transportMessage.Headers;
        var debugFlag = headers.ContainsKey("Debug") ? headers["Debug"] : "false";
        if (debugFlag != null && debugFlag.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            debug.Value = true;
        }
        else
        {
            debug.Value = false;
        }
    }

    public void MutateOutgoing(LogicalMessage message, TransportMessage transportMessage)
    {
        transportMessage.Headers["Debug"] = Debug.ToString();
    }

    static ThreadLocal<bool> debug = new ThreadLocal<bool>();


    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<DebugFlagMutator>(DependencyLifecycle.InstancePerCall);
            });
    }
}