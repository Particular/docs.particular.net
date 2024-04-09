using NServiceBus;

namespace Shared;

public class MyCommand : ICommand
{
    public string Data { get; set; }
}