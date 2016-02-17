---
title: Message forwarding
summary: Describes how to set up message forwarding
tags:
- Forwarding Messages
redirects:
---

## Configuring Message Forwarding

Use this feature to forward successfully processed messages from an endpoint to a specified destination endpoint. If there is a need to collect all successfully processed messages from all endpoints, then use the [Auditing Feature](/nservicebus/operations/auditing.md) instead.

### Using Code

snippet:ForwardingWithCode

### Using app.config

WARNING: The `ForwardReceivedMessagesTo` attribute in the `UnicastBusConfig` configuration section is no longer supported starting from Version 6. Please switch to the code first API when using Version 6 and above.

snippet:configureForwardingUsingXml

### Using IProvideConfiguration

WARNING: The `ForwardReceivedMessagesTo` attribute in the `UnicastBusConfig` configuration section is no longer supported starting from Version 6. Please switch to the code first API when using Version 6 and above.

snippet:ProvideConfigurationForMessageForwarding

### Using IConfigurationSource

WARNING: The `ForwardReceivedMessagesTo` attribute in the `UnicastBusConfig` configuration section is no longer supported starting from Version 6. Please switch to the code first API when using Version 6 and above.

snippet:ConfigurationSourceForMessageForwarding

Initialize the Configuration Source as follows:

snippet:ConfigurationSourceUsageForMessageForwarding 