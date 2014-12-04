using System.Linq;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

public class MessageBodyEncryption
{
    public void Simple()
    {
        #region UsingMessageBodyEncryptor

        var configuration = new BusConfiguration();
        configuration.RegisterComponents(c => c.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall));

        #endregion
    }
}

#region MessageBodyEncryptor

public class MessageEncryptor : IMutateTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Body = transportMessage.Body.Reverse().ToArray();
    }

    public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
    {
        transportMessage.Body = transportMessage.Body.Reverse().ToArray();
    }
}
#endregion