using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Threading.Tasks;

class Program
{

    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
		busConfig.EndpointName( "Samples.RavendbSequenceGenerator" );
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();

        #region RavenDBSetup

        IDocumentStore defaultStore = new DocumentStore()
                                      {
                                          Url = "http://localhost:32076",
										  DefaultDatabase = "Sample"
                                      }
            .Initialize();

        busConfig.UsePersistence<RavenDBPersistence>()
            .DoNotSetupDatabasePermissions() //Only required to simplify the sample setup
            .SetDefaultDocumentStore(defaultStore);

        #endregion

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();

			Parallel.For( 0, 10, index => 
			{
				bus.Send( new IssueNext
				{
					SequenceId = DateTime.Now.ToString("yyyy-MM-dd-hh-mm") //every minute we want a new sequence
				} );
			} );

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
