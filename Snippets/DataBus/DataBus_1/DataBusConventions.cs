namespace DataBusConventions
{
    using NServiceBus.ClaimCheck.DataBus;

    #region MessageWithLargePayloadUsingConvention

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public byte[] LargeBlobDataBus { get; set; }
    }

    #endregion

    class Usage
    {
        Usage(NServiceBus.EndpointConfiguration endpointConfiguration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(
                property => property.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}