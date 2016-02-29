using System;
using System.Threading;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

public class DebugFlagMutator :
    IMutateTransportMessages,
    INeedInitialization
{
    public static bool Debug
    {
        get { return debug.Value; }
    }

    public void MutateIncoming(TransportMessage transportMessage)
    {
        string debugFlag = transportMessage.Headers.ContainsKey("Debug") ? transportMessage.Headers["Debug"] : "false";
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
        busConfiguration.RegisterComponents(c => c.ConfigureComponent<DebugFlagMutator>(DependencyLifecycle.InstancePerCall));
    }
}