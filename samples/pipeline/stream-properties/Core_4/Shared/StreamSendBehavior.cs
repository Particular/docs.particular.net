using System;
using System.IO;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#pragma warning disable 618

#region SendBehaviorDefinition
class StreamSendBehavior : IBehavior<SendLogicalMessageContext>
{
    TimeSpan MaxMessageTimeToLive = TimeSpan.FromDays(14);
    string location;

    public StreamSendBehavior(StreamStorageSettings storageSettings)
    {
        location = Path.GetFullPath(storageSettings.Location);
    }

    public void Invoke(SendLogicalMessageContext context, Action next)
    {
#endregion
        #region copy-stream-properties-to-disk
        var logicalMessage = context.MessageToSend;
        var timeToBeReceived = logicalMessage.Metadata.TimeToBeReceived;

        var message = logicalMessage.Instance;

        foreach (var property in StreamStorageHelper.GetStreamProperties(message))
        {
            var sourceStream = (Stream)property.GetValue(message, null);

            //Ignore null stream properties
            if (sourceStream == null)
            {
                continue;
            }
            var fileKey = GenerateKey(timeToBeReceived);

            var filePath = Path.Combine(location, fileKey);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var target = File.OpenWrite(filePath))
            {
                sourceStream.CopyTo(target);
            }

            //Reset the property to null so no other serializer attempts to use the property
            property.SetValue(message, null);

            //Dispose of the stream
            sourceStream.Dispose();

            //Store the header so on the receiving endpoint the file name is known
            var headerKey = StreamStorageHelper.GetHeaderKey(message, property);
            logicalMessage.Headers["NServiceBus.PropertyStream." + headerKey] = fileKey;
        }

        next();
        #endregion
    }
    #region generate-key-for-stream
    string GenerateKey(TimeSpan timeToBeReceived)
    {
        if (timeToBeReceived > MaxMessageTimeToLive)
        {
            timeToBeReceived = MaxMessageTimeToLive;
        }

        var keepMessageUntil = DateTime.MaxValue;

        if (timeToBeReceived < TimeSpan.MaxValue)
        {
            keepMessageUntil = DateTime.Now + timeToBeReceived;
        }

        return Path.Combine(keepMessageUntil.ToString("yyyy-MM-dd_HH"), Guid.NewGuid().ToString());
    }
    #endregion


}