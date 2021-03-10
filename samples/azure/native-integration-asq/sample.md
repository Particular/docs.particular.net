---
title: Native Integration with Azure Storage Queues Transport
summary: Consuming messages sent by non-NServiceBus endpoints with the Azure Storage Queues transport
reviewed: 2021-02-25
component: ASQ
related:
- nservicebus/azure
- transports/azure-storage-queues
---

## Prerequisites

Ensure an instance of the [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator) or [Azurite](https://github.com/Azure/Azurite) is running.


## Azure Storage Queues transport

This sample uses the [Azure Storage Queues transport](/transports/azure-storage-queues).


## Code walk-through

This sample shows a simple two-endpoint scenario.

 * `NativeSender` sends a `NativeMessage` message to `Receiver`
 * `Receiver` receiving and printing out the contents of the received message.


### Sending a native message

`NativeSender` creates and sends a queue message with a JSON serialized `NativeMessage` payload.

snippet: send-a-native-message


## Receiving a native message

To process a native message, the native `QueueMessage` has to be adapted to the NServiceBus `MessageWrapper` first. To accomplish this, a [custom envelope unwrapper](/transports/azure-storage-queues/configuration.md#custom-envelope-unwrapper) must be registered to provide the following:
1. Message ID to associate with the incoming message
1. Serialized message payload as a byte array
1. Determine the native message type and assign it as an NServiceBus header

snippet: Native-message-mapping

Note: Message type could be determined dynamically by reading the payload if different message types are sent natively.
