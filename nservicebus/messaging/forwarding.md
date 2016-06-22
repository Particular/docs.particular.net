---
title: Message forwarding
summary: Describes how to set up message forwarding
tags:
- Forwarding Messages
redirects:
---


Use this feature to forward successfully processed messages from an endpoint to a specified destination endpoint. Forwarding messages is particularly useful in complex upgrade scenarios, when the old version and new version of a particular endpoint are running side-by-side.

Note that forwarding is a solution to a specific problem and generally should be used for a limited transition period. In case of long-term need to duplicate messages the [Publish-Subscribe](/nservicebus/messaging/publish-subscribe/) pattern will be usually more appropriate.


## Auditing vs Fowarding

Technically [Auditing](/nservicebus/operations/auditing.md) and Forwarding are very similar, both send a copy of the processed message to another queue. The main difference are intended usage scenarios.

Auditing is used for collecting information on what is happening in the system, therefore the audited message is enriched with additional [information regarding processing it](/nservicebus/operations/auditing.md#message-headers). The forwarded message is a copy of the message, without the additional auditing information.

Note: In Version 5 and below the forwarded messages contain some of the headers added during auditing. Those headers are no longer added in Version 6 and above.


## Configuring Message Forwarding

### Using Code

snippet:ForwardingWithCode


### Using app.config

WARNING: The `ForwardReceivedMessagesTo` attribute in the `UnicastBusConfig` configuration section is no longer supported starting from Version 6. Switch to the code first API when using Version 6 and above.

snippet:configureForwardingUsingXml


### Using IProvideConfiguration

WARNING: The `ForwardReceivedMessagesTo` attribute in the `UnicastBusConfig` configuration section is no longer supported starting from Version 6. Switch to the code first API when using Version 6 and above.

snippet:ProvideConfigurationForMessageForwarding


### Using IConfigurationSource

WARNING: The `ForwardReceivedMessagesTo` attribute in the `UnicastBusConfig` configuration section is no longer supported starting from Version 6. Switch to the code first API when using Version 6 and above.

snippet:ConfigurationSourceForMessageForwarding

Initialize the Configuration Source as follows:

snippet:ConfigurationSourceUsageForMessageForwarding
