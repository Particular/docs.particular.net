namespace Snippets3.DataBus.Conventions
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            Configure configuration = null;
            #region DefineMessageWithLargePayloadUsingConvention

            configuration.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}