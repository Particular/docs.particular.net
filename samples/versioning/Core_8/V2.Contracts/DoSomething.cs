using NServiceBus;

public class DoSomething : ICommand
{
    public int SomeData { get; set; }
}