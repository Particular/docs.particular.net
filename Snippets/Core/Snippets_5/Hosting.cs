namespace Core5
{
    using System;
    using NServiceBus;

    class Hosting
    {
        void SendOnly()
        {
            #region Hosting-SendOnly

            BusConfiguration busConfiguration = new BusConfiguration();
            ISendOnlyBus bus = Bus.CreateSendOnly(busConfiguration);

            #endregion
        }

        void Startup()
        {
            #region Hosting-Startup
            BusConfiguration busConfiguration = new BusConfiguration();
            //Apply configuration
            IStartableBus startableBus = Bus.Create(busConfiguration);
            IBus bus = startableBus.Start();
            #endregion
        }

        void Shutdown(IBus bus)
        {
            #region Hosting-Shutdown
            bus.Dispose();
            #endregion
        }

        #region Hosting-Static
        public static class EndpointInstance
        {
            public static IBus Endpoint { get; private set; }
            public static void SetInstance(IBus endpoint)
            {
                if (Endpoint != null)
                {
                    throw new Exception("Endpoint already set.");
                }
                Endpoint = endpoint;
            }
        }
        #endregion

    }
}