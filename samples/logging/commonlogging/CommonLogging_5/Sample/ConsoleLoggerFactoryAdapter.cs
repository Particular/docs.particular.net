using Common.Logging;
using Common.Logging.Simple;

public class ConsoleLoggerFactoryAdapter :
    AbstractSimpleLoggerFactoryAdapter
{
    public ConsoleLoggerFactoryAdapter() : base(null)
    {
    }

    protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
    {
        return new ConsoleLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
    }
}