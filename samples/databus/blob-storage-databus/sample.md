---
title: Azure Blob Storage DataBus
summary: Sending large attachments with NServiceBus over Azure blob storage.
component: ABSDataBus
reviewed: 2025-07-23
related:
- nservicebus/messaging/claimcheck
- samples/databus/blob-storage-databus-cleanup-function
---

 1. Start [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).
 1. Run the solution. Two console applications start.
 1. Find the `Sender` application by looking for the one with `Sender` in its path and press Enter in the window to send a message. A message has been sent that is larger than the allowed 4MB by the transport. NServiceBus sends it as an attachment via Azure storage, allowing it to reach the `Receiver` application.

## Code walk-through

This sample contains three projects:

- Shared - A class library containing shared code including the message definition.
- Sender - A console application responsible for sending the large message.
- Receiver - A console application responsible for receiving the large message from Sender.

### Shared project

Look at the Shared project:

snippet: MessageWithLargePayload

`DataBusProperty<byte[]>` is an NServiceBus data type that instructs NServiceBus to treat the `LargePayload` property as an attachment. It is not transported in the NServiceBus normal flow.

When sending a message using the NServiceBus Message attachments mechanism, the message's payload resides on Azure Storage as a blob. The value assigned to the property while the message is being transported is a special value that allows the message's property to reconnect with its original value once the message is received. An inspected message in flight would look like the following:

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

`Key` represents the Azure Storage blob name used to store the message property's original value.

The `TimeToBeReceived` attribute instructs the NServiceBus framework that it is allowed to clean the message after three minutes if it was not received by the receiver. The message payload will be removed from Azure storage after three minutes.

### Configuring the DataBus location

Both the `Sender` and `Receiver` projects need to share a common location to store large binary objects. This is done by specifying an Azure Storage connection string. This code instructs NServiceBus to use the specified Azure Storage Account for the attachment.

snippet: ConfiguringDataBusLocation

Attachment blobs will be found in the `databus` container.

### Sender project

The following sender project code sends the `MessageWithLargePayload` message, utilizing the NServiceBus attachment mechanism:

snippet: SendMessageLargePayload

Go to the `Receiver` project to see the receiving application.

### Receiver project

This is the receiving message handler:

snippet: MessageWithLargePayloadHandler
