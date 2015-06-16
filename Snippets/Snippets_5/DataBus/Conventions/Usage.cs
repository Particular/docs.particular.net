namespace Snippets5.DataBus.Conventions
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = null;
            #region DefineMessageWithLargePayloadUsingConvention

            busConfiguration.Conventions()
                .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}
