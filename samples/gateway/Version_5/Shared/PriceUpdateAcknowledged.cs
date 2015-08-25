using NServiceBus;

public class PriceUpdateAcknowledged : IMessage
{
    public string BranchOffice { get; set; }
}