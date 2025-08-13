namespace AsyncAPI.Feature;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PublishedEvent : Attribute
{
    public string EventName { get; init; }
    public int Version { get; init; }
}