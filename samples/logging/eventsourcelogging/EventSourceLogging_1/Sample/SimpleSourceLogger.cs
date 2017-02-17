using System.Diagnostics.Tracing;
using NServiceBus.EventSourceLogging;

#region EventSourceLoggerBase
[EventSource(
    Name = "Samples.Logging.SimpleSourceLogger",
    LocalizationResources = "Sample.NServiceBusEventSourceResources")]
public sealed class SimpleSourceLogger :
    EventSourceLoggerBase
{
    [Event(1, Level = EventLevel.Verbose)]
    public override void Debug(string logger, string message)
    {
        base.Debug(logger, message);
    }

    [Event(2, Level = EventLevel.Verbose)]
    public override void DebugException(string logger, string message, string exceptionType, string exceptionMessage, string exceptionValue)
    {
        base.DebugException(logger, message, exceptionType, exceptionMessage, exceptionValue);
    }
    #endregion

    [Event(3, Level = EventLevel.Informational)]
    public override void Info(string logger, string message)
    {
        base.Info(logger, message);
    }

    [Event(4, Level = EventLevel.Informational)]
    public override void InfoException(string logger, string message, string exceptionType, string exceptionMessage, string exceptionValue)
    {
        base.InfoException(logger, message, exceptionType, exceptionMessage, exceptionValue);
    }

    [Event(5, Level = EventLevel.Warning)]
    public override void Warn(string logger, string message)
    {
        base.Warn(logger, message);
    }

    [Event(6, Level = EventLevel.Warning)]
    public override void WarnException(string logger, string message, string exceptionType, string exceptionMessage, string exceptionValue)
    {
        base.WarnException(logger, message, exceptionType, exceptionMessage, exceptionValue);
    }

    [Event(7, Level = EventLevel.Error)]
    public override void Error(string logger, string message)
    {
        base.Error(logger, message);
    }

    [Event(8, Level = EventLevel.Error)]
    public override void ErrorException( string logger, string message, string exceptionType, string exceptionMessage, string exceptionValue)
    {
        base.ErrorException(logger, message, exceptionType, exceptionMessage, exceptionValue);
    }

    [Event(9, Level = EventLevel.Critical)]
    public override void Fatal(string logger, string message)
    {
        base.Fatal(logger, message);
    }

    [Event(10, Level = EventLevel.Critical)]
    public override void FatalException(string logger, string message, string exceptionType, string exceptionMessage, string exceptionValue)
    {
        base.FatalException(logger, message, exceptionType, exceptionMessage, exceptionValue);
    }
}