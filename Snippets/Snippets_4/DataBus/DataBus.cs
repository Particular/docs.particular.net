using NServiceBus;

public class DataBus
{
    public void FileShareDataBus()
    {
        string databusPath = null;

        #region FileShareDataBus

        Configure configure = Configure.With()
            .FileShareDataBus(databusPath);

        #endregion
    }

    public void AzureDataBus()
    {
        #region AzureDataBus

        Configure.With()
            .AzureDataBus();

        #endregion
    }
}

namespace DataBusProperties
{
    #region MessageWithLargePayload

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public DataBusProperty<byte[]> LargeBlob { get; set; }
    }

    #endregion

    #region MessageWithLargePayloadUsingConvention

    public class MessageWithLargePayloadUsingConvention
    {
        public string SomeProperty { get; set; }
        public byte[] LargeBlobDataBus { get; set; }
    }

    #endregion

    public static class MessageConventions
    {
        public static void DefineDataBusPropertiesConvention(Configure configuration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            configuration.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }

}