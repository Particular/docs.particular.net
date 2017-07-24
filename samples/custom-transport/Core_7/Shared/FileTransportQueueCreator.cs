using System.IO;
using System.Threading.Tasks;
using NServiceBus.Transport;

#region QueueCreation

class FileTransportQueueCreator :
    ICreateQueues
{
    public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
    {
        foreach (var address in queueBindings.SendingAddresses)
        {
            CreateQueueDirectory(address);
        }

        foreach (var address in queueBindings.ReceivingAddresses)
        {
            CreateQueueDirectory(address);
        }

        return Task.FromResult(0);
    }

    static void CreateQueueDirectory(string address)
    {
        var fullPath = BaseDirectoryBuilder.BuildBasePath(address);
        var commitedPath = Path.Combine(fullPath, ".committed");
        Directory.CreateDirectory(commitedPath);
        var bodiesPath = Path.Combine(fullPath, ".bodies");
        Directory.CreateDirectory(bodiesPath);
    }

}

#endregion