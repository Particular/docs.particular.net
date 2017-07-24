using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Transport;

#region Dispatcher
class Dispatcher :
    IDispatchMessages
{
    public Task Dispatch(TransportOperations outgoingMessages, TransportTransaction transaction, ContextBag context)
    {
        foreach (var operation in outgoingMessages.UnicastTransportOperations)
        {
            var basePath = BaseDirectoryBuilder.BuildBasePath(operation.Destination);
            var nativeMessageId = Guid.NewGuid().ToString();
            var bodyPath = Path.Combine(basePath, ".bodies", $"{nativeMessageId}.xml");

            var dir = Path.GetDirectoryName(bodyPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllBytes(bodyPath, operation.Message.Body);

            var messageContents = new List<string>
            {
                bodyPath,
                HeaderSerializer.Serialize(operation.Message.Headers)
            };

            var messagePath = Path.Combine(basePath, $"{nativeMessageId}.txt");

            // write to temp file first so an atomic move can be done
            // this avoids the file being locked when the receiver tries to process it
            var tempFile = Path.GetTempFileName();
            File.WriteAllLines(tempFile, messageContents);
            File.Move(tempFile, messagePath);
        }

        return Task.FromResult(0);
    }
}
#endregion