#region definition

using System;
using NServiceBus.Logging;

class ConsoleLoggerDefinition : LoggingFactoryDefinition
{
    Lazy<LogLevel> level;

    public void Level(LogLevel level)
    {
        this.level = new Lazy<LogLevel>(() => level);
    }
    protected override ILoggerFactory GetLoggingFactory()
    {
        return new ConsoleLoggerFactory(level.Value);
    }
}
#endregion