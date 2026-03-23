---
title: Envelope Handlers
summary: Implement IEnvelopeHandler to unwrap incoming messages from custom envelope formats before they enter the NServiceBus pipeline.
component: Core
reviewed: 2026-03-20
related:
- nservicebus/cloudevents
- nservicebus/messaging/third-party-integration
- nservicebus/messaging/headers
---

When receiving messages from external systems or transports that wrap payloads in a custom envelope format, NServiceBus needs to extract the actual message body and headers before processing the message. The `IEnvelopeHandler` interface allows implementing this extraction step.

## How it works

When a message arrives from the transport, NServiceBus passes it through all registered envelope handlers in registration order. Each handler has the opportunity to inspect the message and attempt to unwrap it.

- If a handler returns a non-null headers dictionary, that handler's extracted headers and body are used as the incoming message for the pipeline. No further handlers are tried.
- If a handler returns `null` or throws an exception, the next handler is tried. Exceptions are logged as warnings; they do not fail the message.
- If no handler successfully unwraps the message, NServiceBus treats it as a standard NServiceBus-formatted message.

## Implementing IEnvelopeHandler

Implement the `IEnvelopeHandler` interface and its single method `UnwrapEnvelope`:

```csharp
public class MyEnvelopeHandler : IEnvelopeHandler
{
    public Dictionary<string, string>? UnwrapEnvelope(
        string nativeMessageId,
        IDictionary<string, string> incomingHeaders,
        ReadOnlySpan<byte> incomingBody,
        ContextBag extensions,
        IBufferWriter<byte> bodyWriter)
    {
        // Return null if this handler cannot process the message format.
        if (!CanHandle(incomingHeaders, incomingBody))
        {
            return null;
        }

        // Parse the envelope to extract the inner body and headers.
        var (extractedHeaders, extractedBody) = ParseEnvelope(incomingBody);

        // Write the unwrapped body into the provided writer.
        bodyWriter.Write(extractedBody);

        // Return the extracted headers to be passed to the pipeline.
        return extractedHeaders;
    }
}
```

### UnwrapEnvelope parameters

| Parameter | Description |
|---|---|
| `nativeMessageId` | The native message ID assigned by the transport. Treat as read-only - for diagnostics only. |
| `incomingHeaders` | Headers as provided by the transport. |
| `incomingBody` | The raw body bytes received from the transport. |
| `extensions` | Extension values provided by the transport via `ContextBag`. |
| `bodyWriter` | Write the unwrapped message body here using `IBufferWriter<byte>`. |

**Return value:** Return a `Dictionary<string, string>` of NServiceBus headers if the message was successfully unwrapped, or `null` if this handler does not apply to the message.

NOTE: The `bodyWriter` is automatically reset between handler attempts. Data written to it by a failing or non-matching handler will not affect subsequent handlers.

## Registering an envelope handler

Envelope handlers are registered from within a [feature](/nservicebus/pipeline/features.md) via `FeatureConfigurationContext`:

```csharp
public class MyFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        context.AddEnvelopeHandler<MyEnvelopeHandler>();
    }
}
```

Handlers are instantiated once at endpoint startup and kept alive for the lifetime of the endpoint. Constructor dependencies are resolved from the endpoint's dependency injection container.

## Use cases

- **Third-party system integration** - messages from systems that wrap payloads in proprietary formats.
- **CloudEvents** - receiving messages conforming to the [CloudEvents specification](/nservicebus/cloudevents.md).
- **Legacy format migration** - supporting older message envelope formats alongside current ones during a migration period.
- **Custom transport framing** - transports that add custom metadata or framing outside of NServiceBus headers.

## Related

For a production-ready implementation that handles the [CloudEvents](https://cloudevents.io/) specification (JSON Structured, HTTP Binary, and AMQP Binary content modes), see the [NServiceBus.Envelope.CloudEvents](/nservicebus/cloudevents.md) package.
