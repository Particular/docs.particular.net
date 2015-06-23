---
title: ServiceControl Endpoint Plugins
summary: 'Describes the purpose and internal behavior of the endpoint plugins used by ServiceControl.'
tags:
- ServiceControl 
related:
- samples/custom-checks/monitoring3rdparty
---

ServiceControl is the backend service for ServiceInsight, ServicePulse, and third-party integrations. It collects and stores information from monitored NServiceBus endpoints and exposes this information for consumption by various clients via a HTTP API.

NOTE: When ServiceControl is introduced into an existing environment the standard behavior of error and audit queues will change. When ServiceControl is not monitoring the environment failed messages will remain in the configured error queue and audit messages in the configured audit queue, as soon as ServiceControl is installed and configured messages, in both queues, will be imported by ServiceControl.

Plugins collect information from NServiceBus and are deployed together with each NServiceBus endpoint. 
These plugins are optional from the perspective of the NServiceBus framework itself (they are not required by the endpoint), but they are required in order to collect the information that enables ServiceControl (and its clients) to provide the relevant functionality for each plugin.


## Installing and Deploying

The ServiceControl plugins are deployed with the endpoints they are monitoring. You can add a plugin to an endpoint during development, testing, or production: 
 
* During development, add the relevant plugin NuGet package to the endpoint's project in Visual Studio using the NuGet.    
* When in production, add the plugin DLLs to the BIN directory of the endpoint, and restart the endpoint process for the changes to take effect and the plugin to be loaded.   

NOTE: For NServiceBus version-dependent requirements for each plugin, review the "Dependencies" section in the NuGet Gallery page for the specific plugin.  

**Related articles**

- [How to configure endpoints for monitoring by ServicePulse](/servicepulse/how-to-configure-endpoints-for-monitoring.md)


## Connecting to ServiceControl

Once deployed on an active endpoint, the endpoint sends plugin-specific information to ServiceControl. Plugins send messages using the defined endpoint transport to the ServiceControl queue. Location of ServiceControl queue is determined by the following:

1. **Endpoint`s configuration file**  
Check for an `appSetting` named `ServiceControl/Queue` e.g. `<add key="ServiceControl/Queue" value="particular.servicecontrol@machine"/>`.
1. **Convention based on the configured Error Queue machine**  
If an error queue is configured, for example `error@MachineName`, then the queue `Particular.ServiceControl@MachineName` will be used.
1. **Convention based on the configured Audit Queue machine**  
If an audit queue is configured, for example `audit@MachineName`, then the queue `Particular.ServiceControl@MachineName` will be used.

WARNING: Endpoint with plugins installed that cannot communicate to ServiceControl will shut down.

The ServiceControl queue (and all other ServiceControl related sub-queues) are created during the installation phase of ServiceControl.  

NOTE: If you're using MSMQ and have configured ServiceControl to import audit and error queues those queues most be present on the same machine as ServiceControl is installed on.


## Heartbeat Plugin

The Heartbeat plugin enables endpoint health monitoring in ServicePulse. It sends heartbeat messages from the endpoint to the ServiceControl queue. These messages are sent every 10 seconds (by default).

An endpoint that is marked for monitoring (by ServicePulse) will be expected to send a heartbeat message within the specified time interval. As long as a monitored endpoint sends heartbeat messages, it is marked as "active". Marking an endpoint as active means it is able to properly and periodically send messages using the endpoint-defined transport. 

Note that even if an endpoint is able to send heartbeat messages and it is marked as "active", other failures may occur within the endpoint and its host that may prevent it from performing as expected. For example, the endpoint may not be able to process incoming messages, or it may be able to send messages to the ServiceControl queue but not to another queue. To monitor and get alerts for such cases, develop a custom check using the CustomChecks plugin.    

If a heartbeat message is not received by ServiceControl from an endpoint, that endpoint is marked as "inactive". 

An inactive endpoint indicates that there is a failure in the communication path between ServiceControl and the monitored endpoint. For example, such failures may be caused by a failure of the endpoint itself, a communication failure in the transport, or when ServiceControl is unable to receive and process the heartbeat messages sent by the endpoint.

NOTE: It is essential that you deploy this plugin to your endpoint in production for ServicePulse to be able to monitor your endpoint.


### Nugets

 * NSB v5.x: [ServiceControl.Plugin.Nsb5.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.Heartbeat)
 * NSB v4.x: [ServiceControl.Plugin.Nsb4.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.Heartbeat)
 * NSB v3.x: [ServiceControl.Plugin.Nsb3.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.Heartbeat)


### Deprecated Nuget

If you are using the older version of the plugin, namely **ServiceControl.Plugin.Heartbeat** please remove the package and replace it with the appropriate plugin based on your NServiceBus version. This package has been deprecated and unlisted.

**Related articles**

- [Introduction to Endpoints and Heartbeats in ServicePulse](/servicepulse/intro-endpoints-heartbeats.md)


## CustomChecks Plugin

The CustomChecks Plugin enables custom endpoint monitoring. It allows the developer of an NServiceBus endpoint to define a set of conditions that are checked on endpoint startup or periodically.

These conditions are solution and/or endpoint specific. It is recommended that they include the set of explicit (and implicit) assumptions about what enables the endpoint to function as expected versus what will make the endpoint fail.

For example, custom checks can include checking that a third-party service provider is accessible from the endpoint host, verifying that resources required by the endpoint are above a defined minimum threshold, and more.

As mentioned above, there are two types of custom checks:

* Custom check that runs once, when the endpoint host starts
* Periodic check that runs at defined intervals
 
The result of a custom check is either success or a failure (with a detailed description defined by the developer). This result is sent as a message to the ServiceControl queue and status will be shown in the ServicePulse UI.   

**Related articles**

- [How to Develop Custom Checks for ServicePulse](/samples/custom-checks/monitoring3rdparty)

NOTE: It is essential that you deploy this plugin to your endpoint in production in order to receive error notifications about the custom check failures in the ServicePulse dashboard.


### Nugets

 * NSB v5.x: [ServiceControl.Plugin.Nsb5.CustomChecks](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.CustomChecks)
 * NSB v4.x: [ServiceControl.Plugin.Nsb4.CustomChecks](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.CustomChecks)
 * NSB v3.x: [ServiceControl.Plugin.Nsb3.CustomChecks](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.CustomChecks)


### Deprecated Nuget

If you are using the older version of the plugin, namely **ServiceControl.Plugin.CustomChecks** please remove the package and replace it with the appropriate plugin based on your NServiceBus version. This package has been deprecated and unlisted.


## SagaAudit Plugin

DANGER: **For Development only**. This plugin will result in a significant increase in the load placed on ServiceControl. As such it should not be used in production. 

The SagaAudit plugin enabled Saga Visualization in ServiceInsight. It is built specifically for developers to help debug Sagas by capturing every state change that the saga undergoes.  It is optimized for capturing and recording large amounts of data in regards to each saga message.  This information enables the display of detailed saga data, behavior, and current status in the ServiceInsight Saga View. The plugin sends the relevant saga state information as messages to the ServiceControl queue whenever a saga state changes. This enables the Saga View to be highly detailed and up-to-date.


### Implementation

The SagaAudit plugin captures the following information:

 * The incoming messages (including timeouts) that initiate change in the saga. 
 * The outgoing messages that the saga sends.
 * A snapshot of the current saga data.

All this information is sent to and stored in ServiceControl.

This results in an increase in load in several areas 

 1. Endpoint load in order to capture the required information
 2. Network load due to the extra information sent to ServiceControl 
 3. ServiceControl load in the areas of ingestion, correlation and data cleanup.  

The increase in load is proportional to size of the saga data multiplied by the number of messages the the saga receives. Since both these variables are dependent on the specific saga implementation it is not possible to give accurate predictions on the impact of this load in a production system.


### Nugets

 * NSB v5.x: [ServiceControl.Plugin.Nsb5.SagaAudit](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.SagaAudit)
 * NSB v4.x: [ServiceControl.Plugin.Nsb4.SagaAudit](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.SagaAudit)
 * NSB v3.x: Not Available


### Deprecated Nuget

If you are using the older version of the plugin, namely **ServiceControl.Plugin.SagaAudit** please remove the package and replace it with the appropriate plugin based on your NServiceBus version. This package has been deprecated and unlisted.


### Removing the plugin from Production

If you are currently running your endpoint with the SagaAudit plugin in Production, do the following to remove it:

1. Stop your endpoint
2. Delete the SagaAudit plugin dll from the endpoint's bin directory. (Either `ServiceControl.Plugin.Nsb5.SagaAudit.dll` or `ServiceControl.Plugin.Nsb4.SagaAudit.dll`). In addition to that, if you have automated deployment processes in place, please ensure that this dll is no longer included. 
3. Restart your endpoint

Doing so will stop sending the saga state change messages to ServiceControl reducing message load to ServiceControl. You can always turn it back on if or when needed. 


### Temporarily debugging sagas in Production

If you wish to visualize your saga in Production and the plugin is not already deployed, then add the Saga Audit plugin in the same location where your saga is running and restart your endpoint. Use ServiceInsight to view the visualization and when done, follow the steps above to remove the plugin and restart your service. This approach is very similar for example, when you need to use Journaling to debug a message based system, where you turn it on when needed to collect the information you wish to see and then remove it from production. 

**Related articles**

* [ServiceInsight Overview - The Saga View](/serviceinsight/getting-started-overview.md#the-saga-view)


## DebugSession Plugin 

DANGER: **For Development only**. Since this is meant only for use with Visual Studio it adds no value to deploy this plugin to production. 

The DebugSession plugin is used for for integrated debugging on a developers machine. The DebugSession is a dedicated plugin targeted for the developer that enables integration between ServiceMatrix and ServiceInsight. It helps to filter out older messages in ServiceInsight that are not part of the current debug session in Visual Studio.

When deployed, the debug session plugin adds a specified debug session identifier to the header of each message sent by the endpoint. This allows messages sent by a debugging or test run within Visual Studio to be correlated, filtered, and highlighted within ServiceInsight.

**Related articles**

* [ServiceMatrix and ServiceInsight Interaction](/servicematrix/servicematrix-serviceinsight.md)


### Nugets

 * NSB v5.x: [ServiceControl.Plugin.Nsb5.DebugSession](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.DebugSession)
 * NSB v4.x: [ServiceControl.Plugin.Nsb4.DebugSession](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.DebugSession)
 * NSB v3.x: Not Available 


### Deprecated Nuget

If you are using the older version of the plugin, namely **ServiceControl.Plugin.DebugSession** please remove the package and replace it with the appropriate plugin based on your NServiceBus version. This package has been deprecated and unlisted.
