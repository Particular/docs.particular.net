---
title: ServiceControl Endpoint Plugins
summary: Describes the purpose and internal behavior of the endpoint plugins used by ServiceControl
reviewed: 2017-03-29
related:
- samples/servicecontrol/monitoring3rdparty
---

ServiceControl is the backend service for ServiceInsight, ServicePulse, and third-party integration. It collects and stores information from monitored NServiceBus endpoints and exposes this information for consumption by various clients via a HTTP API.

NOTE: When ServiceControl is introduced into an existing environment the standard behavior of error and audit queues will change. When ServiceControl is not monitoring the environment failed messages will remain in the configured error queue and audit messages in the configured audit queue, as soon as ServiceControl is installed and configured messages, in both queues, will be imported by ServiceControl.

Plugins collect information from NServiceBus and are deployed together with each NServiceBus endpoint. These plugins are optional from the perspective of the NServiceBus framework itself (they are not required by the endpoint), but they are required in order to collect the information that enables ServiceControl (and its clients) to provide the relevant functionality for each plugin.

ServiceControl provides the monitoring capability by analyzing the configured error and audit queues. It can extract information like endpoint name, queue name and, in case of error messages, the exception stack trace, etc. This information is stored in a built-in internal database.


## Configuring an endpoint to be monitored by ServiceControl

To allow ServiceControl to monitor endpoints:

 1. ServiceControl should be [installed](/servicecontrol/installation.md) and at least one instance should be configured using the same transport as that of the endpoints that are being monitored.
 2. For every endpoint that is being monitored by ServiceControl, [configure the endpoint for auditing](/nservicebus/operations/auditing.md#configuring-auditing), and make sure that the audit queue is the same as the audit queue that ServiceControl is configured with.
   
   ```mermaid
   graph LR
 	
   EndpointA --> AuditQ 
   EndpointB --> AuditQ
   EndpointC --> AuditQ
 
   AuditQ[audit] --> ServiceControl 
  	
   ServiceControl .-> AuditLog[audit.log]
   ```
 3. For every endpoint that is being monitored by ServiceControl, [configure recoverability](/nservicebus/recoverability/), and make sure that the error queue is the same as the error queue that ServiceControl is configured with.
   
   ```mermaid
   graph LR
 	
   EndpointA --> ErrorQ 
   EndpointB --> ErrorQ
   EndpointC --> ErrorQ
   	
   ErrorQ[error] --> ServiceControl 
   	
   ServiceControl .-> ErrorLog[error.log]
   ```


## Installing and Deploying Plugins

The ServiceControl plugins are deployed with the endpoints they are monitoring. During development, add the relevant plugin NuGet package to the endpoint's project in Visual Studio using the NuGet.

**Related articles**

 * [How to configure endpoints for monitoring by ServicePulse](/servicepulse/how-to-configure-endpoints-for-monitoring.md)

ServiceControl offers the following endpoint plugins:

 * [Heartbeat Plugin](heartbeat.md) - Enables endpoint health monitoring. It sends heartbeat messages at a regular interval from the endpoint to ServiceControl to help determine whether the endpoint is active. This information is shown in ServicePulse.
 
  ```mermaid
  graph LR
 	
  subgraph Endpoint
    Heartbeats
  end
 	
  Heartbeats -- Heartbeat<br>Data --> SCQ
	
  SCQ[ServiceControl<br>Input Queue] --> SC[ServiceControl]
  ```

 * [Saga Audit Plugin](saga-audit.md) - Enables the Saga visualization capabilities useful for debugging during development. The Saga message behavior and the saga state changes as the Saga is being processed are sent to ServiceControl. This information is shows in ServiceInsight. This plugin is only for development purposes and should not be used in production.
 
  ```mermaid
  graph LR
  subgraph Endpoint
    Auditing
    SagaAudit[Saga Audit]
  end
 	
  SagaAudit -- Saga Change<br>Audit Data --> SCQ[ServiceControl<br>Input Queue]
 	
  Auditing -- Message<br>Audit Data --> AuditQ[audit<br>queue]
 
  AuditQ --> ServiceControl
 	
  SCQ --> ServiceControl
  ````

 * [Custom Check Plugin](custom-checks.md) - Enables custom monitoring abilities for endpoints by allowing the developer to define a set of conditions that needs to be checked. This plugin will report the results of these custom checks to ServiceControl.

  ```mermaid
  graph LR
	
  subgraph Endpoint
    CustomChecks[Custom Checks]
  end
 	
  CustomChecks -- Custom Check<br>Data --> SCQ[ServiceControl<br>Input Queue]
	
  SCQ --> ServiceControl
  ```
