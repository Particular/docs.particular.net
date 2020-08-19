---
title: Azure Blob Storage DataBus
summary: Sending large attachments with NServiceBus over Azure blob storage.
component: ABSDataBus
reviewed: 2020-08-19
related:
- nservicebus/messaging/databus
- samples/azure/blob-storage-databus-cleanup-function
---

 1. Start [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator). Ensure [latest version](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) is installed.
 1. Run the solution. Two console applications start.
 1. Find the `Sender` application by looking for the one with `Sender` in its path and press Enter in the window to send a message. A message has been sent is larger than the allowed 4MB by MSMQ. NServiceBus sends it as an attachment via Azure storage, allowing it to reach the `Receiver` application.


## Code walk-through

This sample contains three projects:

 * Shared - A class library containing shared code including the message definition.
 * Sender - A console application responsible for sending the large message.
 * Receiver - A console application responsible for receiving the large message from Sender.


### Shared project

Look at the Shared project:

snippet: MessageWithLargePayload

`DataBusProperty<byte[]>` is an NServiceBus data type that instructs NServiceBus to treat the `LargePayload` property as an attachment. It is not transported in the NServiceBus normal flow.

When sending a message using the NServiceBus Message attachments mechanism, the message's payload resides on Azure storage as a blob. Value assigned to the property while message is being transported is a special value that allows to reconnect message`s property with its original value once message is received. An inspected message in flight would look like the following:


```json
{
	"Description":"This message contains a large payload that will be sent on the Azure data bus",
	"LargePayload":
	{
		"Key":"7c9a4430-c020-4462-a849-9994f3de354a",
		"HasValue":true
	}
}
```

`Key` represents Azure storage blob name used to store message property original value.

The `TimeToBeReceived` attribute instructs the NServiceBus framework that it is allowed to clean the message after three minutes if it was not received by the receiver. The message payload will be removed from Azure storage after three minutes.


### Configuring the DataBus location

Both the `Sender` and `Receiver` project need to share a common location to store large binary objects. This is done by specifying Azure storage connection string. This code instructs NServiceBus to use specified Azure storage account for the attachment.

snippet: ConfiguringDataBusLocation

Attachment blobs will be found in `databus` container.


### Sender project

The following sender project code sends the `MessageWithLargePayload` message, utilizing the NServiceBus attachment mechanism:

snippet: SendMessageLargePayload

Go to the `Receiver` project to see the receiving application.


### Receiver project

Following is the receiving message handler:

snippet: MessageWithLargePayloadHandler
