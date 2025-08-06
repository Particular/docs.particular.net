using System;
using NServiceBus.Logging;
#region factory

class ConsoleLoggerFactory(LogLevel level) :
    ILoggerFactory
{
    public ILog GetLogger(Type type) => GetLogger(type.FullName);
    public ILog GetLogger(string name)  => new ConsoleLog(name, level);
}

#endregion