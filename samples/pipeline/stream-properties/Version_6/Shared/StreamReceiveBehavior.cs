using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region ReceiveBehaviorDefinition

class StreamReceiveBehavior : Behavior<IIncomingLogicalMessageContext>
{
    string location;

    public StreamReceiveBehavior(StreamStorageSettings storageSettings)
    {
        location = Path.GetFullPath(storageSettings.Location);
    }

    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        #endregion

        #region write-stream-properties-back

        object message = context.Message.Instance;
        List<FileStream> streamsToCleanUp = new List<FileStream>();
        foreach (PropertyInfo property in StreamStorageHelper
            .GetStreamProperties(message))
        {
            string headerKey = StreamStorageHelper.GetHeaderKey(message, property);
            string dataBusKey;
            //only attempt to process properties that have an associated header
            string key = "NServiceBus.PropertyStream." + headerKey;
            if (!context.Headers.TryGetValue(key, out dataBusKey))
            {
                continue;
            }

            string filePath = Path.Combine(location, dataBusKey);

            // If the file doesnt exist then something has gone wrong with the file share. 
            // Perhaps he file has been manually deleted.
            // For safety send the message to the error queue
            if (!File.Exists(filePath))
            {
                string format = $"Expected a file to exist in '{filePath}'. It is possible the file has been prematurely cleaned up.";
                throw new Exception(format);
            }
            FileStream fileStream = File.OpenRead(filePath);
            property.SetValue(message, fileStream);
            streamsToCleanUp.Add(fileStream);
        }

        #endregion

        #region cleanup-after-nested-action

        await next();
        // Clean up all the temporary streams after handler processing
        // via the "next()" delegate has occurred
        foreach (FileStream fileStream in streamsToCleanUp)
        {
            fileStream.Dispose();
        }

        #endregion
    }
}
