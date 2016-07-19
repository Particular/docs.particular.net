namespace Core5.Mutators.Instance
{
    using NServiceBus.MessageMutator;

    #region IMutateOutgoingMessages
    public class MutateOutgoingMessages :
        IMutateOutgoingMessages
    {
        public object MutateOutgoing(object message)
        {
            // return the same instance
            // or optionally create and return a new instance
            return message;
        }
    }
    #endregion
}
