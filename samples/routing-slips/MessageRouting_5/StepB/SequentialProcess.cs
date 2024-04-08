using NServiceBus;

public class SequentialProcess :
    ICommand
{
    public string StepBInfo { get; set; }
}