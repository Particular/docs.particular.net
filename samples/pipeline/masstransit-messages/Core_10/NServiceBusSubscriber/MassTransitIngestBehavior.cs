using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace NServiceBusSubscriber
{
    #region Behavior
    public class MassTransitIngestBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            var envelope = DeserializeMassTransitPayload(context);

            if (!envelope.ContainsKey("messageType"))
            {
                return next();
            }
            
            var types = (envelope["messageType"] as JsonArray)
                .Select(GetTypeName)
                .ToArray();

            context.Message.Headers[NServiceBus.Headers.EnclosedMessageTypes] = string.Join(";", types);
            
            AddHeaderIfExists(context, envelope, "messageId", NServiceBus.Headers.MessageId);
            AddHeaderIfExists(context, envelope, "conversationId", NServiceBus.Headers.ConversationId);
            AddHeaderIfExists(context, envelope, "correlationId", NServiceBus.Headers.CorrelationId);
            AddHeaderIfExists(context, envelope, "sourceAddress", "MassTransit.SourceAddress");
            AddHeaderIfExists(context, envelope, "destinationAddress", "MassTransit.DestinationAddress");
            AddHeaderIfExists(context, envelope, "sentTime", NServiceBus.Headers.TimeSent, sentTime =>
            {
                var parsed = DateTime.Parse(sentTime, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces);
                return NServiceBus.DateTimeOffsetHelper.ToWireFormattedString(parsed);
            });

            if (envelope.ContainsKey("host"))
            {
                var host = envelope["host"] as JsonObject;
                AddHeaderIfExists(context, host, "machineName", NServiceBus.Headers.OriginatingMachine);
                AddHeaderIfExists(context, host, "processName", "MassTransit.Host.ProcessName");
                AddHeaderIfExists(context, host, "processId", "MassTransit.Host.ProcessId");
                AddHeaderIfExists(context, host, "assembly", "MassTransit.Host.Assembly");
                AddHeaderIfExists(context, host, "assemblyVersion", "MassTransit.Host.AssemblyVersion");
                AddHeaderIfExists(context, host, "frameworkVersion", "MassTransit.Host.FrameworkVersion");
                AddHeaderIfExists(context, host, "massTransitVersion", "MassTransit.Host.MassTransitVersion");
                AddHeaderIfExists(context, host, "operatingSystem", "MassTransit.Host.OperatingSystem");
            }

            if (envelope.ContainsKey("headers"))
            {
                var mtHeaders = envelope["headers"] as JsonObject;
                foreach (var header in mtHeaders)
                {
                    context.Message.Headers[header.Key] = header.Value.GetValue<string>();
                }
            }

            if (envelope.ContainsKey("message"))
            {
                var body = envelope["message"] as JsonObject;
                UpdateMessagePayload(context, body);
            }

            return next();
        }

        static JsonObject DeserializeMassTransitPayload(IIncomingPhysicalMessageContext context)
        {
            return JsonNode.Parse(context.Message.Body.ToArray()).AsObject();
        }

        static void AddHeaderIfExists(IIncomingPhysicalMessageContext context, JsonObject massTransitContainer, string incomingHeaderName, string nservicebusHeaderName, Func<string, string> transform = null)
        {
            if (massTransitContainer.ContainsKey(incomingHeaderName) && massTransitContainer[incomingHeaderName] != null)
            {
                massTransitContainer[incomingHeaderName].AsValue().TryGetValue<string>(out var value);
                if (value != null && transform != null)
                {
                    value = transform(value);
                }
                context.Message.Headers[nservicebusHeaderName] = value;
            }
        }

        static void UpdateMessagePayload(IIncomingPhysicalMessageContext context, JsonObject body)
        {
            context.UpdateMessage(Encoding.UTF8.GetBytes(body.ToJsonString()));
        }

        static string GetTypeName(JsonNode typeToken)
        {
            var typeString = typeToken.GetValue<string>();
            if (typeString.StartsWith(urnTypePrefix))
            {
                typeString = typeString.Substring(urnTypePrefix.Length);
            }
            typeString = typeString.Replace(":", ".");
            return typeString;
        }

        const string urnTypePrefix = "urn:message:";
    }
    #endregion
}
