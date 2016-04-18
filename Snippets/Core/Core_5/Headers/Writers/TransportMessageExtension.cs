namespace Core5.Headers.Writers
{
    using NServiceBus;

    public static class TransportMessageExtension
    {
        public static bool IsMessageOfTye<T>(this TransportMessage transportMessage)
        {
            string value;
            if (transportMessage.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out value))
            {
                return value == typeof(T).AssemblyQualifiedName;
            }
            return false;
        }
    }
}