using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

namespace Sender
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region Sending

            Console.WriteLine("Sending messages...");
            for (var i = 0; i < 100; i++)
            {
                var searchGitHub = new SearchGitHub
                {
                    Repository = "NServiceBus",
                    Owner = "Particular",
                    Branch = "master"
                };
                await messageSession.Send("Samples.Throttling.Limited", searchGitHub);
            }
            #endregion
            Console.WriteLine("Messages sent.");

        }
    }
}
