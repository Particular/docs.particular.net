using NServiceBus;

public class MyMessage : ICommand
{
    public int Number { get; set; }
}