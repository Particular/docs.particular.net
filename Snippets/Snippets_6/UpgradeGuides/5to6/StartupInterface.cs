namespace Snippets6.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region 5to6-IWantToRunWhenBusStartsAndStops
    public class Bootstrapper : IWantToRunWhenBusStartsAndStops
    {
        public Task Start(IBusSession session)
        {
            // Do your startup action here. 
            // Either mark your Start method as async or do the following
            return Task.FromResult(0);
        }

        public Task Stop(IBusSession session)
        {
            // Do your cleanup action here. 
            // Either mark your Stop method as async or do the following
            return Task.FromResult(0);
        }
    }
    #endregion    
}
