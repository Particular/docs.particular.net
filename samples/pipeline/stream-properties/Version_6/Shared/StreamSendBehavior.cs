using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus.DeliveryConstraints;
using NServiceBus.Performance.TimeToBeReceived;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.OutgoingPipeline;

#region SendBehaviorDefinition
class StreamSendBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    TimeSpan MaxMessageTimeToLive = TimeSpan.FromDays(14);
    string location;

    public StreamSendBehavior(StreamStorageSettings storageSettings)
    {
        location = Path.GetFullPath(storageSettings.Location);
    }
    public override async Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
#endregion
        #region copy-stream-properties-to-disk
        var timeToBeReceived = TimeSpan.MaxValue;
        DiscardIfNotReceivedBefore constraint;

        if (context.TryGetDeliveryConstraint(out constraint))
        {
            timeToBeReceived = constraint.MaxTime;
        }

        object message = context.Message.Instance;

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
            context.Headers["NServiceBus.PropertyStream." + headerKey] = fileKey;
        }

        await next();
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