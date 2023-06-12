namespace Core8.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(property =>
            {
                return property.Name.EndsWith("DataBus");
            });

            #endregion

        }
    }
}