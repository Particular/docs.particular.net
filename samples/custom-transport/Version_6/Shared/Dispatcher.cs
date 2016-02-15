using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Transports;

#region Dispatcher
class Dispatcher : IDispatchMessages
{
    public Task Dispatch(TransportOperations outgoingMessages, ContextBag context)
    {
        foreach (UnicastTransportOperation transportOperation in outgoingMessages.UnicastTransportOperations)
        {
            string basePath = BaseDirectoryBuilder.BuildBasePath(transportOperation.Destination);
            string nativeMessageId = Guid.NewGuid().ToString();
            string bodyPath = Path.Combine(basePath, ".bodies", nativeMessageId) + ".xml"; 

            File.WriteAllBytes(bodyPath, transportOperation.Message.Body);

            List<string> messageContents = new List<string>
            {
                bodyPath,
                HeaderSerializer.Serialize(transportOperation.Message.Headers)
            };

            DirectoryBasedTransaction transaction;

            string messagePath = Path.Combine(basePath, nativeMessageId) + ".txt";

            if (transportOperation.RequiredDispatchConsistency != DispatchConsistency.Isolated &&
                context.TryGet(out transaction))
            {
                transaction.Enlist(messagePath, messageContents);
            }
            else
            {
                string tempFile = Path.GetTempFileName();

                //write to temp file first so we can do a atomic move 
                //this avoids the file being locked when the receiver tries to process it
                File.WriteAllLines(tempFile, messageContents);
                File.Move(tempFile, messagePath);
            }
        }

        return TaskEx.CompletedTask;
    }
}
#endregion