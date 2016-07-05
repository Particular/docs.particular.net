---
title: ServiceControl Endpoint Plugins
summary: Describes the purpose and internal behavior of the endpoint plugins used by ServiceControl
tags:
- ServiceControl
related:
- samples/custom-checks/monitoring3rdparty
---

ServiceControl is the backend service for ServiceInsight, ServicePulse, and third-party integrations. It collects and stores information from monitored NServiceBus endpoints and exposes this information for consumption by various clients via a HTTP API.

NOTE: When ServiceControl is introduced into an existing environment the standard behavior of error and audit queues will change. When ServiceControl is not monitoring the environment failed messages will remain in the configured error queue and audit messages in the configured audit queue, as soon as ServiceControl is installed and configured messages, in both queues, will be imported by ServiceControl.

Plugins collect information from NServiceBus and are deployed together with each NServiceBus endpoint. These plugins are optional from the perspective of the NServiceBus framework itself (they are not required by the endpoint), but they are required in order to collect the information that enables ServiceControl (and its clients) to provide the relevant functionality for each plugin.

ServiceControl provides the monitoring capability by analyzing the configured error and the audit queues. It can extract information like endpoint name, queue name and in case of error messages the exception stack trace, etc. This information is stored in a built-in internal database.

## Configuring endpoint to be monitored by ServiceControl

To allow ServiceControl to monitor endpoints:

1. ServiceControl should be [installed](/servicecontrol/installation.md) and at least one instance should be configured using the same transport as that of the endpoints that are being monitored.
2. For every endpoint that is being monitored by ServiceControl, [configure the endpoint for auditing](/nservicebus/operations/auditing.md#configuring-auditing). Make sure that the audit queue is the same as the audit queue that ServiceControl is configured with.
3. For every endpoint that is being monitored by ServiceControl, [configure the error queue](/nservicebus/errors/#configure-the-error-queue). Make sure that the error queue is the same as the error queue that ServiceControl is configured with.


## Installing and Deploying Plugins

The ServiceControl plugins are deployed with the endpoints they are monitoring. It is possible add a plugin to an endpoint during development, testing, or production:

 * During development, add the relevant plugin NuGet package to the endpoint's project in Visual Studio using the NuGet.
 * When in production, add the plugin DLLs to the BIN directory of the endpoint, and restart the endpoint process for the changes to take effect and the plugin to be loaded.

**Related articles**

 - [How to configure endpoints for monitoring by ServicePulse](/servicepulse/how-to-configure-endpoints-for-monitoring.md)

 ServiceControl offers the following endpoint plugins:
  - [Heartbeat Plugin](heartbeat.md) - Enables endpoint health monitoring. It sends heartbeat messages at a regular interval from the endpoint to ServiceControl to help determine whether the endpoint is active. This information is shown in ServicePulse.
  - [Saga Plugin](saga-audit.md) - Enables the Saga visualization capabilities useful for debugging during development. The Saga message behavior and the saga state changes as the Saga is being processed are sent to ServiceControl. This information is shows in ServiceInsight. This plugin is only for development purposes and should not be used in production.
  - [Custom Check Plugin](custom-checks.md) - Enables custom monitoring abilities for endpoints by allowing the developer to define a set of conditions that needs to be checked. This plugin will report the results of these custom checks to ServiceControl.

## Connecting to ServiceControl

Once deployed on an active endpoint, the endpoint sends plugin-specific information to ServiceControl. Plugins send messages using the defined endpoint transport to the ServiceControl queue. Location of ServiceControl queue is determined by the following:

1. **Endpoint's configuration file**
Check for an `appSetting` named `ServiceControl/Queue` e.g. `<add key="ServiceControl/Queue" value="particular.servicecontrol"/>`.
1. **Convention based on the configured Error Queue machine**
If an error queue is configured, for example `error@MachineName` with MSMQ transport, then the queue `Particular.ServiceControl@MachineName` will be used.
1. **Convention based on the configured Audit Queue machine**
If an audit queue is configured, for example `audit@MachineName` with MSMQ transport, then the queue `Particular.ServiceControl@MachineName` will be used.

WARNING: Endpoint with plugins installed that cannot communicate to ServiceControl will shut down.

The ServiceControl queue (and all other ServiceControl related sub-queues) are created during the installation phase of ServiceControl.  The  queue name is based on the Window Service name.  If ServiceControl is installed with a Windows Service name other than the default name then the convention based detection does not apply.  In this scenario the configuration setting `ServiceControl/Queue` must be set to the correct queue name.  

NOTE: ServiceControl instances configured to use the MSMQ transport must be installed on the same machine as the error and audit queues.
