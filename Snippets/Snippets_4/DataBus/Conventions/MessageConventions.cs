namespace Snippets4.DataBus.Conventions
{
    using NServiceBus;

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