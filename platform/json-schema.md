---
title: ServicePlatform connection schema
summary: The JSON schema used by the ServicePlatform Connector package
reviewed: 2024-09-26
component: PlatformConnector
versions: 'PlatformConnector:*'
related:
  - platform/connecting
---

The [ServicePlatform Connector package](connecting.md) can parse a JSON file containing connection details with the following schema:

- [`ErrorQueue`](#errorqueue)
- [`Heartbeats`](#heartbeats)
- [`CustomChecks`](#customchecks)
- [`MessageAudit`](#messageaudit)
- [`SagaAudit`](#sagaaudit)
- [`Metrics`](#metrics)

## `ErrorQueue`

The [transport queue to send failed messages to](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address-using-code).<br/>
_Type_: string<br/>
_Required_: No

### Example

```json
{
    "ErrorQueue": "errorQueue"
}
```

## `Heartbeats`

Configuration options for the [Heartbeats](/monitoring/heartbeats/) feature.<br/>
_Type_: object<br/>
_Required_: [No](#notes)

### Example

```json
{
  "Heartbeats": {
    "Enabled": true,
    "HeartbeatsQueue": "heartbeatsQueue",
    "Frequency": "00:00:30",
    "TimeToLive": "00:02:00"
  }
}
```

### Properties

| Name            | Type      | Required | Description                                                                    |
|-----------------|-----------|----------|--------------------------------------------------------------------------------|
| Enabled         | boolean   | [No](#notes)      | If true, the endpoint will send heartbeats to the Particular Service Platform |
| HeartbeatsQueue | string    | [Yes](#notes)      | The transport queue to send Heartbeat messages to                             |
| Frequency       | [timespan](#notes)   | No       | The frequency to send Heartbeat messages                                      |
| TimeToLive      | [timespan](#notes)   | No       | The maximum time to live for Heartbeat messages                               |

## `CustomChecks`

Configuration options for the [Custom Checks](/monitoring/custom-checks/install-plugin.md) feature.<br/>
_Type_: object<br/>
_Required_: [No](#notes)

### Example

```json
{
  "CustomChecks": {
    "Enabled": true,
    "CustomChecksQueue": "customChecksQueue",
    "TimeToLive": "00:02:00"
  }
}
```

### Properties

| Name              | Type      | Required | Description                                                                              |
|-------------------|-----------|----------|------------------------------------------------------------------------------------------|
| Enabled           | boolean   | [No](#notes)      | If true, the endpoint will send custom check results to the Particular Service Platform |
| CustomChecksQueue | string    | [Yes](#notes)      | The transport queue to send Custom Checks messages to                                   |
| TimeToLive        | [timespan](#notes)   | No       | The maximum time to live for Custom Checks messages                                     |

## `MessageAudit`

Configuration options for the [Message Auditing](/nservicebus/operations/auditing.md) feature.<br/>
_Type_: object<br/>
_Required_: [No](#notes)

### Example

```json
{
  "MessageAudit": {
    "Enabled": true,
    "AuditQueue": "auditQueue"
  }
}
```

### Properties

| Name       | Type      | Required | Description                                                                                          |
|------------|-----------|----------|------------------------------------------------------------------------------------------------------|
| Enabled    | boolean   | [No](#notes)      | If true, the endpoint will send a copy of each message processed to the Particular Service Platform |
| AuditQueue | string    | [Yes](#notes)      | The transport queue to send Audit message to                                                        |

## `SagaAudit`

Configuration options for the [Saga Auditing](/nservicebus/sagas/saga-audit.md) feature.<br/>
_Type_: object<br/>
_Required_: [No](#notes)

### Example

```json
{
  "SagaAudit": {
    "Enabled": true,
    "SagaAuditQueue": "sagaAuditQueue"
  }
}
```

### Properties

| Name              | Type      | Required | Description                                                                           |
|-------------------|-----------|----------|---------------------------------------------------------------------------------------|
| Enabled           | boolean   | [No](#notes)      | If true, the endpoint will audit saga invocations to the Particular Service Platform |
| SagaAuditQueue    | string    | [Yes](#notes)      | The transport queue to send Saga Audit messages to                                   |

## `Metrics`

Configuration options for the [Metrics](/monitoring/metrics/) feature.<br/>
_Type_: object<br/>
_Required_: [No](#notes)

### Example

```json
{
  "Metrics": {
    "Enabled": true,
    "MetricsQueue": "metricsQueue",
    "Interval": "00:01:00",
    "InstanceId": "uniqueInstanceId",
    "TimeToLive": "00:02:00"
  }
}
```

### Properties

| Name         | Type      | Required | Description                                                                                |
|--------------|-----------|----------|--------------------------------------------------------------------------------------------|
| Enabled      | boolean   | [No](#notes)      | If true, the endpoint will send metric data to the Particular Service Platform.            |
| MetricsQueue | string    | [Yes](#notes)      | The transport queue to send Metrics messages to                                           |
| Interval     | [timespan](#notes)   | Yes      | The longest interval allowed between Metrics messages                                     |
| InstanceId   | string    | No       | Unique, human-readable, stable between restarts, identifier for running endpoint instance |
| TimeToLive   | [timespan](#notes)  | No       | The maximum time to live for Metrics messages                                             |

## Full example

```json
{
  "ErrorQueue": "errorQueue",
  "Heartbeats": {
    "Enabled": true,
    "HeartbeatsQueue": "heartbeatsQueue",
    "Frequency": "00:00:30",
    "TimeToLive": "00:02:00"
  },
  "CustomChecks": {
    "Enabled": true,
    "CustomChecksQueue": "customChecksQueue",
    "TimeToLive": "00:02:00"
  },
  "MessageAudit": {
    "Enabled": true,
    "AuditQueue": "auditQueue"
  },
  "SagaAudit": {
    "Enabled": true,
    "SagaAuditQueue": "sagaAuditQueue"
  },
  "Metrics": {
    "Enabled": true,
    "MetricsQueue": "metricsQueue",
    "Interval": "00:01:00",
    "InstanceId": "uniqueInstanceId",
    "TimeToLive": "00:02:00"
  }
}
```

## Notes

- If a section is omitted or does not contain an `Enabled` property then the feature is not configured
- TimeSpan properties are encoded as strings in `HH:MM:SS` format
- Required properties are checked only if the feature is enabled
