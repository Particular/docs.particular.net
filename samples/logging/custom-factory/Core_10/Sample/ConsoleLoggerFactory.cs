using System;
using NServiceBus.Logging;
#region factory

class ConsoleLoggerFactory(LogLevel level) :
    ILoggerFactory
{
    public ILog GetLogger(Type type)
    {
        return GetLogger(type.FullName);
    }

    public ILog GetLogger(string name)
    {
        return new ConsoleLog(name, level);
    }
}

#endregion