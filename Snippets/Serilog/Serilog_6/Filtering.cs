using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

public class Filtering
{
    public Filtering()
    {
        #region SerilogFiltering

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                path:"log.txt",
                restrictedToMinimumLevel: LogEventLevel.Debug
            )
            .Filter.ByIncludingOnly(
                inclusionPredicate: Matching.FromSource("MyNamespace"))
            .CreateLogger();

        LogManager.Use<SerilogFactory>();

        #endregion
    }
}