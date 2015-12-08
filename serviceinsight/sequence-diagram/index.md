---
title: Sequence Diagram
summary: ServiceInsight Sequence Diagram view
tags: 
- ServiceInsight
- Sequence Diagram
---

# Introduction

Each visualization diagram in ServiceInsight displays the currently selected selected message in a broader context to help with troubleshooting and debugging.

The sequence diagram shows all of the messages in the same conversation. It also shows when these messages were sent and handled relative to each other. 

!Image of Sequence Diagram

Use the sequence diagram when the timing of when messages were handled is important.

## Which data is used to generate the diagram

When you send a message through NServiceBus, some headers are added automatically. All messages get the `NServiceBus.MessageId` header. This is a unqiue identifier for the message. If handling a message causes more messages to be sent or published then the `MessageId` of the message being handled is copied to the `NServiceBus.RelatedTo` header of the outgoing messages.


## What is on the diagram

The sequence diagram shows a complex interraction of endpoints sending and processing messages over time. 

### Endpoints and timelines

!Image of endpoints

Each endpoint involved in the conversation is represented as a grey box along the top of the diagram. Hover over the endpoint ot get additional information such as the host that the endpoint is running on. Extending from the bottom of each endpoint is a line that shows time flowing from the endpoint to the bottom of the diagram. Elements that appear further down in the diagram happen later.

### Start of conversation

!Image of start of conversation marker

Each conversation is initiated by a single command or event. This message is often triggered by some action which is external to the system such as a user clicking a Submit Order button on a website. The metadata used to generate the diagram doesn't include the trigger in this case but it does include the endpoint that sent or published this message. This is represented by a Start of Conversation marker on the endpoint timeline.

NOTE: Not every sequence diagram will have the start of conversation marker. This can happen if the conversation started a long time ago and the initiating message has been expired. It can also happen if the number of messages in the conversation is very large. In this case, ServiceInsight will only get 50 messages from the conversation and this may not include the initiating message.

### Messages and Handlers

!Image of send (preferrably with a send coming back again)

When an endpoint sends a message a solid arrow is drawn from the timeline of the sending endpoint to the timeline of the processing endpoint. Each sent message will be labelled with the type of that was sent. Note that message arrow can face left or right depending on which order the Endpoints appear at the top of the diagram. The arrow always points from the sender and to the receiver.

!Image of context menu

You can right-click on a message label to get access to additional data and actions about a message.

!Image of handler

Sometime after a message is received it is handled by the endpoint. This is represented as a box on the endpoint timeline. Each handler is labelled with the type of message that it is handling. If the endpoint has received multiple messages of the same type you can see which specific instance is being handled by each handler by hovering over the message label or the handler.

If a message arrow is drawn coming out of a handler then that means that the message was sent as a part of executing the handler. The order that message arrows appear within a handler is representative of the order they were sent.

NOTE: If a handler appears further down the diagram then it was executed later. The size of a handler and the distance between them is not important.

!Image of failing handler

If the processing of a message fails, the handler will be displayed in red with an exclamation mark in it. 

### Events

!Image of an event

Events are represented in a similar fashion to other messages except that they have dashed lines and a different icon. 

NOTE: Each event that is published will appear on the diagram once for each subscriber. This looks like individual messages were sent to each subscriber by the sender regardless of whether Unicast or Multicast routing is used. See [Message Routing](/nservicebus/messaging/routing.md) for more details on routing. 

### Loopback messages

!Image of a loopback

When an endpoint sends a message to itself this is called a loopback message. On the sequence diagram this is represented as a short arrow that does not connect to another endpoint timeline. Each loopback message is displayed with a special icon to make it clear that's what it is. As with any other type of message, hovering over or selecting the message will highlight the handler for that message in the timeline.

### Timout messages

!Image of a timeout

A timeout is a special type of loopback message where the handling is deferred until a later time. This type of message is represented just like a loopback message but it has a clock icon to show that it is a timeout.

