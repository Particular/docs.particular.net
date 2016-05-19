using System;
using NServiceBus.Logging;

#region log
class ConsoleLog : ILog
{
    string name;

    public ConsoleLog(string name, LogLevel level)
    {
        this.name = name;
        IsDebugEnabled = LogLevel.Debug >= level;
        IsInfoEnabled = LogLevel.Info >= level;
        IsWarnEnabled = LogLevel.Warn >= level;
        IsErrorEnabled = LogLevel.Error >= level;
        IsFatalEnabled = LogLevel.Fatal >= level;
    }

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
        if (!IsDebugEnabled)
        {
            return;
        }
        Write("Debug", message);
    }

    public void Debug(string message, Exception exception)
    {
        if (!IsDebugEnabled)
        {
            return;
        }
        Write("Debug", message, exception);
    }


    public void DebugFormat(string format, params object[] args)
    {
        if (!IsDebugEnabled)
        {
            return;
        }
        Write("Debug", format, args);
    }

    public void Info(string message)
    {
        if (!IsInfoEnabled)
        {
            return;
        }
        Write("Info", message);
    }

    public void Info(string message, Exception exception)
    {
        if (!IsInfoEnabled)
        {
            return;
        }
        Write("Info", message, exception);
    }

    public void InfoFormat(string format, params object[] args)
    {
        if (!IsInfoEnabled)
        {
            return;
        }
        Write("Info", format, args);
    }

    public void Warn(string message)
    {
        if (!IsWarnEnabled)
        {
            return;
        }
        Write("Warn", message);
    }

    public void Warn(string message, Exception exception)
    {
        if (!IsWarnEnabled)
        {
            return;
        }
        Write("Warn", message, exception);
    }

    public void WarnFormat(string format, params object[] args)
    {
        if (!IsWarnEnabled)
        {
            return;
        }
        Write("Warn", format, args);
    }

    public void Error(string message)
    {
        if (!IsErrorEnabled)
        {
            return;
        }
        Write("Error", message);
    }

    public void Error(string message, Exception exception)
    {
        if (!IsErrorEnabled)
        {
            return;
        }
        Write("Error", message, exception);
    }

    public void ErrorFormat(string format, params object[] args)
    {
        if (!IsErrorEnabled)
        {
            return;
        }
        Write("Error", format, args);
    }

    public void Fatal(string message)
    {
        if (!IsFatalEnabled)
        {
            return;
        }
        Write("Fatal", message);
    }

    public void Fatal(string message, Exception exception)
    {
        if (!IsFatalEnabled)
        {
            return;
        }
        Write("Fatal", message, exception);
    }

    public void FatalFormat(string format, params object[] args)
    {
        if (!IsFatalEnabled)
        {
            return;
        }
        Write("Fatal", format, args);
    }

    public bool IsDebugEnabled { get; }
    public bool IsInfoEnabled { get; }
    public bool IsWarnEnabled { get; }
    public bool IsErrorEnabled { get; }
    public bool IsFatalEnabled { get; }
}
#endregion