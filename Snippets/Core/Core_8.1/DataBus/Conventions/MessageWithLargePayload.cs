
namespace Core8.DataBus.Conventions
{

    #region MessageWithLargePayloadUsingConvention

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public byte[] LargeBlobDataBus { get; set; }
    }

    #endregion
}
