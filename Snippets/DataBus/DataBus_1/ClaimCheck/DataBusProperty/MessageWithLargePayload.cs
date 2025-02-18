using NServiceBus;
using NServiceBus.ClaimCheck;

namespace ClaimCheck_1.ClaimCheck.DataBusProperty
{
    #region MessageWithLargePayload

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public ClaimCheckProperty<byte[]> LargeBlob { get; set; }
    }

    #endregion
}