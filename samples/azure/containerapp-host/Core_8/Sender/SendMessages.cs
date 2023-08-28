using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sender;

public class SendMessages : IHostedService
{
    private readonly IMessageSession messageSession;
    private readonly Settings configuration;

    public SendMessages(IMessageSession messageSession, Settings configuration)
    {
        this.messageSession = messageSession;
        this.configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        async Task SendMessages()
        {
            var sendTasks = new List<Task>(configuration.MessageCount);
            for (int i = 0; i < configuration.MessageCount; i++)
            {
                sendTasks.Add(messageSession.Send(new CalculateFibonacci { Input = Random.Shared.Next(configuration.MinimumNumber, configuration.MaximumNumber)}, cancellationToken));
            }

            await Task.WhenAll(sendTasks);
        }

        _ = SendMessages();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}