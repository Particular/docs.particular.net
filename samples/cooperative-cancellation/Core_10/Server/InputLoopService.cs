using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Server
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await messageSession.SendLocal(new LongRunningMessage { DataId = Guid.NewGuid() }, CancellationToken.None);
        }
    }
}