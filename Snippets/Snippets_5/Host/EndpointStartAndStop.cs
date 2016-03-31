namespace Snippets5.Host
{
    using NServiceBus;

    #region host-EndpointStartAndStop

    class RunWhenTheEndpointStartsAndStops : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            // perform startup logic
        }

        public void Stop()
        {
            // perform shutdown logic
        }
    }

    #endregion
}
