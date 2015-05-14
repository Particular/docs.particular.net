---
title: Azure Blob Storage DataBus Sample
summary: 'Send large attachments with NServiceBus over Azure blob storage.'
tags:
- Azure
- DataBus
- Large messages
- Message size limit
related:
- nservicebus/messaging/databus
- samples/databus
---

 1. Start Azure Storage Emulator
 1. Run the solution. Two console applications start.
 1. Find the Sender application by looking for the one with "Sender" in its path and press Enter in the window to send a message. You have just sent a message that is larger than the allowed 4MB by MSMQ. NServiceBus sends it as an attachment via Azure storage, allowing it to reach the Receiver application.
 
## Code walk-through

This sample contains three projects: 

 * Messages - A class library containing the sample message.
 * Sender - A console application responsible for sending the large message.
 * Receiver - A console application responsible for receiving the large message from Sender.

### Messages project
 
Let's look at the Messages project:

<!-- import MessageWithLargePayload -->

`DataBusProperty<byte[]>` is an NServiceBus data type that instructs NServiceBus to treat the `LargePayload` property as an attachment. It is not transported in the NServiceBus normal flow.

When sending a message using the NServiceBus Message attachments mechanism, the message's payload resides on Azure storage as a blob. Value assigned to the property while message is being transported is a special value that allows to reconnect message`s property with its original value once message is received. An inspected message in flight would look like the following:


```json
{
	"SomeProperty":"This message contains a large payload that will be sent on the Azure data bus",
	"LargeBlob":
	{
		"Key":"7c9a4430-c020-4462-a849-9994f3de354a",
		"HasValue":true
	}
}
```

`Key` represents Azure storage blob name used to store message property original value.


The `TimeToBeReceived` attribute instructs the NServiceBus framework that it is allowed to clean the message after three minutes if it was not received by the receiver. The message payload will be removed from Azure storage after three minutes.


### Configuring the DataBus location

Both the `Sender` and `Receive` project need to share a common location to store large binary objects. This is done by specifying Azure storage connection string. This code instructs NServiceBus to use specified Azure storage account for the attachment. 

```C#
static void Main()
{
    ...
    busConfiguration.UseDataBus<AzureDataBus>().ConnectionString("UseDevelopmentStorage=true");
    ...
}
```

Attachment blobs will be found in `databus` container.

 
### Sender project

The following sender project code sends the `MessageWithLargePayload `message, utilizing the NServiceBus attachment mechanism:

<!-- import SendMessageLargePayload -->

Go to the `Receiver` project to see the receiving application.

### Receiver project

Following is the receiving message handler:

<!-- import MessageWithLargePayloadHandler --> 