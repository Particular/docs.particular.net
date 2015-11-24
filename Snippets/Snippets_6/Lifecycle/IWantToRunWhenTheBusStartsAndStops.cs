namespace Snippets6.Lifecycle
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region lifecycle-IWantToRunWhenBusStartsAndStops

    class RunWhenTheBusStartsAndStops : IWantToRunWhenBusStartsAndStops
    {
        public async Task Start(IBusContext context)
        {
            // perform startup logic
        }

        public async Task Stop(IBusContext context)
        {
            // perform shutdown logic
        }
    }

    #endregion
}
