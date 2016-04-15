namespace Core6.Headers.Writers
{
    using NServiceBus.MessageMutator;

    public static class TransportMessageExtension
    {
        public static bool IsMessageOfTye<T>(this MutateIncomingTransportMessageContext context)
        {
            string value;
            if (context.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out value))
            {
                return value == typeof(T).AssemblyQualifiedName;
            }
            return false;
        }
    }
}