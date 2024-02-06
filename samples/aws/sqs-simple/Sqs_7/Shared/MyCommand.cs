using NServiceBus;

public class MyCommand : ICommand
{
    public byte[] Data { get; set; }
}