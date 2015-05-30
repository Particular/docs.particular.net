using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-incoming-transport-messages
public class MutateIncomingTransportMessages : IMutateIncomingTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Headers
            .Add("KeyFromMutateIncomingTransportMessages", "ValueFromMutateIncomingTransportMessages");
    }
}
#endregion