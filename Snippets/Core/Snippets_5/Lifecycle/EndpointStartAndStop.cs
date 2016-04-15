namespace Core5.Lifecycle
{
    using NServiceBus;

    #region lifecycle-EndpointStartAndStop

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
