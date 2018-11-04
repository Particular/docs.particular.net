using NServiceBus;
using Serilog;

class TracingUsage
{
    TracingUsage()
    {
        #region SerilogTracingLogger

        var tracingLog = new LoggerConfiguration()
            .WriteTo.File("log.txt")
            .MinimumLevel.Information()
            .CreateLogger();

        #endregion

        var endpointConfiguration = new EndpointConfiguration("EndpointName");

        #region SerilogTracingPassLoggerToFeature

        var serilogTracing = endpointConfiguration.EnableSerilogTracing(tracingLog);
        serilogTracing.EnableMessageTracing();

        #endregion
    }

    void EnableSagaTracing(EndpointConfiguration endpointConfiguration, ILogger logger)
    {
        #region EnableSagaTracing

        var serilogTracing = endpointConfiguration.EnableSerilogTracing(logger);
        serilogTracing.EnableSagaTracing();

        #endregion
    }

    void EnableMessageTracing(EndpointConfiguration endpointConfiguration, ILogger logger)
    {
        #region EnableMessageTracing

        var serilogTracing = endpointConfiguration.EnableSerilogTracing(logger);
        serilogTracing.EnableMessageTracing();

        #endregion
    }

    void Seq()
    {
        #region SerilogTracingSeq

        var tracingLog = new LoggerConfiguration()
            .WriteTo.Seq("http://localhost:5341")
            .MinimumLevel.Information()
            .CreateLogger();

        #endregion
    }
}

public class TheMessage
{
}