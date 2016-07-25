namespace Core4.DataBus.Conventions
{
    using NServiceBus;

    class MessageConventions
    {
        MessageConventions(Configure configuration)
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