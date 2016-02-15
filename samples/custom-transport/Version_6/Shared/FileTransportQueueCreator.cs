using System.IO;
using System.Threading.Tasks;
using NServiceBus.Transports;

#region QueueCreation

class FileTransportQueueCreator : ICreateQueues
{
    public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
    {
        foreach (string address in queueBindings.SendingAddresses)
        {
            CreateQueueFolder(address);
        }

        foreach (string address in queueBindings.ReceivingAddresses)
        {
            CreateQueueFolder(address);
        }

        return TaskEx.CompletedTask;
    }

    static void CreateQueueFolder(string address)
    {
        string fullPath = BaseDirectoryBuilder.BuildBasePath(address);
        string commitedPath = Path.Combine(fullPath, ".committed");
        Directory.CreateDirectory(commitedPath);
        string bodiesPath = Path.Combine(fullPath, ".bodies");
        Directory.CreateDirectory(bodiesPath);
    }

}

#endregion