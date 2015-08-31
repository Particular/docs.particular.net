using System.IO;
using System.IO.Compression;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region TransportMessageCompressionMutator
public class TransportMessageCompressionMutator : IMutateIncomingTransportMessages, IMutateOutgoingTransportMessages
{
    static ILog log = LogManager.GetLogger("TransportMessageCompressionMutator");

 
    public void MutateOutgoing(MutateOutgoingTransportMessagesContext context)
    {
        

        log.Info("transportMessage.Body size before compression: " + context.Body.Length);

        MemoryStream mStream = new MemoryStream(context.Body);
        MemoryStream outStream = new MemoryStream();

        using (GZipStream tinyStream = new GZipStream(outStream, CompressionMode.Compress))
        {
            mStream.CopyTo(tinyStream);
        }
        // copy the compressed buffer only after the GZipStream is disposed, 
        // otherwise, not all the compressed message will be copied.
        context.Body = outStream.ToArray();
        context.SetHeader("IWasCompressed", "true");
        log.Info("transportMessage.Body size after compression: " + context.Body.Length);
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