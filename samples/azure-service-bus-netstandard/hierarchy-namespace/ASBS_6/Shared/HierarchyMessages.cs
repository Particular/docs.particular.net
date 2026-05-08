using NServiceBus;

public class HierarchyCommand : ICommand
{
    public string Source { get; set; }
    public string Destination { get; set; }
}

public class HierarchyEvent : IEvent
{
    public string Source { get; set; }
}