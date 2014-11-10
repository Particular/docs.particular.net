#region ServiceMatrix.OnlineSales.OrderProcessing.EndpointConfig.after
using NServiceBus;
using NServiceBus.Persistence;
 
namespace OnlineSales.OrderProcessing
{
    public partial class EndpointConfig    
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<RavenDBPersistence>();
        }
    }
}
#endregion