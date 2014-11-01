---
title: ServiceControl Endpoint Plugins
summary: 'Describes the purpose and internal behavior of the endpoint plugins used by ServiceControl.'
tags:
- ServiceControl 
---

ServiceControl is the activity information backend service for ServiceInsight, ServicePulse, and third-party developments. It collects information from monitored NServicBus endpoints, stores this information in a dedicated database, and exposes this information for consumption by various clients via HTTP API.

NOTE: When ServiceControl is introduced into an existing environment the standard behavior of error and audit queues will change. When ServiceControl is not monitoring the environment failed messages will remain in the configured error queue and audit messages in the configured audit queue, as soon as ServiceControl is installed and configured messages, in both queues, will be moved into the ServiceControl database.

Plugins collect information from NServiceBus and can be deployed with each NServiceBus endpoint. 
These plugins are optional from the perspective of the NServiceBus framework itself (they are not required by the endpoint), but they are required in order to collect the information that enables ServiceControl (and its clients) to provide the relevant functionality for each plugin.


## Getting the Plugins

The ServiceControl plugins can be downloaded and installed from the NuGet gallery. 

###For NserviceBus Version 5 

| **Plugin** | **NuGet gallery URL** | 
|:----- |:----- |
|Heartbeat|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.Heartbeat|
|Custom Checks|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.CustomChecks|
|Saga Audit|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.SagaAudit|
|Debug Session|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.DebugSession|


###For NserviceBus Version 4 

| **Plugin** | **NuGet gallery URL** | 
|:----- |:----- |
|Heartbeat|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.Heartbeat|
|Custom Checks|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.CustomChecks|
|Saga Audit|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.SagaAudit|
|Debug Session|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.DebugSession|


###For NserviceBus Version 3 

| **Plugin** | **NuGet gallery URL** | 
|:----- |:----- |
|Heartbeat|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.Heartbeat|
|Custom Checks|https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.CustomChecks|

## Installing and Deploying

The ServiceControl plugins are deployed with the endpoints they are monitoring. You can add a plugin to an endpoint during development, testing, or production: 
 
* During development, add the relevant plugin NuGet package to the endpoint's project in Visual Studio using the NuGet Package Manager Console:
   * See "[Finding and Installing a NuGet Package Using the Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)".
   * The plugin-specific NuGet commands are displayed in the relevant NuGet package page in the NuGet Gallery.    
* When in production, add the plugin DLLs to the BIN directory of the endpoint, and restart the endpoint process for the changes to take effect and the plugin to be loaded.   

NOTE: For NServiceBus version-dependent requirements for each plugin, review the "Dependencies" section in the NuGet Gallery page for the specific plugin.  
**Related articles**

- [How to configure endpoints for monitoring by ServicePulse](/ServicePulse/how-to-configure-endpoints-for-monitoring.md)

## Connecting the Plugins to ServiceControl

Once deployed on an active endpoint, the endpoints send plugin-specific information to ServiceControl, with no need for additional configuration. This is done by sending messages using the defined endpoint transport to the ServiceControl queue, which is located in the same physical location as the audit and error queues defined for the endpoint.

The ServiceControl queue (and all other ServiceControl related sub-queues) are created during the installation phase of ServiceControl.  

NOTE: Audit and error queues must be defined for each endpoint monitored by ServiceControl.

## Understanding Plugin Functionality and Behavior

### ServiceControl Heartbeat Plugin

The heartbeat plugin sends heartbeat messages from the endpoint to the ServiceControl queue. These messages are sent every 10 seconds (by default).

An endpoint that is marked for monitoring (by ServicePulse) will be expected to send a heartbeat message within the specified time interval. As long as a monitored endpoint sends heartbeat messages, it is marked as "active". Marking an endpoint as active means it is able to properly and periodically send messages using the endpoint-defined transport. 

Note that even if an endpoint is able to send heartbeat messages and it is marked as "active", other failures may occur within the endpoint and its host that may prevent it from performing as expected. For example, the endpoint may not be able to process incoming messages, or it may be able to send messages to the ServiceControl queue but not to another queue. To monitor and get alerts for such cases, develop a custom check using the CustomChecks plugin.    

If a heartbeat message is not received by ServiceControl from an endpoint, that endpoint is marked as "inactive". 
An inactive endpoint indicates that there is a failure in the communication path between ServiceControl and the monitored endpoint. For example, such failures may be caused by a failure of the endpoint itself, a communication failure in the transport, or when ServiceControl is unable to receive and process the heartbeat messages sent by the endpoint.

**Related articles**

- [Introduction to Endpoints and Heartbeats in ServicePulse](/ServicePulse/intro-endpoints-heartbeats.md)

### ServiceControl CustomChecks Plugin 

The custom checks plugin allows the developer of an NServiceBus endpoint to define a set of conditions that are checked on endpoint startup or periodically.

These conditions are solution and/or endpoint specific. It is recommended that they include the set of explicit (and implicit) assumptions about what enables the endpoint to function as expected versus what will make the endpoint fail.

For example, custom checks can include checking that a third-party service provider is accessible from the endpoint host, verifying that resources required by the endpoint are above a defined minimum threshold, and more.

As mentioned above, there are two types of custom checks:

* Custom check that runs once, when the endpoint host starts
* Periodic check that runs at defined intervals
 
The result of a custom check is either success or a failure (with a detailed description defined by the developer). This result is sent as a message to the ServiceControl queue.   

**Related articles**

- [How to Develop Custom Checks for ServicePulse](/ServicePulse/how-to-develop-custom-checks.md)

### ServiceControl SagaAudit Plugin

The Saga Audit plugin collects saga runtime activity information. This information enables the display of detailed saga data, behavior, and current status in the ServiceInsight Saga View. The plugin sends the relevant saga state information as messages to the ServiceControl queue whenever a saga state changes. This enables the Saga View to be highly detailed and up-to-date.

However, depending on the saga's update frequency, it may also result in a large number of messages, and a higher load on both the sending endpoint and on the receiving ServiceControl instance. As a result, prior to deploying the Saga Audit plugin, you should test to verify that the Saga Audit plugin communication overhead does not interfere with your expected endpoint performance.   


**Related articles**

* [ServiceInsight Overview - The Saga View](/ServiceInsight/getting-started-overview.md#the-saga-view)

### ServiceControl DebugSession Plugin

Debug session is a dedicated plugin that enables integration between ServiceMatrix and ServiceInsight.

When deployed, the debug session plugin adds a specified debug session identifier to the header of each message sent by the endpoint. This allows messages sent by a debugging or test run within Visual Studio to be correlated, filtered, and highlighted within ServiceInsight.

**Related articles**

* [ServiceMatrix and ServiceInsight Interaction](/ServiceMatrix/servicematrix-serviceinsight.md)
  
