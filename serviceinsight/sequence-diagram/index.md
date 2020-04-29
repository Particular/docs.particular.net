---
title: Sequence Diagram
summary: The ServiceInsight sequence diagram view
reviewed: 2020-04-29
component: ServiceInsight
---

The sequence diagram shows when messages in the same conversation as the selected message were sent and handled relative to each other. It shows the interaction of endpoints involved in the conversation to give an understanding of the sequence of messages that occured during that conversation.

![Sequence Diagram](sequence-diagram.png 'width=500')

## How the diagram is generated

When sending or publishing a message, NServiceBus adds identifying headers such as `NServiceBus.MessageId`, which is a unique ID for that message. The first message to be sent in a conversation will also get a unique `ConversationId` header.

If handling an incoming message results in outgoing messages, those messages are given an `NServiceBus.RelatedTo` header equal to the `MessageId` of the incoming message. They are also given the same `ConversationId` as the incoming message.

Additional headers are used to determine endpoint names, message intent, and other factors. [Read more about headers](/nservicebus/messaging/headers.md).

Note: The order of operations is critical to the correctness of the sequence diagram. When endpoints are running on more than one machine, it is crucial that those machines have synchronised clocks. This can be done using an NTP server.

## What is on the diagram

### Endpoints and lifelines

![Endpoint](endpoint.png)

Each endpoint involved in the conversation is represented by a gray box along the top of the diagram. Hover on the endpoint to get more information such as the host that the endpoint is running on. Extending from the bottom of each endpoint is a lifeline that shows time flowing from top to bottom.


### Start of conversation

![Start of conversation marker](start-of-conversation.png)

Each conversation starts with a single command or event. This message is often triggered by some action external to the system (for example, a user clicking a Submit Order button on a website). The metadata used to generate the diagram does not include the trigger in these cases but it does include the endpoint that sent or published the message, represented by a Start of Conversation marker on the endpoint lifeline.

NOTE: The Start of Conversation marker may not always be visible. This can happen if the conversation started a long time ago and the initiating message has expired. It can also happen if the number of messages in the conversation is very large. The diagram will show only the last 50 messages from the conversation and this may not include the initiating message.


### Messages

![Send message](send.png 'width=500')

When a message is sent, a solid arrow is drawn from the lifeline of the sending endpoint to the lifeline of the receiving endpoint labelled with the message type. The arrow always points from the sending endpoint to the receiving endpoint.

![Message context menu](context-menu.png)

Right-clicking on a message label shows actions related to that message.

![Message processing box](handler.png)

Some time after a message is received, it is processed by the receiving endpoint. This is represented by a box on the endpoint lifeline labelled with the message type. If the endpoint received more than one message of that type, the specific message being processed may be seen by hovering over or selecting the processing box.

A message arrow shown leaving a processing box represents an outgoing message that was sent or published while processing the incoming message. These arrows are shown in the order in which those messages were sent or published.

NOTE: A processing box appearing further down the diagram indicates that the processing occurred later. This means other messages sent by other endpoints may have been processed in the meantime. The size of a processing box and the distance between them is not important.

![Failed processing](failing-handler.png)

If the processing of a message failed, the processing box is displayed in red with an exclamation mark. If the message has not already been (automatically) retried, it can be retried manually via the right-click context menu.


### Events

![Event](event.png 'width=500')

Events are represented similarly to other messages but with dashed lines and a different icon.

NOTE: Each event published will appear once for each subscriber, as if individual messages were sent to each subscriber by the sender, regardless of whether unicast or multicast routing is used. [Learn more about message routing](/nservicebus/messaging/routing.md).


### Loopback messages

![Loopback message](loopback.png)

A loopback message is a message that an endpoint sends to itself and is represented by a short arrow with a special icon that does not connect to another endpoint lifeline. As with any other message, hovering over or selecting the message will highlight the processing for that message in the lifeline.


### Timeout messages

![Timeout message](timeout.png)

A timeout message is a special type of loopback message where handling is deferred until later. It is represented as a loopback message with a clock icon.

NOTE: The time of processing may not correspond to the time at which a timeout message was sent back for processing by the timeout message scheduler. The sequence diagram does not currently support visualization of the time at which the timeout message was sent back and will only indicate when it was processed.


### Differences from UML sequence diagrams

The language used in the sequence diagram is largely modeled after the standard defined by UML sequence diagrams. However, due to technical limitations and specifics related to messaging systems, the sequence diagram in ServiceInsight has some notable differences:

| Item | ServiceInsight | UML |
| :--- | :---: | :---: |
| **Start of sequence** | ![Sequence beginning](sequence-beginning.png) | ![Sequence beginning](uml-sequence-beginning.png) |
| | Represented by a black rectangle with a white "play" icon. This representation is used because metadata about what precedes the sequence is unavailable. | Represented by an incoming message (gate) from outside the diagram. |
| **Uni-directional solid lines** | ![Solid line](solid-line.png) | ![UML solid line](uml-solid-line.png) |
| | Used to represent any type of message other than events, including response messages. | Used solely for send and request types of messages. |
| **Uni-directional dashed lines** | ![Dashed line](dashed-line.png) | ![UML dashed line](uml-dashed-line.png) |
| | Used solely to represent event messages. | Used solely to represent create messages and response messages. |
| **Filled arrow style** | ![Filled arrow](filled-arrow.png) | ![UML filled arrow](uml-filled-arrow.png) |
| | Used for all message types. | Used solely for synchronous send messages. |
| **Open arrow style** | - | ![UML open arrow](uml-open-arrow.png) |
| | N/A | Used for response messages and asynchronous messages. |
| **Asynchronous messages** | - | ![UML asynchronous messages](uml-asynchronous.png) |
| | N/A - all NServiceBus messages are asynchronous so the ServiceInsight sequence diagram has no special representation for asynchronous messages and no representation for synchronous messages, even though messages may exhibit synchronous behavior by design. | Represented by a sloping dashed or solid line with an open arrow. |
| **Send to self / loopback messages** | ![Loopback message](loopback-si.png) | ![UML loopback message](uml-loopback.png)
| | Represented by a short uni-directional arrow that does not connect to another endpoint lifeline and a specific icon. | Represented by an arrow that connects back to the sending object's lifeline. It is immediately followed by its handler, which usually overlaps the handler that sent the loopback message. |
| **Handlers** | - | ![UML handler](uml-handler.png) |
| | N/A - currently it's not possible to collect telemetry data to visualize message handlers. | Represented by rectangles directly attached to arrow heads. |
| **Message Processing** | ![Message processing](processing.png) | - |
| | Represented by a labeled rectangle not connected with the arrows of its parent messages. This representation was chosen to reflect that the execution/processing of incoming messages may occur only after several other messages were sent and also to reflect the default asynchronous nature of any associated response messages. | N/A |
