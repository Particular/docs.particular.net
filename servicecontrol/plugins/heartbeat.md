---
title: Heartbeat Plugin
summary: 'Use the Heartbeat plugin to monitor the health of your endpoints.'
tags:
- ServiceControl
- Heartbeat
related:
- servicepulse/intro-endpoints-heartbeats
redirects:
- servicecontrol/heartbeat-configuration
---

The Heartbeat plugin enables endpoint health monitoring in ServicePulse. It sends heartbeat messages from the endpoint to the ServiceControl queue. These messages are sent every 10 seconds (by default).

An endpoint that is marked for monitoring (by ServicePulse) will be expected to send a heartbeat message within the specified time interval. As long as a monitored endpoint sends heartbeat messages, it is marked as "active". Marking an endpoint as active means it is able to properly and periodically send messages using the endpoint-defined transport.

Note that even if an endpoint is able to send heartbeat messages and it is marked as "active", other failures may occur within the endpoint and its host that may prevent it from performing as expected. For example, the endpoint may not be able to process incoming messages, or it may be able to send messages to the ServiceControl queue but not to another queue. To monitor and get alerts for such cases, develop a custom check using the CustomChecks plugin.

If a heartbeat message is not received by ServiceControl from an endpoint, that endpoint is marked as "inactive".

An inactive endpoint indicates that there is a failure in the communication path between ServiceControl and the monitored endpoint. For example, such failures may be caused by a failure of the endpoint itself, a communication failure in the transport, or when ServiceControl is unable to receive and process the heartbeat messages sent by the endpoint.

NOTE: It is essential that you deploy this plugin to your endpoint in production for ServicePulse to be able to monitor your endpoint.


## NuGets

 * NServiceBus Version 5.x: [ServiceControl.Plugin.Nsb5.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.Heartbeat)
 * NServiceBus Version 4.x: [ServiceControl.Plugin.Nsb4.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.Heartbeat)
 * NServiceBus Version 3.x: [ServiceControl.Plugin.Nsb3.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.Heartbeat)

WARNING: The Heartbeat plugin Version 2 for NServiceBus 5 is currently not supporting Send-Only endpoints.


### Deprecated NuGet

If you are using the older version of the plugin, namely **ServiceControl.Plugin.Heartbeat** please remove the package and replace it with the appropriate plugin based on your NServiceBus version. This package has been deprecated and unlisted.


## Configuration


### Heartbeat Interval

ServiceControl heartbeats are sent, by the plugin, at a predefined interval of 10 seconds. The interval value can be overridden on a per endpoint basis adding the following application setting to the endpoint configuration file:

<!-- import heartbeatsIntervalConfig -->

Where the value is convertible to a `TimeSpan` value. In the above sample you are setting the endpoint heartbeat interval to 40 seconds.

When configuring heartbeat interval, make sure Service Control setting [`HeartbeatGracePeriod`](/servicecontrol/creating-config-file.md#configuration-options-servicecontrol-heartbeatgraceperiod) is greater than the heartbeat interval.


### TTL
When the plugin sends heartbeat messages, the default TTL is fixed to four times the configured value of the Heartbeat interval. In some cases, this still caused the heartbeat message to end up in the Dead Letter Queue depending on the message load in the ServiceControl Queue.

TTL is now configurable, same as the heartbeat interval.
Add the app setting in app.config as shown for to configure the TTL to a custom value instead of the default value based on heartbeat interval. Provide the timespan string for the value as shown. In this example, a heartbeat message will be sent every 30 seconds and the TTL for the heartbeat message is 3 minutes.

<!-- import heartbeatsTtlConfig -->

Note: To enable the change the endpoint needs to be restarted.
