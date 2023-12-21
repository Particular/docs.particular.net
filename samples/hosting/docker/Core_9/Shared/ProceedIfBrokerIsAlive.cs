namespace Shared;

using System.Net.Sockets;

public class ProceedIfBrokerIsAlive
{
    public static async Task WaitForBroker(string host, CancellationToken cancellationToken = default)
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
