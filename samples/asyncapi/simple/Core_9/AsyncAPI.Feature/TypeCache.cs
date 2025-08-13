namespace AsyncAPI.Feature;

public record TypeCache
{
    public string EndpointName { get; init; }
    public List<Type> Events { get; init; }
}