namespace Core5.Mutators.Instance
{
    using NServiceBus.MessageMutator;

    #region IMutateIncomingMessages
    public class MutateIncomingMessages :
        IMutateIncomingMessages
    {
        public object MutateIncoming(object message)
        {
            // return the same instance
            // or optionally create and return a new instance
            return message;
        }
    }
    #endregion
}
