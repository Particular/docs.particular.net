title: ServiceInsight Troubleshooting
summary: How to resolve common ServiceInsight issues
reviewed: 2021-10-05
---

## ServiceInsight is slow

Loading a conversation is slow when selecting messages in the grid.

1. Audit instance(s) might not be running.
2. ServiceControl remotes configuration might be incorrect. See [ServiceControl Troubleshooting - Faulty remotes configuration](/servicecontrol/troubleshooting.md#faulty-remotes-configuration)

## ServiceInsight isn't show succesfully processed messages

When ServiceInsight isn't showing succesfully processed messages (green icon) this then:

1. Auditing might not be enabled, see [NServiceBus auditing](/nservicebus/operations/auditing.md).
2. ServiceControl Audit instance(s) might not be running.
3. ServiceControl remotes configuration might be incorrect. See [ServiceControl Troubleshooting - Faulty remotes configuration](/servicecontrol/troubleshooting.md#faulty-remotes-configuration)
