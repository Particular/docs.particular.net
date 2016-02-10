namespace Snippets6.Lifecycle
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region lifecycle-IWantToRunWhenBusStartsAndStops

    class RunWhenTheBusStartsAndStops : IWantToRunWhenBusStartsAndStops
    {
        public async Task Start(IMessageSession session)
        {
            // perform startup logic
        }

        public async Task Stop(IMessageSession session)
        {
            // perform shutdown logic
        }
    }

    #endregion
}
