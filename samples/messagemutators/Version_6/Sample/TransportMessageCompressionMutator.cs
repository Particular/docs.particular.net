using System.IO;
using System.IO.Compression;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region TransportMessageCompressionMutator
public class TransportMessageCompressionMutator : IMutateIncomingTransportMessages, IMutateOutgoingTransportMessages
{
    static ILog log = LogManager.GetLogger("TransportMessageCompressionMutator");

    public void MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        log.Info("transportMessage.Body size before compression: " + context.OutgoingBody.Length);

        MemoryStream mStream = new MemoryStream(context.OutgoingBody);
        MemoryStream outStream = new MemoryStream();

        using (GZipStream tinyStream = new GZipStream(outStream, CompressionMode.Compress))
        {
            mStream.CopyTo(tinyStream);
        }
        // copy the compressed buffer only after the GZipStream is disposed, 
        // otherwise, not all the compressed message will be copied.
        context.OutgoingBody = outStream.ToArray();
        context.OutgoingHeaders["IWasCompressed"]= "true";
        log.Info("transportMessage.Body size after compression: " + context.OutgoingBody.Length);
    }

    public void MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        if (!context.Headers.ContainsKey("IWasCompressed"))
        {
            return;
        }
        using (GZipStream bigStream = new GZipStream(new MemoryStream(context.Body), CompressionMode.Decompress))
        {
            MemoryStream bigStreamOut = new MemoryStream();
            bigStream.CopyTo(bigStreamOut);
            context.Body = bigStreamOut.ToArray();
        }
    }
}
#endregion