---
title: Attachments / DataBus Sample
summary: 'Send images or video by putting an attribute over your large property. NServiceBus takes care of the rest. '
tags:
- DataBus
- MSMQ Limit
- Large messages
---

Large chunks of data such as images or video files can be transported using NServiceBus DataBus.

You only have to put an attribute over your large property and NServiceBus takes care of the rest. This is particularly important when running in cloud environments where limits on message size are usually much lower than on-premise.

To see how to send and receive attachments in NServiceBus, open the [Databus sample](https://github.com/Particular/NServiceBus.Msmq.Samples/tree/master/DataBus):

 1. Run the solution. Two console applications start.
 2. Find the Sender application by looking for the one with "Sender" in its path and pressing Enter in the window to send a message.      You have just sent a message that is larger than the allowed 4MB by MSMQ. NServiceBus sends it as an attachment, allowing it to reach the Receiver application.
 3. Click 'e' and Enter. A message larger than the allowed 4MB is sent, but this time without utilizing the NServiceBus attachments mechanism. An exception is thrown at the "Sender" application as shown below:

## Code walk-through

This sample contains three projects:

 * Messages - A class library containing the sample messages. Only one of the message types utilizes the NServiceBus DataBus.
 * Sender - A console application responsible for sending the large messages.
 * Receiver - A console application responsible for receiving the large messages from Sender.

### Messages project

Let's look at the Messages project, at the two messages. We start with the large one that is not utilizing the DataBus mechanism. The message is a simple byte array command:

```C#
public class AnotherMessageWithLargePayload : ICommand
{
    public byte[]LargeBlob { get; set; }
}
```

The other message utilizes the DataBus mechanism:

```C#
[TimeToBeReceived("00:01:00")]
public class MessageWithLargePayload : ICommand
{
    public string SomeProperty { get; set; }
    public DataBusProperty<byte[]> LargeBlob { get; set; }
}
```

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

Both the Sender and Receive project need to share a common location to store large binary objects. This is done by callin `FileShareDataBus`. This code instructs NServiceBus to use the FileSharing transport mechanism for the attachment. 

```C#
static string BasePath = "..\\..\\..\\storage";
static void Main()
{
    ...
    busConfiguration.FileShareDataBus(BasePath);
    ...
}
```

### Sender project

The following sender project code sends the MessageWithLargePayload message, utilizing the NServiceBus attachment mechanism:

```C#
var message = new MessageWithLargePayload
{
    SomeProperty = "This message contains a large blob that will be sent on the data bus",
    LargeBlob = new DataBusProperty<byte[]>(new byte[1024*1024*5]) //5MB
};
bus.Send("Sample.DataBus.Receiver",message);
```

The following Sender project code sends the AnotherMessageWithLargePayload message without utilizing the NServiceBus attachment mechanism:

```C#
var message = new AnotherMessageWithLargePayload
{
    LargeBlob = new byte[1024 * 1024 * 5] //5MB
};
bus.Send("Sample.DataBus.Receiver", message);
```

In both cases, a 5MB message is sent, but in the MessageWithLargePayload it goes through, while AnotherMessageWithLargePayload fails.

Go to the Receiver project to see the receiving application.

### Receiver project

Following is the receiving message handler:

```C#
public class MessageWithLargePayloadHandler : IHandleMessages<MessageWithLargePayload>
{
    public void Handle(MessageWithLargePayload message)
    {
        Console.WriteLine("Message received, size of blob property: " + message.LargeBlob.Value.Length + " Bytes");
    }
}
```