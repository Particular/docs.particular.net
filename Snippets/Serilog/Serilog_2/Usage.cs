using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;

class Usage
{
    Usage()
    {
        #region SerilogInCode

        Log.Logger = new LoggerConfiguration()
            .WriteTo.ColoredConsole()
            .CreateLogger();

        LogManager.Use<SerilogFactory>();

        #endregion
    }
}
