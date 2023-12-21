using Microsoft.Extensions.Hosting;

using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared
{
    public class ProceedIfRabbitMqIsAlive
    {
        public static async Task WaitForRabbitMq(string host, CancellationToken cancellationToken = default)
        {
            do
            {
                try
                {
                    using var tcpClientB = new TcpClient();

                    await tcpClientB.ConnectAsync(host, 5672);

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
