using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Transport;

#region mutate-incoming-transport-messages
public class MutateIncomingTransportMessages : IMutateIncomingTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Headers
            .Add("MutateIncomingTransportMessages", "ValueMutateIncomingTransportMessages");
    }
}
#endregion