---
title: Configuring Usage Reporting
summary: Viewing endpoint usage summary and generating a usage report
component: ServicePulse
reviewed: 2024-05-08
related:
---

Specific settings for collecting usage data to generate a usage report.

> [!NOTE]
> Usage requires ServicePulse version 1.39 or later, and ServiceControl version 5.3 or later.

## Connection Settings

In most scenarios existing ServiceControl connection settings will be used to establish a connection to the broker.
If there is a connection problem, specific usage settings can be provided via the Usage tab under Configuration.

Look at the [Diagnostics](#diagnostics) tab to diagnose connection issues.

TODO: Need a section on each type of connection and an explanation of the parameters involved, with minimum access/permissions required on the broker

### Azure Service Bus

```
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "VisualEditor0",
            "Effect": "Allow",
            "Action": "cloudwatch:GetMetricStatistics",
            "Resource": "*"
        },
        {
            "Sid": "VisualEditor1",
            "Effect": "Allow",
            "Action": "sqs:ListQueues",
            "Resource": "*"
        }
    ]
}
```

### Amazon SQS

```
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "VisualEditor0",
            "Effect": "Allow",
            "Action": "cloudwatch:GetMetricStatistics",
            "Resource": "*"
        },
        {
            "Sid": "VisualEditor1",
            "Effect": "Allow",
            "Action": "sqs:ListQueues",
            "Resource": "*"
        }
    ]
}
```

### SQLServer

User with rights to query [INFORMATION_SCHEMA].[COLUMNS] table.

### RabbitMQ

User with monitoring tag and read permission.

### MSMQ & Azure Storage Queues

MSMQ and Azure Storage Queues do not support quering of metrics. To be able to report on usage in these systems, [Audit](./../servicecontrol/audit-instances) and/or [Monitoring](./../monitoring) need to be enabled on all NServiceBus endpoints.

configure [auditing](./../nservicebus/operations/auditing.md) on all NServiceBus endpoints.

configure [monitoring](./../monitoring/metrics) on all NServiceBus endpoints.

## Diagnostics

The Diagnostics tab helps to diagnose any connection issues to the broker, as well as the Audit and Monitoring instances.

## Report Masks

Information that can be considered sensitive can be obfuscated in the usage report.
All words to be retracted can be specified in the Masks tab - one word per line.
