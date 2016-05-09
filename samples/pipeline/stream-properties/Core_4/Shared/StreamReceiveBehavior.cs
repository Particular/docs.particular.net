using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
#pragma warning disable 618

#region ReceiveBehaviorDefinition
class StreamReceiveBehavior : IBehavior<ReceiveLogicalMessageContext>
{
    string location;

    public StreamReceiveBehavior(StreamStorageSettings storageSettings)
    {
        location = Path.GetFullPath(storageSettings.Location);
    }
    public void Invoke(ReceiveLogicalMessageContext context, Action next)
    {
#endregion
        #region write-stream-properties-back
        object message = context.LogicalMessage.Instance;
        List<FileStream> streamsToCleanUp = new List<FileStream>();
        foreach (PropertyInfo property in StreamStorageHelper.GetStreamProperties(message))
        {
            string headerKey = StreamStorageHelper.GetHeaderKey(message, property);
            string dataBusKey;
            //only attempt to process properties that have an associated header
            string key = "NServiceBus.PropertyStream." + headerKey;
            if (!context.LogicalMessage.Headers.TryGetValue(key, out dataBusKey))
            {
                continue;
            }

            string filePath = Path.Combine(location, dataBusKey);

            // If the file doesnt exist then something has gone wrong with the file share.
            // Perhaps he file has been manually deleted.
            // For safety send the message to the error queue
            if (!File.Exists(filePath))
            {
                string format = string.Format("Expected a file to exist in '{0}'. It is possible the file has been prematurely cleaned up.", filePath);
                throw new Exception(format);
            }
            FileStream fileStream = File.OpenRead(filePath);
            property.SetValue(message,fileStream);
            streamsToCleanUp.Add(fileStream);
        }
        #endregion

        #region cleanup-after-nested-action
        next();
        // Clean up all the temporary streams after handler processing
        // via the "next()" delegate has occurred
        foreach (FileStream fileStream in streamsToCleanUp)
        {
            fileStream.Dispose();
        }
        #endregion
    }

}