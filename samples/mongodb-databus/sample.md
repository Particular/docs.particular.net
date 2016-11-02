---
title: MongoDB DataBus
reviewed: 2016-03-21
component: MongoDatabusTekmaven
tags:
 - DataBus
 - Large messages
related:
 - nservicebus/mongodb-persistence-tekmaven
 - nservicebus/messaging/databus
---

 1. Run the solution. Two console applications start.
 1. Find the Sender application by looking for the one with "Sender" in its path and pressing Enter in the window to send a message. A message has just been sent that is larger than the allowed 4MB by MSMQ. NServiceBus sends it as an attachment, allowing it to reach the Receiver application.
 1. Click 'e' and Enter. A message larger than the allowed 4MB is sent, but this time without utilizing the NServiceBus attachments mechanism. An exception is thrown at the "Sender" application.


## Code walk-through

This sample contains three projects:

 * Messages - A class library containing the sample messages. Only one of the message types utilizes the DataBus.
 * Sender - A console application responsible for sending the large messages.
 * Receiver - A console application responsible for receiving the large messages from Sender.


### Messages project

Look at the Messages project, at the two messages. Start with the large message that is not utilizing the DataBus mechanism. The message is a simple byte array command:

snippet:AnotherMessageWithLargePayload

The other message utilizes the DataBus mechanism:

snippet: MessageWithLargePayload


### Configuring the Databus location

Both the `Sender` and `Receive` project need to share a common location to store large binary objects.

snippet:ConfigureDataBus

Note that the connection string used for the databus is shared by the [MongoDB Persistence](/nservicebus/mongodb-persistence-tekmaven).


### Sender project

The following sender project code sends the `MessageWithLargePayload `message, utilizing the NServiceBus attachment mechanism:

snippet:SendMessageLargePayload

The following `Sender` project code sends the `AnotherMessageWithLargePayload` message without utilizing the NServiceBus attachment mechanism:

snippet: SendMessageTooLargePayload

In both cases, a 5MB message is sent, but in the `MessageWithLargePayload `it goes through, while `AnotherMessageWithLargePayload` fails.

Go to the `Receiver` project to see the receiving application.


### Receiver project

Following is the receiving message handler:

snippet: MessageWithLargePayloadHandler