using System.Text;
using NServiceBus;
using NServiceBus.Support;

public static class QueueNameAwareTranslation
{
    public static void EnableQueueNameOverrideInAddressTranslation(this TransportExtensions<MsmqTransport> transportExtensions)
    {
        transportExtensions.OverrideAddressTranslation(logicalAddress =>
        {
            string machine;
            if (!logicalAddress.EndpointInstance.Properties.TryGetValue("machine", out machine))
            {
                machine = RuntimeEnvironment.MachineName;
            }
            string queueName;
            if (!logicalAddress.EndpointInstance.Properties.TryGetValue("queue", out queueName))
            {
                queueName = logicalAddress.EndpointInstance.Endpoint;
            }
            var queue = new StringBuilder(queueName);
            if (logicalAddress.EndpointInstance.Discriminator != null)
            {
                queue.Append("-" + logicalAddress.EndpointInstance.Discriminator);
            }
            if (logicalAddress.Qualifier != null)
            {
                queue.Append("." + logicalAddress.Qualifier);
            }
            return queue + "@" + machine;
        });
    }
}