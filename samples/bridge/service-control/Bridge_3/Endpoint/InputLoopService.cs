using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;


public sealed class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        Console.WriteLine("Press enter to exit");
        Console.WriteLine("Press 'o' to send a message");
        Console.WriteLine("Press 'f' to toggle simulating of message processing failure");

        while (true)
        {

            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            var lowerInvariant = char.ToLowerInvariant(key.KeyChar);
            if (lowerInvariant == 'o')
            {
                var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                var message = new MyMessage(id);
                await messageSession.Send(message, stoppingToken);
            }
            if (lowerInvariant == 'f')
            {
                FailureSimulator.Enabled = !FailureSimulator.Enabled;
                Console.WriteLine("Failure simulation is now turned " + (FailureSimulator.Enabled ? "on" : "off"));
            }
        }
    }
}
