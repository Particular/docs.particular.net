using System;
using NServiceBus;

class Program
{

	static void Main()
	{
		#region multi-hosting

		using( var bus1 = StartInstance1() )
		using( var bus2 = StartInstance2() )
		{
			Console.WriteLine( "\r\nBus instances created and configured; press any key to stop program\r\n" );
			Console.ReadKey();
		}
		#endregion
	}

	static IBus StartInstance1()
	{
		#region multi-hosting-assembly-scan

		BusConfiguration busConfig = new BusConfiguration();

		busConfig.EndpointName( "Samples.MultiHosting-1" );
		busConfig.AssembliesToScan( AllAssemblies.Matching( "instance-1-assembly." ) );
		busConfig.UseSerialization<JsonSerializer>();
		busConfig.EnableInstallers();
		busConfig.UsePersistence<InMemoryPersistence>();

		IBus bus = Bus.Create( busConfig ).Start();

		return bus;

		#endregion
	}

	static IBus StartInstance2()
	{
		BusConfiguration busConfig = new BusConfiguration();

		busConfig.EndpointName( "Samples.MultiHosting-2" );
		busConfig.AssembliesToScan( AllAssemblies.Matching( "instance-2-assembly." ) );
		busConfig.UseSerialization<XmlSerializer>();
		busConfig.EnableInstallers();
		busConfig.UsePersistence<InMemoryPersistence>();

		IBus bus = Bus.Create( busConfig ).Start();

		return bus;
	}
}
