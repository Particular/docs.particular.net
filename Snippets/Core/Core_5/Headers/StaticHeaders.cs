namespace Core5.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        StaticHeaders(BusConfiguration busConfiguration)
        {
            #region header-static-endpoint
            IStartableBus startableBus = Bus.Create(busConfiguration);
            startableBus.OutgoingHeaders.Add("AllOutgoing", "ValueAllOutgoing");
            using (IBus bus = startableBus.Start())
            {
                #endregion
            }
        }
    }
}