#region definition
using NServiceBus.Logging;

class ConsoleLoggerDefinition :
    LoggingFactoryDefinition
{
    LogLevel level = LogLevel.Info;

    public void Level(LogLevel level)
    {
        this.level = level;
    }

    protected override ILoggerFactory GetLoggingFactory() => new ConsoleLoggerFactory(level);
}
#endregion