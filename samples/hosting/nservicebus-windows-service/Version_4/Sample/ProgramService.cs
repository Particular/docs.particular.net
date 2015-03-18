using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;
using System.ServiceProcess;

class ProgramService : ServiceBase
{
	IBus bus;

	#region windowsservice-hosting-main

	static void Main()
	{
		using( var service = new ProgramService() )
		{
			if( Environment.UserInteractive )
			{
				service.OnStart( null );

				Console.WriteLine( "\r\nBus created and configured; press any key to stop program\r\n" );
				Console.Read();

				service.OnStop();

				return;
			}
			Run( service );
		}
	}

	#endregion

	#region windowsservice-hosting-onstart

	protected override void OnStart( string[] args )
	{
		Configure.Serialization.Json();
		Configure.Features.Enable<Sagas>();

		Configure configure = Configure.With();
		configure.DefineEndpointName( "Samples.WindowsServiceAndConsole" );
		configure.Log4Net();
		configure.DefaultBuilder();
		configure.InMemorySagaPersister();
		configure.UseInMemoryTimeoutPersister();
		configure.InMemorySubscriptionStorage();
		configure.UseTransport<Msmq>();

		bus = configure.UnicastBus()
			.CreateBus()
			.Start( () => Configure.Instance.ForInstallationOn<Windows>().Install() );
	}

	#endregion

	#region windowsservice-hosting-onstop

	protected override void OnStop()
	{
		bus = null;
	}

	#endregion

}