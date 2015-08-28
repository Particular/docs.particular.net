namespace Snippets4.Lifecycle
{
    using NServiceBus;

    #region lifecycle-iwanttorunwhenthebusstartsandstops

    class RunWhenTheBusStartsAndStops : IWantToRunWhenBusStartsAndStops
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
