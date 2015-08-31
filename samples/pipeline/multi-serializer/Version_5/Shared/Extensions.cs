using System;
using System.Linq;
using NServiceBus;

static class Extensions
{
    public static bool IsControlMessage(this TransportMessage transportMessage)
    {
        return transportMessage.Headers != null &&
               transportMessage.Headers.ContainsKey(Headers.ControlMessageHeader);
    }
    public static bool ContainsAttribute<T>(this Type type)
    {
        return type.GetCustomAttributes(typeof(T), true).Any();
    }
}