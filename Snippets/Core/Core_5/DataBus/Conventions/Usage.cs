namespace Core5.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            var conventions = busConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(p =>
            {
                return p.Name.EndsWith("DataBus");
            });

            #endregion

        }
    }
}