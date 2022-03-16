using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NServiceBus.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusSubscriber
{
    using System.Globalization;

    #region Behavior
    public class MassTransitIngestBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            if (!context.MessageHeaders.TryGetValue("Content-Type", out var contentType) || contentType != "application/vnd.masstransit+json")
            {
                return next();
            }

            var envelope = DeserializeMassTransitPayload(context);

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
                var host = envelope["host"] as JObject;
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
                var mtHeaders = envelope["headers"] as JObject;
                foreach (var header in mtHeaders.Children<JProperty>())
                {
                    context.Message.Headers[header.Name] = header.Value.Value<string>();
                }
            }

            if (envelope.ContainsKey("messageType"))
            {
                var types = (envelope["messageType"] as JArray)
                    .Select(token => GetTypeName(token))
                    .ToArray();

                context.Message.Headers[NServiceBus.Headers.EnclosedMessageTypes] = string.Join(";", types);
            }

            if (envelope.ContainsKey("message"))
            {
                var body = envelope["message"] as JObject;
                UpdateMessagePayload(context, body);
            }

            return next();
        }

        static JObject DeserializeMassTransitPayload(IIncomingPhysicalMessageContext context)
        {
            using (var memoryStream = new MemoryStream(context.Message.Body.ToArray()))
            using (var streamReader = new StreamReader(memoryStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var envelope = serializer.Deserialize(jsonReader, typeof(JObject)) as JObject;
                return envelope;
            }
        }

        static void AddHeaderIfExists(IIncomingPhysicalMessageContext context, JObject massTransitContainer, string incomingHeaderName, string nservicebusHeaderName, Func<string, string> transform = null)
        {
            if (massTransitContainer.ContainsKey(incomingHeaderName))
            {
                var value = massTransitContainer[incomingHeaderName].Value<string>();
                if (transform != null)
                {
                    value = transform(value);
                }
                context.Message.Headers[nservicebusHeaderName] = value;
            }
        }

        static void UpdateMessagePayload(IIncomingPhysicalMessageContext context, JObject body)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamReader = new StreamWriter(memoryStream))
            using (var jsonWriter = new JsonTextWriter(streamReader))
            {
                serializer.Serialize(jsonWriter, body);
                jsonWriter.Flush();
                context.UpdateMessage(memoryStream.ToArray());
            }
        }

        static string GetTypeName(JToken typeToken)
        {
            var typeString = typeToken.Value<string>();
            if (typeString.StartsWith(urnTypePrefix))
            {
                typeString = typeString.Substring(urnTypePrefix.Length);
            }
            typeString = typeString.Replace(":", ".");
            return typeString;
        }

        static readonly JsonSerializer serializer = new JsonSerializer();
        const string urnTypePrefix = "urn:message:";
    }
    #endregion
}
