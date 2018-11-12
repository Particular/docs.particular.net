---
title: Oversized Sends
reviewed: 2017-05-05
component: ASB
versions: '[7,)'
tags:
- Azure
- Error Handling
redirects:
 - nservicebus/azure-service-bus/oversized-sends
---

include: legacy-asb-warning

As mentioned in [error handling](error-handling.md#azure-service-bus-sdk-message-size-problems) the Azure Service Bus SDK has a peculiar behavior when it comes to computing the message size, resulting in the possibility that oversized messages get sent even when not intended.

The transport handles this scenario by advising to use [the databus](/nservicebus/messaging/databus/) to send oversized messages instead. However, in certain scenarios, this is not the desired behavior. Therefore it is possible to override this behavior.

One such scenario is, for example, an HTTP API that allows bulk uploading of arrays of entities to process. The API has logic to split these arrays into parts that would normally fit inside a brokered message, but due to the unknown entity property content and the internal serialization overhead associated with that, the message suddenly exceeds the maximum body size. In this scenario, the message to be sent would be lost, and it would be up to the caller of the API to deal with the problem.


## Configuring an oversized brokered message handler

To provide an alternate behavior, a custom implementation of `IHandleOversizedBrokeredMessages` can be configured

snippet: asb-configure-oversized-messages-handler

and registered using the `OversizedBrokeredMessageHandler` API.

snippet: asb-configure-oversized-messages-handler-config
