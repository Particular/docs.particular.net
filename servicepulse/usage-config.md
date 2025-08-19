---
title: Usage Reporting Setup
summary: Viewing endpoint usage summary and generating a usage report
component: ServicePulse
reviewed: 2024-05-08
related:
  - servicepulse/usage
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

Steps:

1. Create an **ApplicationId (aka ClientId)** for ServiceControl
2. Assign it the **Monitoring Reader** role
3. Configure for the ServiceControl instance at minimum:
    - `TenantId`
    - `SubscriptionId`
    - `ClientId`
    - `ClientSecret`


#### Using Azure Portal

Instructions when using the Azure Portal, alternative is using the Azure CLI listed below.


1. Create App
    - Native to: **Home > App registrations**
    - Select **➕ New registration**
2. Assign application to role:
    - Natigate to: **Home > Service Bus > {service bus namespace} > Access control (IAM)**
    - Select: **➕ Add**
    - Enter:
      - Role: `Monitoring Reader`
      - Members: Select **➕ Select Members > {application name}**
    - Select: **Review and Assign**

#### Using Azure CLI

Instructions when using the Azure CLI or scripting, alternative is using the Azure Portal above.

```ps1
# Set context first
az account set --subscription "YourAzureSubscriptionName"

# Create ApplicationId (ClientId)
az ad app create --display-name ServiceControlUsageReporting

# Store your ApplicationId (ClientId)
$applicationId = "<Your Application ID>"

# List subscription ID
az servicebus namespace list

# Store your Subscription ID
$subscriptionId = "<Your Subscription ID>"

# List resource group
az group list

# Store resource group name
$resourceGroupName = "<Your Resource Group Name>"

# Assign role to resource group
$scope = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName"

# or to specific resource in resource group
$scope = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroupName/providers/Microsoft.ServiceBus/namespaces/$namespaceName
# end alternative

# assign Monitoring Reader role to ApplicationId
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
