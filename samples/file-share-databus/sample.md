---
title: File Share Data Bus Usage
reviewed: 2019-12-12
component: FileShareDataBus
redirects:
 - nservicebus/attachments-databus-sample
related:
 - nservicebus/messaging/databus
---

 1. Run the solution. Two console applications start.
 1. Find the Sender application by looking for the one with "Sender" in its path
 1. Press `d` in the window to send a large message. A message has just been sent that is larger than the limit allowed by the learning transport. NServiceBus sends it as an attachment, allowing it to reach the Receiver application.
 1. Click 'n' in the Sender window. A message larger than the allowed limit is sent, but this time without utilizing the NServiceBus attachments mechanism. An exception is thrown in the "Sender" application.

WARNING: The FileShareDataBus **does not** remove physical attachments once the message has been processed. Apply a custom [cleanup-strategy](/nservicebus/messaging/databus/file-share.md#cleanup-strategy).


## Code walk-through

This sample contains three projects:

 * Messages - A class library containing the sample messages. Only one of the message types utilizes the data bus.
 * Sender - A console application responsible for sending the large messages.
 * Receiver - A console application responsible for receiving the large messages from Sender.


### Messages project

Look at the two messages in the Messages project. Start with the large message that is not utilizing the data bus mechanism. The message is a simple byte array command:

snippet: AnotherMessageWithLargePayload

The other message utilizes the data bus mechanism:

snippet: MessageWithLargePayload

`DataBusProperty<byte[]>` instructs NServiceBus to treat the `LargeBlob` property as an attachment. It is sent separately from other message properties.

When sending a message using the file share data bus, the `DataBus` properties get serialized to a file. Other properties are included in a message sent to the Receiving endpoint.

The `TimeToBeReceived` attribute indicates that the message can be deleted after one minute if not processed by the receiver. The message payload remains in the storage directory after the message is cleaned by the NServiceBus framework.

Following is an example of the message with `DataBus` property that is sent to the receiving endpoint:

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


### Configuring the databus location

Both the `Sender` and `Receive` project must share a common location to store large binary objects. This is done by calling `FileShareDataBus`. This code instructs NServiceBus to use the FileSharing transport mechanism for the attachment.

snippet: ConfigureDataBus


### Sender project

The following `Sender` project code sends the `MessageWithLargePayload `message, using the NServiceBus attachment mechanism:

snippet: SendMessageLargePayload

The following `Sender` project code sends the `AnotherMessageWithLargePayload` message _without_ using the NServiceBus attachment mechanism:

snippet: SendMessageTooLargePayload

In both cases, a 5MB message is sent, but the `MessageWithLargePayload` message is successfully delivered, while `AnotherMessageWithLargePayload` fails.


### Receiver project

This is the receiving message handler:

snippet: MessageWithLargePayloadHandler