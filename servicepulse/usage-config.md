---
title: Usage Setup
summary: Viewing endpoint usage summary and generating a usage report
component: ServicePulse
reviewed: 2024-05-08
related:
---

Specific settings for collecting usage data to generate a usage report.

> [!NOTE]
> The usage data collection functionality requires ServicePulse version 1.39 or later, and ServiceControl version 5.3 or later.

## Connection setup

In most scenarios existing ServiceControl Error instance connection settings will be used to establish a connection to the broker.
If there is a connection problem, specific usage settings can be provided via the Usage tab under Configuration.

Look at the [Diagnostics](#diagnostics) tab to diagnose connection issues.

### Azure Service Bus

#### Settings

The follow settings are available to setup a connection to Azure Service Bus:

- LicenseComponent/ASB/ServiceBusName
- LicenseComponent/ASB/ClientId
- LicenseComponent/ASB/ClientSecret
- LicenseComponent/ASB/TenantId
- LicenseComponent/ASB/SubscriptionId
- LicenseComponent/ASB/ManagementUrl

Refer to the [usage reporting](/servicecontrol/creating-config-file.md#usage-reporting) section of the ServiceControl config file for an explanation of the Azure Service Bus specific settings.

#### Permissions

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

#### Settings

The follow settings are available to setup a connection to Amazon SQS:

- LicenseComponent/AmazonSQS/AccessKey
- LicenseComponent/AmazonSQS/SecretKey
- LicenseComponent/AmazonSQS/Profile
- LicenseComponent/AmazonSQS/Region
- LicenseComponent/AmazonSQS/Prefix

Refer to the [usage reporting](/servicecontrol/creating-config-file.md#usage-reporting) section of the ServiceControl config file for an explanation of the Amazon SQS specific settings.

#### Permissions

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

#### Settings

The follow settings are available to setup a connection to SqlServer:

- LicenseComponent/SqlServer/ConnectionString
- LicenseComponent/SqlServer/AdditionalCatalogs

Refer to the [usage reporting](/servicecontrol/creating-config-file.md#usage-reporting) section of the ServiceControl config file for an explanation of the SqlServer specific settings.

#### Permissions

User with rights to query [INFORMATION_SCHEMA].[COLUMNS] table.

### RabbitMQ

#### Settings

The follow settings are available to setup a connection to RabbitMQ:

- LicenseComponent/RabbitMQ/ApiUrl
- LicenseComponent/RabbitMQ/UserName
- LicenseComponent/RabbitMQ/Password

Refer to the [usage reporting](/servicecontrol/creating-config-file.md#usage-reporting) section of the ServiceControl config file for an explanation of the RabbitMQ specific settings.

#### Permissions

User with monitoring tag and read permission.

### MSMQ & Azure Storage Queues

MSMQ and Azure Storage Queues do not support quering of metrics. To be able to report on usage in these systems, [Audit](./../servicecontrol/audit-instances) and/or [Monitoring](./../monitoring) need to be enabled on all NServiceBus endpoints.

Configure [auditing](./../nservicebus/operations/auditing.md) on all NServiceBus endpoints.

Configure [monitoring](./../monitoring/metrics) on all NServiceBus endpoints.

## Diagnostics

The Diagnostics tab helps to diagnose any connection issues to the broker, as well as the Audit and Monitoring instances.

## Report Masks

Information that can be considered sensitive can be obfuscated in the usage report.
All words to be retracted can be specified in the Masks tab - one word per line.