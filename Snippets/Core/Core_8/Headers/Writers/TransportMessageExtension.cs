namespace Core8.Headers.Writers
{
    using NServiceBus.MessageMutator;

    public static class TransportMessageExtension
    {
        public static bool IsMessageOfTye<T>(this MutateIncomingTransportMessageContext context)
        {
            if (context.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out var value))
            {
                return value == typeof(T).AssemblyQualifiedName;
            }
            return false;
        }
    }
}