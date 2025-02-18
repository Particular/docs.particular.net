namespace Core9.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            #region DefineMessageWithLargePayloadUsingConvention

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(
                property => property.Name.EndsWith("DataBus"));

            #endregion
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}