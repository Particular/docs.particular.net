using NServiceBus;
using NServiceBus.Serilog.Tracing;
using Serilog;

class Usage
{
    Usage()
    {
        #region SerilogTracingLogger

        var tracingLog = new LoggerConfiguration()
            .WriteTo.ColoredConsole()
            .MinimumLevel.Information()
            .CreateLogger();

        #endregion

        #region SerilogTracingPassLoggerToFeature

        var busConfiguration = new EndpointConfiguration("EndpointName");
        busConfiguration.EnableFeature<TracingLog>();
        busConfiguration.SerilogTracingTarget(tracingLog);

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