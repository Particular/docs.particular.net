using NServiceBus;

namespace Shared
{
    public class PriceUpdateAcknowledged :
    IMessage
    {
        public string BranchOffice { get; set; }
    }
}