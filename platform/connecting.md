---
title: Connecting endpoints
summary: Connecting NServiceBus endpoints to the Particular Service Platform
reviewed: 2021-11-25
component: PlatformConnector
versions: 'PlatformConnector:*'
related:
  - platform/json-schema
  - samples/platform-connector
---

The ServicePlatform Connector plugin provides a unified API to connect an NServiceBus endpoint to the Particular Service Platform by configuring:

- An [error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address-using-code) address
- [Message auditing](/nservicebus/operations/auditing.md)
- [Saga auditing](/nservicebus/sagas/saga-audit.md)
- [Performance metrics](/monitoring/metrics/) collection
- [Custom checks](/monitoring/custom-checks/install-plugin.md)
- [Heartbeats](/monitoring/heartbeats/)

## Json

The connection details can be parsed from json compliant with [the configuration schema](json-schema.md).

snippet: PlatformConnector-FromJson

The json file looks like this:

snippet: PlatformConnector-Json

A json file specific to a concrete deployment of the ServicePlatform is available via ServicePulse. 

![Screenshot of ServicePulse showing the configuration endpoint connection json file tab](connecting.servicepulse.png)

## Code first

The connection details can be constructed in code.

snippet: PlatformConnector-CodeFirst

## Combined

It is possible to load configuration from json and then override settings via code.

snippet: PlatformConnector-Combo