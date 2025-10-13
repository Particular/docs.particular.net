using System.Reflection;

namespace AsyncAPI.Feature;

public class PublishedEventsConvention : IMessageConvention
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

    public string Name { get; } = "AsyncAPI Sample Event Message Convention";
}