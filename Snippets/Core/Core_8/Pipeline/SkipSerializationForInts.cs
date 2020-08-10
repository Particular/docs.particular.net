namespace Core8.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region SkipSerialization

    class SkipSerializationForInts :
        Behavior<IOutgoingLogicalMessageContext>
    {
        public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            var outgoingLogicalMessage = context.Message;
            if (outgoingLogicalMessage.MessageType == typeof(int))
            {
                var headers = context.Headers;
                headers["MyCustomHeader"] = outgoingLogicalMessage.Instance.ToString();
                context.SkipSerialization();
            }
            return next();
        }

        public class Registration :
            RegisterStep
        {
            public Registration()
                : base(
                    stepId: "SkipSerializationForInts",
                    behavior: typeof(SkipSerializationForInts),
                    description: "Skips serialization for integers")
            {
            }
        }
    }

    #endregion
}