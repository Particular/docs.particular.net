---
title: Attachments / DataBus Sample
summary: 'Send images or video by putting an attribute over your large property. NServiceBus takes care of the rest. '
tags:
- DataBus
- Large messages
redirects:
- nservicebus/attachments-databus-sample
related:
- nservicebus/messaging/databus
---

 1. Run the solution. Two console applications start.
 1. Find the Sender application by looking for the one with "Sender" in its path and pressing Enter in the window to send a message. You have just sent a message that is larger than the allowed 4MB by MSMQ. NServiceBus sends it as an attachment, allowing it to reach the Receiver application.
 1. Click 'e' and Enter. A message larger than the allowed 4MB is sent, but this time without utilizing the NServiceBus attachments mechanism. An exception is thrown at the "Sender" application.


## Code walk-through

This sample contains three projects:

 * Messages - A class library containing the sample messages. Only one of the message types utilizes the NServiceBus DataBus.
 * Sender - A console application responsible for sending the large messages.
 * Receiver - A console application responsible for receiving the large messages from Sender.


### Messages project

Let's look at the Messages project, at the two messages. Start with the large message that is not utilizing the DataBus mechanism. The message is a simple byte array command:

snippet:AnotherMessageWithLargePayload

The other message utilizes the DataBus mechanism:

snippet: MessageWithLargePayload

`DataBusProperty<byte[]>` is an NServiceBus data type that instructs NServiceBus to treat the `LargeBlob` property as an attachment. It is not transported in the NServiceBus normal flow.

When sending a message using the NServiceBus Message attachments mechanism, the message's payload resides in the folder. In addition, a
'signaling' message is sent to the Receiving endpoint.

The `TimeToBeReceived` attribute instructs the NServiceBus framework that it is allowed to clean the MSMQ message after one minute if it was not received by the receiver. The message payload remains in the Storage folder after the MSMQ message is cleaned by the NServiceBus framework.

Following is an example of the signaling message that is sent to the receiving endpoint:

```json
{
	"SomeProperty":"This message contains a large blob that will be sent on the data bus",
	"LargeBlob":
	{
		"Key":"2014-09-29_09\\67de3a8e-0563-40d5-b81b-6f7b27d6431e",
		"HasValue":true
	}
}
```


### Configuring the Databus location

Both the `Sender` and `Receive` project need to share a common location to store large binary objects. This is done by calling `FileShareDataBus`. This code instructs NServiceBus to use the FileSharing transport mechanism for the attachment.

snippet:ConfigureDataBus


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