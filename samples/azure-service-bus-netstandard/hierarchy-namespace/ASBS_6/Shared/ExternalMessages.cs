using NServiceBus;

namespace Shared;

public class ExternalCommand : ICommand
{
    public string Source { get; set; }
    public string Destination { get; set; }
}

public interface IExternalEvent : IEvent
{
    string Source { get; set; }
}

public class ExternalEvent : IExternalEvent
{
    public string Source { get; set; }
}

public class OtherExternalEvent : IExternalEvent
{
    public string Source { get; set; }
}