using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Distributor
{
    class Program
    {
        static void Main(string[] args)
        {
            #region DistributorCode

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.Scaleout.Distributor");
            busConfiguration.RunMSMQDistributor(withWorker: false);
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();
            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }

            #endregion
        }
    }
}