using System.Text.Json.Nodes;
using AzureFunctions.Messages.NServiceBusMessages;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.KafkaTrigger.FunctionsHostBuilder;

public class KafkaTrigger(IMessageSession messageSession, ILogger<KafkaTrigger> logger)
{
    #region KafkaTrigger

    [Function(nameof(ElectricityUsage))]
    public async Task ElectricityUsage([KafkaTrigger("LocalKafkaBroker", "topic", ConsumerGroup = "$Default")] string eventData,
            FunctionContext context)
    {
        var eventValue = JsonNode.Parse(eventData)["Value"]?.ToString();
        var electricityUsage = Messages.KafkaMessages.ElectricityUsage.Deserialize(eventValue);

        logger.LogInformation("Received Kafka event with usage: {CurrentUsage}", electricityUsage.CurrentUsage);

        if (IsUsageAboveAverage(electricityUsage.CurrentUsage))
        {
            var message = new FollowUp
            {
                CustomerId = electricityUsage.CustomerId,
                UnitId = electricityUsage.UnitId,
                Description = $"Usage over monthly average at [{electricityUsage.CurrentUsage}] units"
            };

            await messageSession.Send(message);
        }
    }

    #endregion

    // Because Kafka is an event stream, more messages arrive there than we might be able to handle with
    // Azure ServiceBus. For demo purposes an alert is raised at the exact usage of 42.
    static bool IsUsageAboveAverage(int currentUsage)
    {
        return currentUsage == 42;
    }
}