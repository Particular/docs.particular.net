using System;
using NServiceBus.Logging;
#region factory

class ConsoleLoggerFactory : ILoggerFactory
{
    Lazy<LogLevel> level = new Lazy<LogLevel>(() => LogLevel.Info);

    public void Level(LogLevel level)
    {
        this.level = new Lazy<LogLevel>(() => level);
    }

    public ILog GetLogger(Type type)
    {
        return GetLogger(type.FullName);
    }

    public ILog GetLogger(string name)
    {
        return new ConsoleLog(name, level.Value);
    }
}

#endregion