namespace Core3
{
    using System;
    using NServiceBus;
    using NServiceBus.Unicast;

    class Hosting
    {
        void Simple()
        {
            #region Hosting-SendOnly

            var configure = Configure.With();
            var configUnicastBus = configure.UnicastBus();
            var bus = configUnicastBus.SendOnly();

            #endregion
        }

        void Startup()
        {
            #region Hosting-Startup

            var configure = Configure.With();
            var configUnicastBus = configure.UnicastBus();
            var startableBus = configUnicastBus.CreateBus();
            var bus = startableBus.Start();
            #endregion
        }

        void Shutdown(IBus bus)
        {
            #region Hosting-Shutdown
            var busImpl = (UnicastBus) bus;
            busImpl.Dispose();
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