using System.Reflection;
using NServiceBus;

namespace Infrastructure;

class PublishedEventsConvention : IMessageConvention
{
    public bool IsMessageType(Type type)
    {
        return false;
    }

    public bool IsCommandType(Type type)
    {
        return false;
    }

    public bool IsEventType(Type type)
    {
        return type.GetCustomAttribute<PublishedEvent>() != null;
    }

    public string Name { get; } = "TODO";
}