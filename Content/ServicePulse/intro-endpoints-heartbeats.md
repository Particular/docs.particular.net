---
title: ServicePulse Introduction: Endpoints and Heartbeats
summary: Describes how ServicePulse monitors endpoints activity and availability using heartbeat messages
tags:
- ServicePulse
- Introduction
- Heartbeats
---

The Endpoints indicator in the ServicePulse dashboard displays the current status of the monitored NServiceBus endpoints. Specifically, it indicates whether, based on the heartbeats received from each endpoint, the endpoint is active or inactive.

This article explains how this is done, and defines the meaning of each of the terms used in describing the status of an endpoint (terms like "active", "monitored" and "heartbeat").  

![ServicePulse Dashboard Indicators (all green)](images/indicators-green.JPG)

### Monitored vs. Un-Monitored Endpoints

An NServiceBus endpoint can be set for monitoring by ServicePulse or can remain un-monitored. See [How to configure endpoints for monitoring by ServicePulse](/ServicePulse/how-to-configure-endpoints-for-monitoring) for detailed instructions on preparing an endpoint for monitoring by ServicePulse.

It is recommended that you configure all your endpoints for monitoring by ServicePulse. Once configured, the endpoint will appear in the ServicePulse Configuration page.   

**TODO: config monitoring page**

In the ServicePulse Configuration page you can specify which endpoints you wish to monitor with ServicePulse by checking (or un-checking) the checkbox next to the endpoint's entry.  

This is configuration setting only. The endpoint itself will be unchanged, and will still be set for monitoring; the change affects only the way in which ServicePulse treats information received from the endpoint. 

Un-checking an endpoint means that it will be ignored by ServicePulse (for example, alerts and indications will not be displayed for the un-monitored endpoint). 

Re-checking the endpoint setting as "monitored" will not make any changes to the endpoint itself, and will only affect the ServicePulse behavior towards information received from the (now re-)monitored endpoint.
 
Un-monitoring an endpoint via the ServicePulse Configuration page is useful when you wish to temporarily or permanently ignore the behavior of an endpoint. For example, you may wish to shutdown the endpoint's host for maintenance, or possibly the host itself will be permanently deactivated (as when a VM is deactivated). In such cases, the endpoint's data remains available in ServicePulse (via the underlying ServiceControl database) but you may not wish to receive any notifications form ServicePulse related to this intentionally disabled endpoint.  
 
### Active vs. Inactive Endpoints

ServicePulse relies on heartbeat messages sent from the monitored endpoints to indicate whether an endpoint is active or inactive.

An endpoint that is marked for monitoring (by ServicePulse) will be expected to periodically send a heartbeat message. As long as a monitored endpoint sends heartbeat messages, it is marked as "active". Therefore, marking an endpoint as active means it is able to properly and periodically send messages using the endpoint-defined transport.

If a heartbeat message is not received by ServiceControl from an endpoint within the specified timespan, that endpoint is marked as "inactive". An inactive endpoint indicates that there is a failure in the communication path between ServiceControl and the monitored endpoint. For example, such failures may be caused by a failure of the endpoint itself, a communication failure in the transport, or when ServiceControl is unable to receive and process the heartbeat messages sent by the endpoint.

**NOTE:** Even if an endpoint is able to send heartbeat messages and it is marked as "active", other failures may occur within the endpoint and its host that may prevent it from performing as expected. For example, the endpoint may not be able to process incoming messages, or it may be able to send messages to the ServiceControl queue but not to another queue. To monitor and get alerts for such cases, develop a custom check using the CustomChecks plugin.

For more details on how heartbeats are sent by an endpoint, see [ServiceControl Heartbeat Plugin](/ServiceControl/Plugins#servicecontrol-plugin-heartbeat).

Only monitored endpoints are expected by ServicePulse to send heartbeat messages periodically. Un-monitored endpoints (i.e. endpoints marked in the ServicePulse configuration page as "un-monitored") may be sending heartbeats (depending on their specific configuration and whether the relevant plugins were installed for that endpoint) but these heartbeats messages are ignored by ServiceControl and ServicePulse as long as the endpoint is marked as an un-monitored endpoint in the ServicePulse configuration page.

![Sets of Endpoints: Monitored (active and inactive) vs. Un-monitored, ](images/endpoints-sets-monitored-active.jpg)     