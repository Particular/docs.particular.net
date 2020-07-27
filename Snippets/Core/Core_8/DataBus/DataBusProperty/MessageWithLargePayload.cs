namespace Core8.DataBus.DataBusProperty
{
    using NServiceBus;

    #region MessageWithLargePayload

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public DataBusProperty<byte[]> LargeBlob { get; set; }
    }

    #endregion
}
