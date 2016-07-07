namespace Core4.MessageIdentity
{
    using NServiceBus;
    using NServiceBus.MessageMutator;

    #region MessageId-Mutator
    public class MessageIdFromMessageMutator : IMutateOutgoingMessages
    {
        public MessageIdFromMessageMutator(IBus bus)
        {
            this.bus = bus;
        }

        public object MutateOutgoing(object message)
        {
            bus.SetMessageHeader(message, "NServiceBus.MessageId", GenerateIdForMessage(message));

            return message;
        }

        string GenerateIdForMessage(object message)
        {
            throw new System.NotImplementedException();
        }

        IBus bus;
    }
    #endregion
}
