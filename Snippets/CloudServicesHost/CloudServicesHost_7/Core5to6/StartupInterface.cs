using System.Threading.Tasks;
using NServiceBus;

namespace CloudServicesHost_7.Core5to6
{
    #region 5to6-EndpointStartAndStopCloudHost
    public class Bootstrapper :
        IWantToRunWhenEndpointStartsAndStops
    {
        public Task Start(IMessageSession session)
        {
            // Do startup actions here.
            // Either mark Start method as async or do the following
            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            // Do cleanup actions here.
            // Either mark Stop method as async or do the following
            return Task.CompletedTask;
        }
    }
    #endregion
}