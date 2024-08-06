namespace Core9.DataBus.DataBusProperty
{
    using NServiceBus;

#pragma warning disable CS0618 // Type or member is obsolete
    #region MessageWithLargePayload

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public DataBusProperty<byte[]> LargeBlob { get; set; }
    }

    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
}