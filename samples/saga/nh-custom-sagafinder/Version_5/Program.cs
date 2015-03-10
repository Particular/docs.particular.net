using System;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{

	static void Main()
	{
		BusConfiguration busConfig = new BusConfiguration();
		busConfig.EndpointName( "Samples.NHibernateCustomSagaFinder" );
		busConfig.UseSerialization<JsonSerializer>();
		busConfig.EnableInstallers();

		#region NHibernateSetup

		busConfig.UsePersistence<NHibernatePersistence>()
			.ConnectionString( @"Data Source=.\SqlExpress;Initial Catalog=NHCustomSagaFinder;Integrated Security=True" );

		#endregion

		using( IStartableBus bus = Bus.Create( busConfig ) )
		{
			bus.Start();
			bus.SendLocal( new StartOrder
			{
				OrderId = "123"
			} );

			Console.WriteLine( "\r\nPress any key to stop program\r\n" );
			Console.ReadKey();
		}
	}
}
