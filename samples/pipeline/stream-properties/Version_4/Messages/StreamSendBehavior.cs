using System;
using System.IO;
using System.Reflection;
using NServiceBus.DataBus;
using NServiceBus.Gateway.HeaderManagement;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
#pragma warning disable 618

public class StreamSendBehavior : IBehavior<SendLogicalMessageContext>
{
    public IDataBus DataBus { get; set; }
    TimeSpan MaxMessageTimeToLive = TimeSpan.FromDays(14);
    
    string location;

    public StreamSendBehavior(StreamStorageLocation storageLocation)
    {
        location = storageLocation.Location;
    }
    public void Invoke(SendLogicalMessageContext context, Action next)
    {
        TimeSpan timeToBeReceived = context.MessageToSend.Metadata.TimeToBeReceived;

        object message = context.MessageToSend.Instance;

        foreach (PropertyInfo property in StreamDatabusHelpers.GetStreamProperties(message))
        {
            Stream sourceStream = (Stream) property.GetValue(message, null);

            if (sourceStream == null)
                continue;

            string fileKey = GenerateKey(timeToBeReceived);

            string filePath = Path.Combine(location, fileKey);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (FileStream target = File.OpenWrite(filePath))
            {
                sourceStream.CopyTo(target);
            }

            property.SetValue(message, null);
            sourceStream.Dispose();
            string headerKey = StreamDatabusHelpers.GetHeaderKey(message, property);
            context.MessageToSend.Headers[HeaderMapper.DATABUS_PREFIX + headerKey] = fileKey;
        }

        next();
    }


    string GenerateKey(TimeSpan timeToBeReceived)
    {
        if (timeToBeReceived > MaxMessageTimeToLive)
            timeToBeReceived = MaxMessageTimeToLive;

        DateTime keepMessageUntil = DateTime.MaxValue;

        if (timeToBeReceived < TimeSpan.MaxValue)
            keepMessageUntil = DateTime.Now + timeToBeReceived;

        return Path.Combine(keepMessageUntil.ToString("yyyy-MM-dd_HH"), Guid.NewGuid().ToString());
    }


}