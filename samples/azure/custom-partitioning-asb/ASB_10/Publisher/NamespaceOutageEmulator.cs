using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

class NamespaceOutageEmulator
{
    readonly string connectionString;

    public NamespaceOutageEmulator(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task Toggle()
    {
        var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

        var topic1 = await namespaceManager.GetTopicAsync("bundle-1")
            .ConfigureAwait(false);
        var newStatus = NewStatus(topic1.Status);
        topic1.Status = newStatus;
        await namespaceManager.UpdateTopicAsync(topic1)
            .ConfigureAwait(false);

        try
        {
            var topic2 = await namespaceManager.GetTopicAsync("bundle-2")
                .ConfigureAwait(false);
            topic2.Status = newStatus;
            await namespaceManager.UpdateTopicAsync(topic2)
                .ConfigureAwait(false);

        }
        catch (MessagingEntityNotFoundException)
        {
        }

        var parts = Console.Title.Split(new [] {" ("}, StringSplitOptions.RemoveEmptyEntries);
        Console.Title = parts[0] + (topic1.Status == EntityStatus.Active ? "" : " (OUTAGE: Namespace with connection string 'AzureServiceBus.ConnectionString.1' is not available)");
    }
    public static async Task EnsureNoOutageIsEmulated(string connectionString)
    {
        var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

        try
        {
            var topic1 = await namespaceManager.GetTopicAsync("bundle-1")
                .ConfigureAwait(false);

            if (topic1.Status != EntityStatus.Active)
            {
                topic1.Status = EntityStatus.Active;
                await namespaceManager.UpdateTopicAsync(topic1)
                    .ConfigureAwait(false);
            }
        }
        catch (MessagingEntityNotFoundException)
        {
        }

        try
        {
            var topic2 = await namespaceManager.GetTopicAsync("bundle-2")
                .ConfigureAwait(false);
        
            if (topic2.Status != EntityStatus.Active)
            {
                topic2.Status = EntityStatus.Active;
                await namespaceManager.UpdateTopicAsync(topic2)
                    .ConfigureAwait(false);
            }
        }
        catch (MessagingEntityNotFoundException)
        {
        }
    }

    static EntityStatus NewStatus(EntityStatus currentStatus)
    {
        return currentStatus == EntityStatus.Disabled
            ? EntityStatus.Active 
            : EntityStatus.Disabled;
    }
}