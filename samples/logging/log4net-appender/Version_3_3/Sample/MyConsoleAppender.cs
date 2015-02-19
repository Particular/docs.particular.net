using System;
using log4net.Appender;
using log4net.Core;

#region Appender
class MyConsoleAppender : AppenderSkeleton
{
    protected override void Append(LoggingEvent loggingEvent)
    {
        Console.ForegroundColor = Color;
        loggingEvent.WriteRenderedMessage(Console.Out);
        Console.WriteLine();
        Console.ResetColor();
    }

    public ConsoleColor Color { get; set; }
}
#endregion