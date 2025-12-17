---
title: ServiceInsight Troubleshooting
summary: How to resolve common ServiceInsight issues
reviewed: 2025-08-07
component: ServiceInsight
---

include: serviceinsight-sunset

## ServiceInsight is slow

If ServiceInsight takes a long time to load a conversation when selecting messages in the grid, verify if one of the following scenarios is occurring:

1. Audit instance(s) might not be running.
1. ServiceControl remotes configuration might be incorrect. See [ServiceControl Troubleshooting - API is slow](/servicecontrol/troubleshooting.md#api-is-slow)

## ServiceInsight isn't showing successfully processed messages

If ServiceInsight isn't showing successfully processed messages (i.e. messages with a green icon):

1. Auditing might not be enabled, see [NServiceBus auditing](/nservicebus/operations/auditing.md).
2. ServiceControl Audit instance(s) might not be running.
3. ServiceControl remotes configuration might be incorrect. See [ServiceControl Troubleshooting - API is slow](/servicecontrol/troubleshooting.md#api-is-slow)
