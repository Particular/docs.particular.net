namespace Shared
{
    using System;
    using System.IO;
    using System.Threading;

    public static class Logger
    {
        static string outputFilePath = Path.GetFullPath(@"..\..\..\..\..\..\MultiHostedEndpointsOutput.txt");
        static object locker = new object();
        static Logger()
        {
            AppDomain.CurrentDomain.ProcessExit += Exit;
            File.Delete(outputFilePath);
            File.AppendAllText(outputFilePath, "startcode MultiHostedEndpointsOutput\r\n");
        }

        static void Exit(object sender, EventArgs e)
        {
            File.AppendAllText(outputFilePath, "endcode");
        }

        public static void WriteLine(string message)
        {
            message = string.Format("Thread:{0} {1}\r\n", Thread.CurrentThread.ManagedThreadId, message);
            lock (locker)
            {
                File.AppendAllText(outputFilePath, message);
            }
        }

    }
}