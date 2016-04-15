namespace Snippets5.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            ConventionsBuilder conventions = busConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}
