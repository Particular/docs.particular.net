---
title: Message Details
summary: Describes how ServicePulse displays details about messages. Also describes retrying, or deleting of failed messages
component: ServicePulse
reviewed: 2025-04-15
related:
- servicepulse/intro-pending-retries
---

Each message can be browsed to see in-depth details about a given message. Individual messages can be accessed by clicking the respective entry in any of the message list views. If it's a failed message, it can be deleted or retried.

![Failed Messages Page](images/failed-message-page.png 'width=500')

Each individual message page allows for viewing the following message details:

## All Messages

### Message metadata

Failure or Processing timestamp, endpoint name and location, and, for a failed message, any retry status.
![Message Details Metadata](images/message-details-metadata.png 'width=500')

### Headers

A complete set of message headers. This list can be filtered by header key or value, and the value can be copied to clipboard by hovering the cursor over it to reveal a "Copy to clipboard" button.
![Message Details Headers List](images/message-details-headers.png 'width=500')

### Body

The message body, with section expand/collapse and syntax highlighting. Can be copied to clipboard.
The body display can display text serialized message bodies, e.g. XML or JSON. Binary or other serialization formats will result in no body contents being displayed.

Supported content types:
  - `application/json` - supports syntax highlighting
  - `text/xml` - supports syntax highlighting
  - `text/*` - does not support syntax highlighting
  - `application/xml` - supports syntax highlighting
  - `application/*+json` - supports syntax highlighting
  - `application/*+xml` - supports syntax highlighting

![Message Details Body](images/message-details-body.png 'width=500')

## Failed Messages

### StackTrace

The full .NET exception stacktrace. Can be copied to clipboard.
![Message Details StackTrace](images/message-details-stacktrace.png 'width=500')

## Messages with Audited Conversation Data

### Flow diagram

Displays a flow diagram of the conversation that contains the message. This illustrates the message and all related messages from the same conversation, along with the nature of the messages and the endpoints involved. Other messages in the conversation can also be viewed and link to their respective details pages.

Each message is represented by a box (node) indicating the message type and displaying details including time information and, optionally, the sending and receiving endpoints. Published events and sent commands have different icons and illustrations.

Read more about the [flow Diagram](flow-diagram.md).
![Flow Diagram](images/flow-diagram.png 'width=800')

### Saga diagram

Sagas play a critical role in NServiceBus systems. As coordinators of processes, they are started by certain messages and interact with a variety of messages and services. The saga diagram illustrates how the saga was initiated and other messages that were sent or handled, with detailed message data, time information, and details on saga data changes.

Read more about the [Saga Diagram](saga-diagram.md).
![Saga Diagram](images/saga-diagram.png 'width=800')

### Sequence diagram

While a flow diagram is useful for showing why each message in a conversation was sent, a sequence diagram is better for understanding when messages were sent and handled.

Read more about the [Sequence Diagram](sequence-diagram.md).
![Sequence Diagram](images/sequence-diagram.png 'width=800')
