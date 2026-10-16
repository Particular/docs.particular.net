---
title: Azure Service Bus Session support
component: ASBS
summary: How Azure Service Bus transport supports ordered delivery through the use of sessions
related:
 - transports/azure-service-bus/configuration
reviewed: 2026-10-16
---

## Ordered delivery

Azure Service Bus supports ordered delivery through the use of sessions.

In order to leverage the session support in Azure Service Bus, the transport needs to be configured to use sessions. This can be done by setting the `EnableSessions` property on the endpoint configuration.
This will ensure that, when installers are enabled, the transport will
create the relevant queues, topics and subscriptions with sessions enabled.

snippet: enable-sessions

When sessions are enabled, the transport will create a session for each
message sent to the queue or topic, based on the session ID. The session ID
can be set using the `SendOptions` or `PublishOptions`, depending on the
operation used, using the `SessionId` property.

To send a message to an ordered queue:

snippet: sending-set-session-id

To publish a message to an ordered topic:

snippet: publishing-set-session-id

-- todo: we could possibly drop this, thoughts?
When receiving messages, the transport will automatically accept the session and process messages in the order they were sent, based on the session ID.
Note that when using sessions, the transport will only process one message per
session at a time per endpoint instance. This means that if multiple messages
are available on a session, they will be processed sequentially, one after the
other.

It's important to note that when sending or publishing to a session-enabled queue or topic, the session ID must be set for each message.
If the session ID is not set, the message will be rejected by the broker.
A session-enabled queue or topic can therefore ONLY process messages that
have a session ID.

### Failures

todo


### Session-state

todo

### Endpoint-licensing

If there's a need to have part of a logical endpoint's message types to be
processed in order, and part of the message types to be processed out of order, then the logical endpoint needs to be split into two physical endpoints. One physical endpoint will have sessions enabled, and the other physical endpoint will have sessions disabled.
These will have a slightly different name, but will be treated as a single
logical endpoint and will therefore be licensed as a single endpoint.