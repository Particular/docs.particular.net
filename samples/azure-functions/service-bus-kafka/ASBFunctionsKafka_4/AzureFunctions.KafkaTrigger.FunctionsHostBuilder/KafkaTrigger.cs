using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using NServiceBus;

namespace AzureFunctions.KafkaTrigger.FunctionsHostBuilder;

public class KafkaTrigger
{
    readonly IMessageSession messageSession;

    public KafkaTrigger(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    [Function("KafkaTrigger")]
    public Task Run([KafkaTrigger("LocalBroker", "input-topic", ConsumerGroup = "$Default")] KafkaEventData<string> kevent, ILogger log)
    {
        log.LogInformation("Received Kafka message: {KeventValue}", kevent.Value);

        var message = new FollowUp
        {
            Value = kevent.Value
        };

        return messageSession.Send(message);
    }
}