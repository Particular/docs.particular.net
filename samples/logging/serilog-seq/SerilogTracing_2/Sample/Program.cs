using System;
using NServiceBus;
using NServiceBus.Serilog.Tracing;
using Serilog;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.SerilogSeq";
        #region ConfigureSerilog
        var tracingLog = new LoggerConfiguration()
            .WriteTo.Seq("http://localhost:5341")
            .MinimumLevel.Information()
            .CreateLogger();
        #endregion

        #region UseConfig

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.SerilogSeq");
        busConfiguration.EnableFeature<TracingLog>();
        busConfiguration.SerilogTracingTarget(tracingLog);

        #endregion

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var createUser = new CreateUser
            {
                UserName = "jsmith",
                FamilyName = "Smith",
                GivenNames = "John",
            };
            bus.SendLocal(createUser);
            Console.WriteLine("Message sent");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
