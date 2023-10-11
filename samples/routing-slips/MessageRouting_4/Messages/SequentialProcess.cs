using NServiceBus;
#region multi-message
public class SequentialProcess :
    ICommand
{
    public string StepAInfo { get; set; }
    public string StepBInfo { get; set; }
    public string StepCInfo { get; set; }
}
#endregion