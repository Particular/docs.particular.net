namespace Shared;

using NServiceBus;

public class MyCommand : ICommand
{
    public string Data { get; set; }
}