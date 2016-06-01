using NServiceBus;

public class DoSomething : ICommand
{
    public int SequenceId { get; set; }
}