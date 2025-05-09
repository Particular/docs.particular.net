---
title: Installing the Custom Checks Plugin
summary: Enabling custom endpoint instance monitoring by installing the custom checks plugin
reviewed: 2025-04-30
component: CustomChecks
versions: 'CustomChecks:*'
---

> [!NOTE]
> This plugin can be enabled and configured with the [ServicePlatform Connector plugin](/platform/connecting.md).

To install the custom checks plugin into an endpoint, reference the [NServiceBus.CustomChecks NuGet package](https://www.nuget.org/packages/NServiceBus.CustomChecks/) and add the following to the endpoint configuration:

snippet: CustomCheckNew_Enable

> [!NOTE]
> `ServiceControl_Queue` is a placeholder for the actual ServiceControl Instance (Not a ServiceControl Audit instance) input queue. The ServiceControl input queue is equal to the [ServiceControl instance name](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolinstancename).

It may not make sense to enable the custom checks plugin in all environments. For instance, a development environment may not have a running ServiceControl instance to consume custom check messages. In these cases, enable the plugin conditionally, based on an environment variable or configuration setting.


### Time-To-Live (TTL)

The `timeToLive` (TTL) parameter is optional and defaults to four times the interval for periodic checks or infinite for one-time checks. Some queue systems (e.g. MSMQ) handle TTL proactively by removing timed out messages from the queues. Others do it only when a message is about to be received. Running the custom checks plugin without ServiceControl installed may cause the destination queue to grow indefinitely and consume system resources.
