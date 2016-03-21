namespace Snippets6.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            ConventionsBuilder conventions = endpointConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}
