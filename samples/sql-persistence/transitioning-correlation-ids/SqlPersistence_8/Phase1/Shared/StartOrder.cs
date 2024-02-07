using NServiceBus;

#region messagePhase1
public class StartOrder :
    IMessage
{
    public int OrderNumber { get; set; }
}
#endregion