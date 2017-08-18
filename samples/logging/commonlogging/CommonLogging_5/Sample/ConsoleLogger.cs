using System;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

public class ConsoleLogger :
    AbstractSimpleLogger
{
    public ConsoleLogger(string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName, string dateTimeFormat) : 
        base(logName, logLevel, showlevel, showDateTime, showLogName, dateTimeFormat)
    {
    }

    protected override void WriteInternal(LogLevel level, object message, Exception exception)
    {
        var stringBuilder = new StringBuilder();
        FormatOutput(stringBuilder, level, message, exception);
        Console.Out.WriteLine(stringBuilder.ToString());
    }
}