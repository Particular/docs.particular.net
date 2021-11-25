---
title: Connecting endpoints
summary: Connecting NServiceBus endpoints to the Particular Service Platform
reviewed: 2021-11-25
component: PlatformConnector
versions: 'PlatformConnector:*'
related:
  - platform/json-schema
---

The ServicePlatform Connector plugin uses a simple API to connect an NServiceBus endpoint to the Particular Service Platform and configure:

- An [error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address-using-code) to send failed mesages to
- [Message auditing](/nservicebus/operations/auditing.md)
- [Saga auditing](/nservicebus/sagas/saga-audit.md)
- The collection and aggregation of [performance metrics](/monitoring/metrics/)
- [Custom checks](/monitoring/custom-checks/install-plugin.md)
- [Heartbeats](/monitoring/heartbeats/)

## Json

The connection details can be parsed natively from json with a [specific schema](json-schema.md).

snippet: PlatformConnector-FromJson

The json file looks like this:

snippet: PlatformConnector-Json

This configuration can be retrieved from a running ServicePulse instance. 

![Screenshot of ServicePulse showing the configuration endpoint connection json file tab](connecting.servicepulse.png)

## Code first

The connection details can be constructed in code.

snippet: PlatformConnector-CodeFirst

## Combined

It is possible to load configuration from json and then override settings via code.

snippet: PlatformConnector-Combo