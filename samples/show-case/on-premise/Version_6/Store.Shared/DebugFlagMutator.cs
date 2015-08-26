namespace Store.Common
{
    using System;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.MessageMutator;

    public class DebugFlagMutator : 
        IMutateIncomingTransportMessages,
        IMutateOutgoingTransportMessages, 
        INeedInitialization
    {
        public static bool Debug { get { return debug.Value; } }

        public void MutateIncoming(TransportMessage transportMessage)
        {
            var debugFlag = transportMessage.Headers.ContainsKey("Debug") ? transportMessage.Headers["Debug"] : "false";
            if (debugFlag !=null && debugFlag.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                debug.Value = true;
            }
            else
            {
                debug.Value = false;
            }
        }

        static ThreadLocal<bool> debug = new ThreadLocal<bool>();


        public void Customize(BusConfiguration configuration)
        {
            configuration.RegisterComponents(c => c.ConfigureComponent<DebugFlagMutator>(DependencyLifecycle.InstancePerCall));
        }

        public void MutateOutgoing(MutateOutgoingTransportMessagesContext context)
        {
            context.SetHeader("Debug", Debug.ToString());
        }
    }
}
