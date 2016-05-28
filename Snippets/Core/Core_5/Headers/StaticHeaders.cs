namespace Core5.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        StaticHeaders(BusConfiguration busConfiguration)
        {
            #region header-static-endpoint
            var startableBus = Bus.Create(busConfiguration);
            startableBus.OutgoingHeaders.Add("AllOutgoing", "ValueAllOutgoing");
            using (var bus = startableBus.Start())
            {
                #endregion
            }
        }
    }
}