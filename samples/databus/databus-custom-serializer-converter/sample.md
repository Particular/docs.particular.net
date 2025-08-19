---
title: Databus with SystemJsonSerializer message serializer
summary: The file share data bus allows large properties to be transferred via a Windows file share using custom converter for SystemJsonSerializer
reviewed: 2025-08-03
component: FileShareDataBus
related:
 - nservicebus/messaging/claimcheck
---

Although System.Text.Json is one of the supported serializers from NServiceBus 8, System.Text.Json does not support ISerializable .
Hence when trying to send large messages through "DataBusProperty<>" using a Databus and when using the SystemJsonSerializer as the message serializer in the endpoint configuration, an unhandled exception is thrown.
This can be overcome by using a custom converter that basically does the serialization and deserialization when trying send large messages through "DataBusProperty" using a Databus.

This sample shows how to send large attachments with NServiceBus via Windows file share  using custom converter for SystemJsonSerializer

## Running the sample

 1. Run the solution. Two console applications start.
 1. Find the Sender application by looking for the one with "Sender" in its path
 1. Press <kbd>D</kbd> in the window to send a large message. A message has just been sent that is larger than the limit allowed by the learning transport. NServiceBus sends it as an attachment, allowing it to reach the Receiver application.

> [!WARNING]
> The FileShareDataBus **does not** remove physical attachments once the message has been processed. Apply a custom [cleanup-strategy](/nservicebus/messaging/claimcheck/file-share.md#cleanup-strategy).

## Code walk-through

This sample contains three projects:

* Messages - A class library containing shared code including the message definition and the custom data bus serializer converter.
* Sender - A console application responsible for sending the large messages.
* Receiver - A console application responsible for receiving the large messages from Sender.

### Messages project

The message in the Messages project utilizes the data bus mechanism:

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

#### Custom SystemJsonSerializer Converter

In this sample, both the `Sender` and `Receiver` endpoints use [SystemJsonSerializer](/nservicebus/serialization/system-json.md) for the message serialization. Since System.Text.Json does not support ISerializable , a custom converter is required that basically does the serialization and deserialization when trying send large messages through "DataBusProperty<T>" using a Databus

This sample follows a factory pattern that inherits from JsonConverterFactory to create the converter.

snippet: DatabusPropertyConverterFactory

snippet: DatabusPropertyConverter

### Configuring the databus location

Both the `Sender` and `Receive` project must share a common location to store large binary objects. This is done by calling `FileShareDataBus`. This code instructs NServiceBus to use the FileSharing transport mechanism for the attachment.

snippet: ConfigureDataBus

### Custom serialization option

Both the `Sender` and `Receiver` endpoints use custom [serialization options](/nservicebus/serialization/system-json.md#usage-customizing-serialization-options) to modify how the serialization and deserialization is performed

snippet: CustomJsonSerializerOptions

### Sender project

The following `Sender` project code sends the `MessageWithLargePayload` message, using the NServiceBus attachment mechanism:

snippet: SendMessageLargePayload

### Receiver project

This is the receiving message handler:

snippet: MessageWithLargePayloadHandler
