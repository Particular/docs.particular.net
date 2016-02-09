using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using Shared;

namespace SqlRelay
{
    class Program
    {
        static void Main()
        {
            #region sqlrelay-config
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("SqlRelay");
            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True");
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.DisableFeature<AutoSubscribe>();
            busConfiguration.EnableInstallers();
            #endregion

            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
