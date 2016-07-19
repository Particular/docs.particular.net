using System.IO;
using System.Threading.Tasks;
using NServiceBus.Transports;

#region QueueCreation

class FileTransportQueueCreator :
    ICreateQueues
{
    public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
    {
        foreach (var address in queueBindings.SendingAddresses)
        {
            CreateQueueFolder(address);
        }

        foreach (var address in queueBindings.ReceivingAddresses)
        {
            CreateQueueFolder(address);
        }

        return TaskEx.CompletedTask;
    }

    static void CreateQueueFolder(string address)
    {
        var fullPath = BaseDirectoryBuilder.BuildBasePath(address);
        var commitedPath = Path.Combine(fullPath, ".committed");
        Directory.CreateDirectory(commitedPath);
        var bodiesPath = Path.Combine(fullPath, ".bodies");
        Directory.CreateDirectory(bodiesPath);
    }

}

#endregion