using NServiceBus;

public class Ping : ICommand
{
    public string Message { get; set; }
}