namespace Core6.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            var conventionsBuilder = endpointConfiguration.Conventions();
            conventionsBuilder.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}
