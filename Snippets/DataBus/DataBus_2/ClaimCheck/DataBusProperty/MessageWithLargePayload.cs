namespace ClaimCheck_2.ClaimCheck.DataBusProperty
{
    using NServiceBus;

    #region MessageWithLargePayload

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public ClaimCheckProperty<byte[]> LargeBlob { get; set; }
    }

    #endregion
}