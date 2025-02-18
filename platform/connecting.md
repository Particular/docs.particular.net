---
title: Connecting endpoints
summary: Connecting NServiceBus endpoints to the Particular Service Platform
reviewed: 2024-10-01
component: PlatformConnector
versions: 'PlatformConnector:*'
related:
  - platform/json-schema
  - samples/platform-connector
---

The ServicePlatform Connector plugin provides a unified API to connect an NServiceBus endpoint to the Particular Service Platform by configuring:

- an [error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address-using-code) address
- [message auditing](/nservicebus/operations/auditing.md)
- [saga auditing](/nservicebus/sagas/saga-audit.md)
- [performance metrics](/monitoring/metrics/) collection
- [custom checks](/monitoring/custom-checks/install-plugin.md)
- [heartbeats](/monitoring/heartbeats/)

## JSON

The connection details can be parsed from JSON-compliant text with [a specific configuration schema](json-schema.md).

snippet: PlatformConnector-FromJson

The JSON file looks like this:

snippet: PlatformConnector-Json

A JSON file specific to a concrete deployment of the ServicePlatform is available via ServicePulse.

![Screenshot of ServicePulse showing the configuration endpoint connection json file tab](connecting.servicepulse.png)

## Code first

The connection details can be constructed in code.

snippet: PlatformConnector-CodeFirst

## Combined

It is possible to load configuration from JSON and then override settings via code.

snippet: PlatformConnector-Combo