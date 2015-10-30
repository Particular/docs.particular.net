using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region TransportMessageCompressionMutator
public class TransportMessageCompressionMutator : IMutateIncomingTransportMessages, IMutateOutgoingTransportMessages
{
    static ILog logger = LogManager.GetLogger("TransportMessageCompressionMutator");

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        logger.InfoFormat("transportMessage.Body size before compression: {0}", context.OutgoingBody.Length);

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
        logger.InfoFormat("transportMessage.Body size after compression: {0}", context.OutgoingBody.Length);
        return Task.FromResult(0);
    }

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        if (!context.Headers.ContainsKey("IWasCompressed"))
        {
            return Task.FromResult(0);
        }
        using (GZipStream bigStream = new GZipStream(new MemoryStream(context.Body), CompressionMode.Decompress))
        {
            MemoryStream bigStreamOut = new MemoryStream();
            bigStream.CopyTo(bigStreamOut);
            context.Body = bigStreamOut.ToArray();
        }
        return Task.FromResult(0);
    }
}
#endregion