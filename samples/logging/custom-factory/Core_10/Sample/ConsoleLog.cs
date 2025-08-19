using System;
using NServiceBus.Logging;

#region log
class ConsoleLog(string name, LogLevel level) :
    ILog
{
    public bool IsDebugEnabled { get; } = LogLevel.Debug >= level;
    public bool IsInfoEnabled { get; } = LogLevel.Info >= level;
    public bool IsWarnEnabled { get; } = LogLevel.Warn >= level;
    public bool IsErrorEnabled { get; } = LogLevel.Error >= level;
    public bool IsFatalEnabled { get; } = LogLevel.Fatal >= level;

    void Write(string level, string message, Exception exception)
    {
        Console.WriteLine($"{name}. {level}. {message}. Exception: {exception}");
    }

    void Write(string level, string message)
    {
        Console.WriteLine($"{name}. {level}. {message}.");
    }

    void Write(string level, string format, params object[] args)
    {
        format = $"{name}. {level}. {format}";
        Console.WriteLine(format, args);
    }

    public void Debug(string message)
    {
        if (IsDebugEnabled)
        {
            Write("Debug", message);
        }
    }

    public void Debug(string message, Exception exception)
    {
        if (IsDebugEnabled)
        {
            Write("Debug", message, exception);
        }
    }

    public void DebugFormat(string format, params object[] args)
    {
        if (IsDebugEnabled)
        {
            Write("Debug", format, args);
        }
    }
    // Other log methods excluded for brevity
    #endregion
    public void Info(string message)
    {
        if (IsInfoEnabled)
        {
            Write("Info", message);
        }
    }

    public void Info(string message, Exception exception)
    {
        if (IsInfoEnabled)
        {
            Write("Info", message, exception);
        }
    }

    public void InfoFormat(string format, params object[] args)
    {
        if (IsInfoEnabled)
        {
            Write("Info", format, args);
        }
    }

    public void Warn(string message)
    {
        if (IsWarnEnabled)
        {
            Write("Warn", message);
        }
    }

    public void Warn(string message, Exception exception)
    {
        if (IsWarnEnabled)
        {
            Write("Warn", message, exception);
        }
    }

    public void WarnFormat(string format, params object[] args)
    {
        if (IsWarnEnabled)
        {
            Write("Warn", format, args);
        }
    }

    public void Error(string message)
    {
        if (IsErrorEnabled)
        {
            Write("Error", message);
        }
    }

    public void Error(string message, Exception exception)
    {
        if (IsErrorEnabled)
        {
            Write("Error", message, exception);
        }
    }

    public void ErrorFormat(string format, params object[] args)
    {
        if (IsErrorEnabled)
        {
            Write("Error", format, args);
        }
    }

    public void Fatal(string message)
    {
        if (IsFatalEnabled)
        {
            Write("Fatal", message);
        }
    }

    public void Fatal(string message, Exception exception)
    {
        if (IsFatalEnabled)
        {
            Write("Fatal", message, exception);
        }
    }

    public void FatalFormat(string format, params object[] args)
    {
        if (IsFatalEnabled)
        {
            Write("Fatal", format, args);
        }
    }

}