using System.Buffers.Binary;
using System.Text;
using System.Text.Json;
using Contracts;
using NServiceBus.MessageMutator;

#region EbcdicMutator
sealed class FixedLengthEBCDICToJsonMutator : IMutateIncomingTransportMessages
{
    // EBCDIC International (Latin-1) code page
    static readonly Encoding Ebcdic = Encoding.GetEncoding("IBM500");
    const int RecordLength = 70;

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        if (context.Headers.ContainsKey("NServiceBus.EnclosedMessageTypes"))
            return Task.CompletedTask;

        var body = context.Body.Span;
        if (body.Length != RecordLength)
            return Task.CompletedTask;

        // Parse fixed-length EBCDIC record
        var orderId = Ebcdic.GetString(body[..36]);
        var product = Ebcdic.GetString(body[36..66]).TrimEnd();
        var quantity = BinaryPrimitives.ReadInt32BigEndian(body[66..70]);

        // Write JSON body
        using var ms = new MemoryStream();
        using (var writer = new Utf8JsonWriter(ms))
        {
            writer.WriteStartObject();
            writer.WriteString("OrderId", orderId);
            writer.WriteString("Product", product);
            writer.WriteNumber("Quantity", quantity);
            writer.WriteEndObject();
        }
        context.Body = new ReadOnlyMemory<byte>(ms.ToArray());

        var messageType = typeof(PlaceOrder);
        var messageId = context.Headers.TryGetValue("NServiceBus.MessageId", out var existingId)
            ? existingId
            : Guid.NewGuid().ToString();

        context.Headers["NServiceBus.MessageId"] = messageId;
        context.Headers["NServiceBus.ConversationId"] = messageId;
        context.Headers["NServiceBus.EnclosedMessageTypes"] = messageType.FullName!;
        context.Headers["NServiceBus.ContentType"] = "application/json";
        context.Headers["NServiceBus.MessageIntent"] = "Send";
        context.Headers["NServiceBus.TimeSent"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:ffffff") + " Z";
        context.Headers["NServiceBus.ReplyToAddress"] = "DEV.RECEIVER";
        context.Headers["NServiceBus.OriginatingEndpoint"] = "LegacyMainframe";
        context.Headers["NServiceBus.OriginatingMachine"] = "MAINFRAME";

        return Task.CompletedTask;
    }
}
#endregion
