using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Encryption;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

public class MessageBodyEncryption
{
    public void Simple()
    {
        #region UsingMessageBodyEncryptorV5

        var configuration = new BusConfiguration();
        configuration.RegisterComponents(c => c.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall));

        #endregion
    }
}

#region MessageBodyEncryptorV5

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