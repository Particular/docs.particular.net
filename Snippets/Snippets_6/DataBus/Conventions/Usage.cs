namespace Snippets6.DataBus.Conventions
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = null;
            #region DefineMessageWithLargePayloadUsingConvention

            ConventionsBuilder conventions = busConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}
