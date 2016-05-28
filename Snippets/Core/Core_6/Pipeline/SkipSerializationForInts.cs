namespace Core6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region SkipSerialization
    class SkipSerializationForInts : Behavior<IOutgoingLogicalMessageContext>
    {
        public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            var outgoingLogicalMessage = context.Message;
            if (outgoingLogicalMessage.MessageType == typeof(int))
            {
                context.Headers["MyCustomHeader"] = outgoingLogicalMessage.Instance.ToString();
                context.SkipSerialization();
            }
            return next();
        }

        public class Registration : RegisterStep
        {
            public Registration()
                : base("SkipSerializationForInts", typeof(SkipSerializationForInts), "Skips serialization for integers")
            {
            }
        }
    }
    #endregion
}