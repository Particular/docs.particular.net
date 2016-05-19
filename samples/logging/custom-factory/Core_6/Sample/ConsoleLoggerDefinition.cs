#region definition

using NServiceBus.Logging;

class ConsoleLoggerDefinition : LoggingFactoryDefinition
{

    protected override ILoggerFactory GetLoggingFactory()
    {
        return new ConsoleLoggerFactory();
    }
}
#endregion