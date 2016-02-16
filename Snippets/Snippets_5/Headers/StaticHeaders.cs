namespace Snippets5.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
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