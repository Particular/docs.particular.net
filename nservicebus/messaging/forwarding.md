---
title: Message forwarding
summary: Describes how to set up message forwarding
tags:
- Forwarding Messages
redirects:
---


Use this feature to forward successfully processed messages from an endpoint to a specified destination endpoint. Forwarding messages is particularly useful in complex upgrade scenarios, when the old version and new version of a particular endpoint are running side-by-side.


## Auditing vs Fowarding

Technically [Auditing](/nservicebus/operations/auditing.md) and Forwarding are very similar, both send a copy of the processed message to another queue. The main difference are intended usage scenarios.

Auditing is used for collecting information on what is happening in the system, therefore the audited message is enriched with additional [information regarding processing it](/nservicebus/operations/auditing.md#message-headers). The forwarded message is a copy of the processed message, without the additional auditing information.

Note: In Versions 5 and below some of the audit headers were available for the forwarded messages. Starting from Version 6 the forwarded messages will no longer contain the audit message headers.


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
