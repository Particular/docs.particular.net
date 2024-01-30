using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared
{
    public static class ProceedIfBrokerIsAlive
    {
        public static async Task WaitForBroker(string host, CancellationToken cancellationToken = default)
        {
            do
            {
                try
                {
                    using var tcpClient = new TcpClient();
                    await tcpClient.ConnectAsync(host, 5672);

                    return;
                }
                catch (Exception)
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
            while (!cancellationToken.IsCancellationRequested);
        }
    }
}
