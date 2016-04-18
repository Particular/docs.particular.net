using System.IO;
using System.IO.Compression;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

#region TransportMessageCompressionMutator
public class TransportMessageCompressionMutator : IMutateTransportMessages
{
    static ILog logger = LogManager.GetLogger("TransportMessageCompressionMutator");

    public void MutateOutgoing(LogicalMessage message, TransportMessage transportMessage)
    {
        logger.InfoFormat("transportMessage.Body size before compression: {0}", transportMessage.Body.Length);

        MemoryStream mStream = new MemoryStream(transportMessage.Body);
        MemoryStream outStream = new MemoryStream();

        using (GZipStream tinyStream = new GZipStream(outStream, CompressionMode.Compress))
        {
            mStream.CopyTo(tinyStream);
        }
        // copy the compressed buffer only after the GZipStream is disposed, 
        // otherwise, not all the compressed message will be copied.
        transportMessage.Body = outStream.ToArray();
        transportMessage.Headers["IWasCompressed"] = "true";
        logger.InfoFormat("transportMessage.Body size after compression: {0}", transportMessage.Body.Length);
    }

    public void MutateIncoming(TransportMessage transportMessage)
    {
        if (!transportMessage.Headers.ContainsKey("IWasCompressed"))
        {
            return;
        }
        using (GZipStream bigStream = new GZipStream(new MemoryStream(transportMessage.Body), CompressionMode.Decompress))
        {
            MemoryStream bigStreamOut = new MemoryStream();
            bigStream.CopyTo(bigStreamOut);
            transportMessage.Body = bigStreamOut.ToArray();
        }
    }
}
#endregion