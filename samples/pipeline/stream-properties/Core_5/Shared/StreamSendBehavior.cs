using System;
using System.IO;
using System.Reflection;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast.Messages;

#region SendBehaviorDefinition
class StreamSendBehavior : IBehavior<OutgoingContext>
{
    TimeSpan MaxMessageTimeToLive = TimeSpan.FromDays(14);
    string location;

    public StreamSendBehavior(StreamStorageSettings storageSettings)
    {
        location = Path.GetFullPath(storageSettings.Location);
    }
#endregion
    public void Invoke(OutgoingContext context, Action next)
    {
        #region copy-stream-properties-to-disk
        LogicalMessage logicalMessage = context.OutgoingLogicalMessage;
        TimeSpan timeToBeReceived = logicalMessage.Metadata.TimeToBeReceived;

        object message = logicalMessage.Instance;

        foreach (PropertyInfo property in StreamStorageHelper.GetStreamProperties(message))
        {
            Stream sourceStream = (Stream)property.GetValue(message, null);

            //Ignore null stream properties
            if (sourceStream == null)
            {
                continue;
            }
            string fileKey = GenerateKey(timeToBeReceived);

            string filePath = Path.Combine(location, fileKey);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (FileStream target = File.OpenWrite(filePath))
            {
                sourceStream.CopyTo(target);
            }

            //Reset the property to null so no other serializer attempts to use the property
            property.SetValue(message, null);

            //Dispose of the stream
            sourceStream.Dispose();

            //Store the header so on the receiving endpoint the file name is known
            string headerKey = StreamStorageHelper.GetHeaderKey(message, property);
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

        DateTime keepMessageUntil = DateTime.MaxValue;

        if (timeToBeReceived < TimeSpan.MaxValue)
        {
            keepMessageUntil = DateTime.Now + timeToBeReceived;
        }

        return Path.Combine(keepMessageUntil.ToString("yyyy-MM-dd_HH"), Guid.NewGuid().ToString());
    }
    #endregion

}