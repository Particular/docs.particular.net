---
title: ServiceInsight Troubleshooting
summary: How to resolve common ServiceInsight issues
reviewed: 2022-05-12
---

## ServiceInsight is slow

Loading a conversation is slow when selecting messages in the grid.

1. Audit instance(s) might not be running.
2. ServiceControl remotes configuration might be incorrect. See [ServiceControl Troubleshooting - API is slow](/servicecontrol/troubleshooting.md#api-is-slow)

## ServiceInsight isn't showing succesfully processed messages

When ServiceInsight isn't showing succesfully processed messages (green icon) this then:

1. Auditing might not be enabled, see [NServiceBus auditing](/nservicebus/operations/auditing.md).
2. ServiceControl Audit instance(s) might not be running.
3. ServiceControl remotes configuration might be incorrect. See [ServiceControl Troubleshooting - API is slow](/servicecontrol/troubleshooting.md#api-is-slow)
