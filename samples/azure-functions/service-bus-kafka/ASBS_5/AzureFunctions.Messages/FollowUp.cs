using NServiceBus;

namespace AzureFunctions.Messages.NServiceBusMessages;

public class FollowUp : IMessage
{
    public int CustomerId { get; set; }
    public int UnitId { get; set; }
    public string Description { get; set; }
}