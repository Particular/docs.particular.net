using System;
using NServiceBus;

namespace Distributor
{
    class Program
    {
        static void Main()
        {
            #region DistributorCode

            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.Scaleout.Distributor");
            busConfiguration.RunMSMQDistributor(withWorker: false);
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }

            #endregion
        }
    }
}