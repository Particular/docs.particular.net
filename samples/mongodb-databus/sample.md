---
title: MongoDB DataBus
reviewed: 2018-01-29
component: MongoDatabusTekmaven
tags:
 - DataBus
related:
 - persistence/mongodb-tekmaven
 - nservicebus/messaging/databus
---
## Prerequisites
Local installation of MongoDB

## Running the project
 1. Run the solution. Two console applications will start.
 1. Find the Sender application by looking for the console with "Sender" in its path. Press Enter in the window to send a message. A message has just been sent that is larger than the allowed 4MB by MSMQ. NServiceBus sends it as an attachment, allowing it to reach the Receiver application.
 1. Click 'e' and Enter. A message larger than the allowed 4MB is sent, but this time without utilizing the NServiceBus attachments mechanism. An exception is thrown at the "Sender" application.


## Code walk-through

This sample contains three projects:

 * Messages - A class library containing the sample messages. Only one of the message types utilizes the DataBus.
 * Sender - A console application responsible for sending the large messages.
 * Receiver - A console application responsible for receiving the large messages from Sender.


### Messages project

There are two messages defined in the Messages project. Start by looking at `AnotherMessageWithLargePayload` which is not utilizing the DataBus mechanism. The message is a simple byte array command:

snippet: AnotherMessageWithLargePayload

The `MessageWithLargePayload` message utilizes the DataBus mechanism:

snippet: MessageWithLargePayload


### Configuring the Databus location

Both the `Sender` and `Receive` project need to share a common location to store large binary objects.

snippet: ConfigureDataBus

Note that the connection string used for the databus is shared by the [MongoDB Persistence](/persistence/mongodb-tekmaven).


### Sender project

The following `Sender` project code sends the `MessageWithLargePayload` message utilizing the NServiceBus attachment mechanism:

snippet: SendMessageLargePayload

The following `Sender` project code sends the `AnotherMessageWithLargePayload` message without utilizing the NServiceBus attachment mechanism:

snippet: SendMessageTooLargePayload

In both cases, a 5MB message is sent, but in the `MessageWithLargePayload` message the payload goes through, while the `AnotherMessageWithLargePayload` message fails.

Go to the `Receiver` project to see the receiving application.


### Receiver project

Following is the receiving message handler:

snippet: MessageWithLargePayloadHandler
