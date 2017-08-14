namespace Core5.Headers.Writers
{
    using NServiceBus;

    public static class TransportMessageExtension
    {
        public static bool IsMessageOfTye<T>(this TransportMessage transportMessage)
        {
            if (transportMessage.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out var value))
            {
                return value == typeof(T).AssemblyQualifiedName;
            }
            return false;
        }
    }
}