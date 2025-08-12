namespace AsyncAPI.Feature;

public record TypeCache
{
    public string EndpointName { get; init; }
    public Dictionary<Type, Type> PublishedEventCache { get; init; }
    public Dictionary<string, (Type SubscribedType, Type ActualType)> SubscribedEventCache { get; init; }
}