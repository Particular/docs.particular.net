---
title: Usage Reporting Setup
summary: Viewing endpoint usage summary and generating a usage report
component: ServicePulse
reviewed: 2024-05-08
related:
  - nservicebus/usage
---

Specific settings for collecting usage data to generate a usage report.

> [!NOTE]
> The usage data collection functionality requires ServicePulse version 1.40 or later, and ServiceControl version 5.4 or later.

## Connection setup

In most scenarios existing ServiceControl error instance connection settings will be used to establish a connection to the broker.

![usage-setup-connections](images/usage-setup-connection.png "width=600")

If there is a connection problem, specific usage settings can be provided as environment variables or directly in the [ServiceControl.exe.config](/servicecontrol/servicecontrol-instances/configuration.md) file.
The Usage Setup tab provides easy copy/paste functionality to obtain the required settings in the correct format, based on configuration type.

Look at the [Diagnostics](#diagnostics) tab to diagnose connection issues.

### Azure Service Bus

#### Setup using Azure Portal

1. Create an application Id for ServiceControl
2. Assign the role `Monitoring Reader` to this application Id
3. Update ServiceControl configuration and set at minimum `TenantId`, `SubscriptionId`, `ClientId` (aka ApplicationId) and its accompanying `ClientSecret`


#### Setup using Azure CLI

```ps1
# set correct context and subscription
az account set --subscription "YourAzureSubscriptionName"
Set-AzContext -Subscription "YourAzureSubscriptionName"

# Application ID: az ad app create --display-name ServiceControlUsageReporting
$applicationId = "817c7efc-0c93-473a-99b9-1612d32b8678"

# Azure Service Bus subscription ID: az servicebus namespace list
$subscriptionId = "934f5a76-bd9e-4d9a-be26-94b1476bab33"

# Resource group name: az group list
$resourceGroupName = "ramon.smits"
# Assign role to resource group

$scope = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName"
# or to specific resource in resource group 
#$scope = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName/providers/Microsoft.ServiceBus/namespaces/$namespaceName

New-AzRoleAssignment -ApplicationId $applicationId -RoleDefinitionName "Monitoring Reader" -Scope $scope
```

#### Settings

Refer to the [Usage Reporting when using the Azure Service Bus transport](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-the-azure-service-bus-transport) section of the ServiceControl config file for an explanation of the Azure Service Bus-specific settings.

#### Minimum Permissions

The built-in role [`Monitoring Reader`](https://learn.microsoft.com/en-us/azure/azure-monitor/roles-permissions-security#monitoring-reader) is sufficient to access the required Azure Service Bus metrics.

To lock it down even further to the minimally required permissions, create a custom role with the following permissions:

```json
{
    "properties": {
        "roleName": "myrolename",
        "description": "",
        "assignableScopes": [
            "/subscriptions/xxxxxxxxxxxxxxxxxxxxx"
        ],
        "permissions": [
            {
                "actions": [
                    "Microsoft.ServiceBus/namespaces/read",
                    "Microsoft.ServiceBus/namespaces/providers/Microsoft.Insights/metricDefinitions/read",
                    "Microsoft.ServiceBus/namespaces/queues/read",
                    "Microsoft.Resources/subscriptions/read",
                    "Microsoft.Resources/subscriptions/resources/read"
                ],
                "notActions": [],
                "dataActions": [],
                "notDataActions": []
            }
        ]
    }
}
```


The `Microsoft.ServiceBus` permissions are required to read queue names and metric data from Azure Monitor. The `Microsoft.Resources/subscriptions` permissions are required in order to locate the Service Bus namespace within the Azure subscription.

### Amazon SQS

#### Settings

Refer to the [Usage Reporting when using the Amazon SQS transport](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-the-amazon-sqs-transport) section of the ServiceControl config file for an explanation of the Amazon SQS-specific settings.

#### Minimum Permissions

```json
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

Refer to the [Usage Reporting when using the SqlServer transport](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-the-sqlserver-transport) section of the ServiceControl config file for an explanation of the SQL Server-specific settings.

#### Minimum Permissions

User with rights to query [INFORMATION_SCHEMA].[COLUMNS] table.

### PostgreSQL

#### Settings

Refer to the [Usage Reporting when using the PostgreSQL transport](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-the-postgresql-transport) section of the ServiceControl config file for an explanation of the PostgreSQL Server-specific settings.

#### Minimum Permissions

User with rights to query [INFORMATION_SCHEMA].[COLUMNS] table.

### RabbitMQ

#### Settings

Refer to the [Usage Reporting when using the RabbitMQ transport](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-the-rabbitmq-transport) section of the ServiceControl config file for an explanation of the RabbitMQ-specific settings.

#### Minimum permissions

User with monitoring tag and read permission.

### MSMQ & Azure Storage Queues

MSMQ and Azure Storage Queues do not support querying of metrics. To enable the automatic usage reporting functionality for these systems, auditing and/or monitoring must be setup:

- Auditing
  - install the [Audit](./../servicecontrol/audit-instances) instance
  - configure [auditing](./../nservicebus/operations/auditing.md) on all NServiceBus endpoints
- Monitoring
  - install the [Monitoring](./../monitoring) instance
  - configure [metrics](./../monitoring/metrics) on all NServiceBus endpoints

## Diagnostics

The Diagnostics tab helps to diagnose any connection issues to the broker, as well as the audit and monitoring instances.

![usage-setup-diagnostics](images/usage-setup-diagnostics.png "width=600")

After making any setting changes, press the `Refresh Connection Test` button to see if the problem is resolved.
If unable to fix the issue, open a [non-critical support case](https://particular.net/support) and include the diagnostic output.

## Report masks

Information that is considered sensitive can be obfuscated in the usage report.
All words to be redacted can be specified in the `Mask Report Data` tab. Specify one word per line.

![usage-setup-masks](images/usage-setup-masks.png "width=600")
