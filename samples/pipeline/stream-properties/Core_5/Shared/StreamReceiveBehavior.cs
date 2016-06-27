using System;
using System.Collections.Generic;
using System.IO;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region ReceiveBehaviorDefinition
class StreamReceiveBehavior : IBehavior<IncomingContext>
{
    string location;

    public StreamReceiveBehavior(StreamStorageSettings storageSettings)
    {
        location = Path.GetFullPath(storageSettings.Location);
    }

    public void Invoke(IncomingContext context, Action next)
    {
#endregion
        #region write-stream-properties-back
        var message = context.IncomingLogicalMessage.Instance;
        var streamsToCleanUp = new List<FileStream>();
        foreach (var property in StreamStorageHelper
            .GetStreamProperties(message))
        {
            var headerKey = StreamStorageHelper.GetHeaderKey(message, property);
            string dataBusKey;
            //only attempt to process properties that have an associated header
            var key = $"NServiceBus.PropertyStream.{headerKey}";
            if (!context.IncomingLogicalMessage.Headers.TryGetValue(key, out dataBusKey))
            {
                continue;
            }

            var filePath = Path.Combine(location, dataBusKey);

            // If the file doesnt exist then something has gone wrong with the file share.
            // Perhaps he file has been manually deleted.
            // For safety send the message to the error queue
            if (!File.Exists(filePath))
            {
                var format = $"Expected a file to exist in '{filePath}'. It is possible the file has been prematurely cleaned up.";
                throw new Exception(format);
            }
            var fileStream = File.OpenRead(filePath);
            property.SetValue(message, fileStream);
            streamsToCleanUp.Add(fileStream);
        }
        #endregion

        #region cleanup-after-nested-action
        next();
        // Clean up all the temporary streams after handler processing
        // via the "next()" delegate has occurred
        foreach (var fileStream in streamsToCleanUp)
        {
            fileStream.Dispose();
        }
        #endregion
    }
}
