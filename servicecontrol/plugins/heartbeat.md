---
title: Heartbeat Plugin
summary: Use the Heartbeat plugin to monitor the health of the endpoints
reviewed: 2017-07-12
component: Heartbeats
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

NOTE: It is essential to deploy this plugin to the endpoint in production for ServicePulse to be able to monitor the endpoint.


### Deprecated NuGet

The first release of the Heartbeat plugin the NuGet packages was named **ServiceControl.Plugin.Heartbeat**. **This packages is now obsolete and should be remove**. Replace it with the appropriate plugin based on the NServiceBus version. The new NServiceBus version specific packages are:

| NServiceBus Version | Nuget Package                        | 
|---------------------|--------------------------------------|
| 3.x                 | [ServiceControl.Plugin.Nsb3.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.Heartbeat/) |  
| 4.x                 | [ServiceControl.Plugin.Nsb4.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.Heartbeat/) |  
| 5.x                 | [ServiceControl.Plugin.Nsb5.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.Heartbeat/) |  
| 6.x                 | [ServiceControl.Plugin.Nsb6.Heartbeat](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb6.Heartbeat/) |  


## Configuration


partial: queue


### Heartbeat Interval

ServiceControl heartbeats are sent, by the plugin, at a predefined interval of 10 seconds. The interval value can be overridden on a per endpoint basis adding the following application setting to the endpoint configuration file:

snippet: heartbeatsIntervalConfig

Where the value is convertible to a `TimeSpan` value. The above sample is setting the endpoint heartbeat interval to 30 seconds.


partial: intervalCode


When configuring heartbeat interval, ensure ServiceControl setting [`HeartbeatGracePeriod`](/servicecontrol/creating-config-file.md#plugin-specific-servicecontrolheartbeatgraceperiod) is greater than the heartbeat interval.


### Time-To-Live (TTL)

When the plugin sends heartbeat messages, the default TTL is fixed to four times the configured value of the Heartbeat interval.

TTL is now configurable, as of Version 1.1.0

Add the app setting in app.config as shown to configure the TTL to a custom value instead of the default value based on heartbeat interval. Provide the timespan string for the value as shown. In this example, a heartbeat message will be sent every 30 seconds and the TTL for the heartbeat message is 3 minutes.

snippet: heartbeatsTtlConfig

Note: To enable the change the endpoint needs to be restarted.

partial: ttlCode


partial: disable


## Expired heartbeat messages

Heartbeat messages have a time to be received (TTBR) set based on the TTL value.  If ServiceControl does not consume the heartbeat messages before the TTBR expires then those messages may be discarded. Transports like MSMQ and ASB support DLQ and the expired heartbeat messages can be explicitly configured to be forwarded to the DLQ instead of being discarded. 


### MSMQ

Although NServiceBus configures the use of DLQ by default, messages that are defined with TTBR will not be automatically forwarded to the DLQ and will be discarded. Configuration can be specified to [override this behavior](/transports/msmq/dead-letter-queues.md#enabling-dlq-for-messages-with-ttbr) so that these messages can be forwarded to the DLQ.

WARNING: When using NServiceBus Versions 6.1 or below, messages will be forwarded to the DLQ even if TTBR is set on the messages.  To avoid this behavior, DLQ can be disabled by configuring the [MSMQ connection strings](/transports/msmq/connection-strings.md).  The heartbeat messages will be forwarded to the DLQ when ServiceControl is either stopped or very busy. In this case, the dead letter queue needs to be monitored and cleaned up.


### ASB

Forwarding messages with expired TTL to DLQ is a configuration that needs to be set on the destination queue which is the ServiceControl queue for the heartbeat messages. In order to forward the heartbeat messages after the TTBR expiration to DLQ, ServiceControl needs to be [explicitly configured](/transports/azure-service-bus/configuration/azureservicebusqueueconfig.md) by specifying `EnableDeadLetteringOnMessageExpiration`.
