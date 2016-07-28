namespace Core3.Encryption.MessageBody
{
    using System.Linq;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;

    #region MessageBodyEncryptor

    public class MessageEncryptor :
        IMutateTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            transportMessage.Body = transportMessage.Body.Reverse().ToArray();
        }

        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            transportMessage.Body = transportMessage.Body.Reverse().ToArray();
        }
    }

    #endregion
}