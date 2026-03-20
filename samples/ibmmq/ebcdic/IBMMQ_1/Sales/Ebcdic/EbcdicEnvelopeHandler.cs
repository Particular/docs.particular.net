using System.Buffers;
using System.Buffers.Binary;
using System.Text;
using System.Text.Json;
using Contracts;
using NServiceBus.Extensibility;

#region EbcdicEnvelopeHandler
sealed class EbcdicEnvelopeHandler : IEnvelopeHandler
{
    const int CODESET_EBCDIC_INT = 500;
    static readonly Encoding Ebcdic = Encoding.GetEncoding(CODESET_EBCDIC_INT);
    const int RecordLength = 70;

    public Dictionary<string, string>? UnwrapEnvelope(
        string nativeMessageId,
        IDictionary<string, string> incomingHeaders,
        ReadOnlySpan<byte> incomingBody,
        ContextBag extensions,
        IBufferWriter<byte> bodyWriter)
    {
        if (incomingHeaders.ContainsKey("NServiceBus.EnclosedMessageTypes"))
            return null;

        if (incomingBody.Length != RecordLength)
            return null;

        // Parse fixed-length EBCDIC record
        var orderId = Ebcdic.GetString(incomingBody[..36]);
        var product = Ebcdic.GetString(incomingBody[36..66]).TrimEnd();
        var quantity = BinaryPrimitives.ReadInt32BigEndian(incomingBody[66..70]);

        // Write JSON body
        var writer = new Utf8JsonWriter(bodyWriter);
        writer.WriteStartObject();
        writer.WriteString("OrderId", orderId);
        writer.WriteString("Product", product);
        writer.WriteNumber("Quantity", quantity);
        writer.WriteEndObject();
        writer.Flush();

        var messageType = typeof(PlaceOrder);

        //MQMessage m;
        return new Dictionary<string, string>
        {
            ["NServiceBus.MessageId"] = nativeMessageId,
            ["NServiceBus.ConversationId"] = nativeMessageId,
            ["NServiceBus.EnclosedMessageTypes"] = messageType.FullName + ", " + messageType.Assembly.GetName().Name,
            ["NServiceBus.ContentType"] = "application/json",
            ["NServiceBus.MessageIntent"] = "Send",
            ["NServiceBus.TimeSent"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:ffffff") + " Z",
            ["NServiceBus.ReplyToAddress"] = "Acme.LegacySender",
            ["NServiceBus.OriginatingEndpoint"] = "LegacyMainframe",
            ["NServiceBus.OriginatingMachine"] = "MAINFRAME",
        };
    }
}
#endregion
