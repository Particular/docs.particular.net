#region ServiceMatrix.OnlineSalesV5.eCommerce.Infrastructure.ConfigurePersistence
using NServiceBus;
using NServiceBus.Persistence;

namespace OnlineSalesV5.eCommerce.Infrastructure
{
    public class ConfigurePersistence : INeedInitialization
    {
        public void Customize(BusConfiguration config)
        {
            config.UsePersistence<RavenDBPersistence>();
        }
    }
}
#endregion