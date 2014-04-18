---
title: Heartbeat and Custom Checks support for NServiceBus v3
summary: Details how to setup a custom solution su support heartbeats and custom checks for endpoints that still targets NServiceBus v3
tags:
- ServiceControl
- NServiceBus v3
- Heartbeat
- Custom Checks
---

ServiceControl supports out-of-the-box NServiceBus v4 (and above) through [heartbeats and custom checks](/ServicePulse/how-to-configure-endpoints-for-monitoring).

It possible to add support to ServiceControl also for endpoints that targets NServiceBus v3 by developing custom code that hooks into the NServiceBus message handling pipeling enriching messages, as expected by ServiceControl, and introducing the communication channel between the endpoint itself and ServiceControl.

In the ServiceControl github repository there is a [dedicated branch](https://github.com/Particular/ServiceControl/tree/ServiceControl.Plugin.V3) containing all the code required to setup custom support for endpoints running NServiceBus v3.

### Heartbeats

Heartbeats management is done in three steps:

* *Enrich messages*: each message that flow through the endpoint must be enriched with some additional information required by ServiceControl;
* *Startup registration message*: ServiceControl expects that each endpoint at startup time sends a dedicated startup message;
* *Heartbeat message*: an endpoint is expected to send a heartbeat message at predefined interval to signal to ServiceControl its status;

#### Enrich messages

#### Startup registration message

#### Heartbeat message

### Custom checks

**NOTE** Heartbeats and Custom Checks are not officially supported. The above mentioned code samples are provided as is to illustrate how to let a NServiceBus v3 based endpoint to communicate with ServiceControl.