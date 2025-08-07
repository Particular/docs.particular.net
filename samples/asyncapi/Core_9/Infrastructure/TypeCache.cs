namespace Infrastructure;

public record TypeCache
{
    public Dictionary<Type, Type> PublishedEventCache { get; init; }

    public Dictionary<string, (Type SubscribedType, Type ActualType)> SubscribedEventCache { get; init; }
}