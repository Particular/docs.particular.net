namespace Core3.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configuration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            configuration.DefiningDataBusPropertiesAs(property =>
            {
                return property.Name.EndsWith("DataBus");
            });

            #endregion

        }
    }
}