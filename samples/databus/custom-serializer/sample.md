---
title: Custom Serializer for DataBus
reviewed: 2018-03-16
component: Core
tags:
 - DataBus
related:
 - nservicebus/messaging/databus
---

1. Run the solution. Two console applications start.
2. Find the Sender application by looking for the one with "Sender" in it's console title and press Enter to send a message. A message is sent that includes a DataBus property that is serialized using a custom JSON serializer.
3. The sample uses the `FileShareDataBus`. Open the solution folder in Windows Explorer and navigate to the `\storage\` sub-folder. Each sub-folder within the `\storage` folder contains serialized DataBus properties. 

WARNING: The FileShareDataBus **does not** remove physical attachments once the message has been processed. Apply a custom [cleanup-strategy](/nservicebus/messaging/databus/file-share.md#cleanup-strategy).

## Code walk-through

This sample contains three projects:

 * Shared - A class library with messages and the custom DataBus serializer.
 * Sender - A console application responsible for sending large messages.
 * Receiver - A console application responsible for receiving large messages from the sender.


### Shared project

Look at the custom DataBus serializer:

snippet: CustomDataBusSerializer

The custom serializer implements `IDataBusSerializer`. 


### Sender project

The endpoint in the Sender project is configured to use the custom DataBus serializer:

snippet: ConfigureSenderCustomDataBusSerializer


### Receiver project

The endpoint in the Receiver project must be configured to use the same custom DataBus serializer in order to read DataBus properties sent by the Sender endpoint:

snippet: ConfigureReceiverCustomDataBusSerializer
