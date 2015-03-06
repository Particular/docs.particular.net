using System;
using System.IO;
using System.Reflection;
using NServiceBus.DataBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast.Messages;

#pragma warning disable 618

public class StreamSendBehavior : IBehavior<SendLogicalMessageContext>
{
    public IDataBus DataBus { get; set; }
    TimeSpan MaxMessageTimeToLive = TimeSpan.FromDays(14);
    
    string location;

    public StreamSendBehavior(StreamStorageSettings storageSettings)
    {
        location = Path.GetFullPath(storageSettings.Location);
    }
    public void Invoke(SendLogicalMessageContext context, Action next)
    {
        #region copy-stream-properties-to-disk
        LogicalMessage logicalMessage = context.MessageToSend;
        TimeSpan timeToBeReceived = logicalMessage.Metadata.TimeToBeReceived;

        object message = logicalMessage.Instance;

        foreach (PropertyInfo property in StreamStorageHelpers.GetStreamProperties(message))
        {
            Stream sourceStream = (Stream) property.GetValue(message, null);

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

            property.SetValue(message, null);
            sourceStream.Dispose();
            string headerKey = StreamStorageHelpers.GetHeaderKey(message, property);
            logicalMessage.Headers["NServiceBus.PropertyStream." + headerKey] = fileKey;
        }

        next();
        #endregion
    }
    #region generata-key-for-stream
    string GenerateKey(TimeSpan timeToBeReceived)
    {
        if (timeToBeReceived > MaxMessageTimeToLive)
            timeToBeReceived = MaxMessageTimeToLive;

        DateTime keepMessageUntil = DateTime.MaxValue;

        if (timeToBeReceived < TimeSpan.MaxValue)
            keepMessageUntil = DateTime.Now + timeToBeReceived;

        return Path.Combine(keepMessageUntil.ToString("yyyy-MM-dd_HH"), Guid.NewGuid().ToString());
    }
    #endregion


}