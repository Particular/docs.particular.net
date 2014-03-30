---
title: ServiceControl Endpoint Plugins
summary: 'Reviews the Endpoint Plugins used by ServiceControl'
tags:
- ServiceControl 
---

## Introduction

ServiceControl is the activity information backend service for ServiceInsight, ServicePulse and 3rd party developments. It collects information from monitored NServicBus endpoints, stores this information in a dedicated database, and exposes this information for consumption by various clients via HTTP API.

The process of collecting information from NServiceBus is performed by using plugins that can be deployed with each NServiceBus endpoint. 
These plugins are optional from the perspective of the NServiceBus framework itself (they are not required by the endpoint), but they are required in order to collect that information that will enable ServiceControl (and its clients) to provide the relevant functionality for each plugin.


## Getting the plugins

The ServiceControl plugins are available for installation and download from the Nuget gallery. 

| **Plugin** | **Nuget gallery URL** | 
|:----- |:----- |
|Heartbeat|http://www.nuget.org/packages/ServiceControl.Plugin.Heartbeat|
|Custom Checks|http://www.nuget.org/packages/ServiceControl.Plugin.CustomChecks|
|Saga Audit|http://www.nuget.org/packages/ServiceControl.Plugin.SagaAudit|
|Debug Session|http://www.nuget.org/packages/ServiceControl.Plugin.DebugSession|


**NOTE:** For NServiceBus version dependency requirements for each plugin, review the "Dependencies" section in the Nuget Gallery page for the specific plugin.  

## Installation and Deployment

The ServiceControl plugins are deployed with the endpoints the are are monitoring, and you can add a plugin to an endpoint during development, testing or production: 
 
* During development, add the relevant plugin nuget package to the endpoint's project in Visual Studio using Nuget Package Manager Console
   * See "[Finding and Installing a NuGet Package Using the Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)"
   * The plugin specific nuget commands are displayed in the relevant nuget package page in the Nuget Gallery    
* When in production, add the plugin DLL's to the bin directory of the endpoint, and restart the endpoint process for the changes to take affect and the plugin to be loaded.   

#### Related articles:

- [How to configure endpoints for monitoring by ServicePulse](http://docs.particular.net/ServicePulse/how-to-configure-endpoints-for-monitoring)

## Connecting the plugins to ServiceControl

Once deployed on an active endpoint, the endpoints send plugin specific information to ServiceControl, with no need for additional configuration. 

This is done by sending messages using the defined endpoint transport) to the ServiceControl queue which is located in the same physical location as the Audit and Error queues defined for the endpoint.

The ServiceControl queue (and all other ServiceControl related sub-queues) are created during the installation phase of ServiceControl.  

**NOTE:** Audit and Error queues must be defined for each endpoint monitored by ServiceControl


## Plugin functionality and behavior

### ServiceControl.Plugin.Heartbeat

The heartbeat plugin sends heartbeats messages from the endpoint to the ServiceControl queue. These messages are send every 30 seconds (non-configurable).

An endpoint that is marked for monitoring (by ServicePulse) will be expected to send a heartbeat message every 30 seconds. As long as a monitored endpoint sends heartbeat messages, it is marked as "active". Marking an endpoint as active means it is able to properly and periodically send messages using the endpoint-defined transport. 

Note that even if an endpoint is able to send heartbeat messages and it is marked as "active", other failures may occur within the endpoint and it host that may inhibit it from performing as expected. For example, the endpoint may not be able to process incoming messages, or it may be able to send messages to the ServiceControl queue, but not to another queue. To monitor and alert on such cases, develop a Custom Check using the CustomChecks plugin.    

If a heartbeat message is not received by ServiceControl from an endpoint, that endpoint is marked as "inactive". 
An inactive endpoint indicates that there is a failure in the communication path between ServiceControl and the monitored endpoint. For example, such failures may be caused by a failure of the endpoint itself, a communication failure in the transport, or when ServiceControl is unable to receive and process the heartbeat messages sent by the endpoint.

### ServiceControl.Plugin.CustomChecks

The custom checks plugin allows the developer of an NServiceBus endpoint to define a set of conditions that will be checked on endpoint start or periodically.

These conditions are solution and/or endpoint specific and is recommended that they include the set of explicit (and implicit) assumptions about what will enable the endpoint to function as expected vs. what will make the endpoint fail.

For example, custom checks can include checking that a 3rd party service provider is accessible from the endpoint host, verifying that resources required by the endpoint are above a defined minimum threshold and more.

As mentioned above, there are two types of custom checks:

* Custom check that runs once on endpoint host start up
* Periodic check, that runs at defined intervals
 
The result of a custom check is either successful or a failure (with a detailed description defined by the developer). This result is sent as a message to the ServiceControl queue.   

#### Related articles

- [How to Develop Custom Checks for ServicePulse](http://docs.particular.net/ServicePulse/how-to-develop-custom-checks)

### ServiceControl.Plugin.SagaAudit

The Saga Audit plugin collects Saga runtime activity information. This information enables the display of detailed Saga data, behavior and current status in the ServiceInsight Saga View.

The Saga Audit plugin sends the relevant saga state information to the ServiceControl queue whenever a Saga state changes. This enables the Saga View to be up-to-date and extrremely detailed.

However, and depending on the Sagas update frequency, it may also result is a large number of messages, and a higher load on both the sending endpoint and on the receiving ServiceControl instance. 

As a result, prior to deploying the Saga Audit plugin, you should test and verify that the Saga Audit plugin communication overhead does not interfere with your expected endpoint performance.   


#### Related articles

* [ServiceInsight Overview - The Saga View](http://docs.particular.net/ServiceInsight/getting-started-overview#the-saga-view)

### ServiceControl.Plugin.DebugSession

Debug session is a dedicated plugin that enables the integration between ServiceMatrix and ServiceInsight.

When deployed, the debug session plugin adds a specified debug session identifier to the header of each message send by the endpoint.

This allows messages sent by a debugging or test run within Visual Studio to be correlated, filtered and highlighted within ServiceInsight.

#### Related articles

* [ServiceMatrix and ServiceInsight Interaction](http://docs.particular.net/ServiceMatrix/servicematrix-serviceinsight)
  
