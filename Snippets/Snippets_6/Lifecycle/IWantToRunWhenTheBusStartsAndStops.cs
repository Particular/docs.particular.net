namespace Snippets6.Lifecycle
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region lifecycle-iwanttorunwhenthebusstartsandstops

    class RunWhenTheBusStartsAndStops : IWantToRunWhenBusStartsAndStops
    {
        public Task StartAsync()
        {
            // perform startup logic
            return Task.FromResult(0);
        }

        public Task StopAsync()
        {
            // perform shutdown logic
            return Task.FromResult(0);
        }
    }

    #endregion
}
