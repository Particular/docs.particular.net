using System;
using System.IO;
using System.Threading;
using NServiceBus.Logging;

#region Logger
static class Logger
{
    static ILog log = LogManager.GetLogger(typeof(Logger));
    public static string OutputFilePath = Path.GetFullPath(@"..\..\..\StartupShutdownSequence.txt");
    static object locker = new object();

    static Logger()
    {
        AppDomain.CurrentDomain.ProcessExit += Exit;
        File.Delete(OutputFilePath);
        File.AppendAllText(OutputFilePath, "startcode StartupShutdownSequence\r\n");
    }

    static void Exit(object sender, EventArgs e)
    {
        File.AppendAllText(OutputFilePath, "endcode");
    }

    public static void WriteLine(string message)
    {
        message = $"Thread:{Thread.CurrentThread.ManagedThreadId} {message}\r\n";
        log.Info(message);
        lock (locker)
        {
            File.AppendAllText(OutputFilePath, message);
        }
    }

}
#endregion