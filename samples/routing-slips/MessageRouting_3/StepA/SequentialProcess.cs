using NServiceBus;

#region single-message
public class SequentialProcess :
    ICommand
{
    public string StepAInfo { get; set; }
}
#endregion