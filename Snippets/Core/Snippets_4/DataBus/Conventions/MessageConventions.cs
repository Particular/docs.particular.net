namespace Snippets4.DataBus.Conventions
{
    using NServiceBus;

    class MessageConventions
    {
        MessageConventions(Configure configuration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            configuration.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}