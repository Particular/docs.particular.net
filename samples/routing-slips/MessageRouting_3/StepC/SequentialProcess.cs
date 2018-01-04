using NServiceBus;

public class SequentialProcess :
    ICommand
{
    public string StepCInfo { get; set; }
}