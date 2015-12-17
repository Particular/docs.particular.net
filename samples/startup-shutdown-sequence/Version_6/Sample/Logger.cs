using System;
using System.IO;
using System.Threading;

#region Logger
static class Logger
{
    static string outputFilePath = Path.GetFullPath(@"..\..\..\StartupShutdownSequence.txt");
    static object locker = new object();
    static Logger()
    {
        AppDomain.CurrentDomain.ProcessExit += Exit;
        File.Delete(outputFilePath);
        File.AppendAllText(outputFilePath, "startcode StartupShutdownSequence\r\n");
    }

    static void Exit(object sender, EventArgs e)
    {
        File.AppendAllText(outputFilePath, "endcode");
    }

    public static void WriteLine(string message)
    {
        message = $"Thread:{Thread.CurrentThread.ManagedThreadId} {message}\r\n";
        Console.Write(message);
        lock (locker)
        {
            File.AppendAllText(outputFilePath, message);
        }
    }

}
#endregion