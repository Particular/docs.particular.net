namespace Core5.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            var conventionsBuilder = busConfiguration.Conventions();
            conventionsBuilder.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}
