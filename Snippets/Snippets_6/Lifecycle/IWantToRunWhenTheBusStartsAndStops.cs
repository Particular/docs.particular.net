namespace Snippets6.Lifecycle
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region lifecycle-IWantToRunWhenBusStartsAndStops

    class RunWhenTheBusStartsAndStops : IWantToRunWhenBusStartsAndStops
    {
        public async Task StartAsync()
        {
            // perform startup logic
        }

        public async Task StopAsync()
        {
            // perform shutdown logic
        }
    }

    #endregion
}
