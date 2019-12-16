using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        static SemaphoreSlim semaphore = new SemaphoreSlim(0);

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += CancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;

            var host = new Host();

            Console.Title = host.EndpointName;

            await host.Start();
            await Console.Out.WriteLineAsync("Press Ctrl+C to exit...");

            // wait until notified that the process should exit
            await semaphore.WaitAsync();

            await host.Stop();
        }

        static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            semaphore.Release();
        }

        static void ProcessExit(object sender, EventArgs e)
        {
            semaphore.Release();
        }
    }
}