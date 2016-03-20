namespace Snippets3.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configuration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            configuration.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}