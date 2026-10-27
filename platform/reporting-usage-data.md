---
title: Reporting usage data
summary: Configure NServiceBus endpoints to report usage information to the Particular Service Platform
reviewed: 2026-10-27
component: Core
versions: 'Core:[10.3,)'
---

The Particular Service Platform collects usage information about running endpoints from a variety of sources.

- **Broker**: For Azure Service Bus, Amazon SQS, SQL Server, PostgreSQL, and RabbitMQ transports the messaging infrastructure is queried directly
- **Audit**: When the installation includes a ServiceControl Audit instance, and endpoints are configured to audit processed messages
- **Monitoring**: When the installation includes a ServiceControl Monitoring instance, and endpoints are configured to report monitoring data

Endpoints running NServiceBus version 10.3 and above can be configured to track usage information and periodically report it directly to the Particular Service Platform. When information from this source is available, it is preferred over information from other sources.

To configure an NServiceBus endpoint to report usage information to the Particular Service Platform:

```csharp
endpointConfiguration.ReportUsageInformationToServiceControl();
```

> [!IMPORTANT]
>This requires a ServiceControl Error instance running version 6.22 or above to process usage information sent by endpoints.

By default, NServiceBus uses the default ServiceControl queue name: `Particular.ServiceControl`. If a different queue name is required:

```csharp
endpointConfiguration.ServiceControlErrorInstanceQueue("My.ServiceControl");
```

When enabled, this feature sends a message to ServiceControl every 10 minutes and when the endpoint is shut down.

These messages contain:

- A timestamp
- The name of the endpoint
- A count of successfully processed messages

ServiceControl aggregates this information for all endpoints and includes it in the usage report that is downloaded from ServicePulse at renewal time.

> [!IMPORTANT]
> Usage information is never sent directly to Particular. Endpoints send usage information to ServiceControl, and that data is included in the usage report which can be downloaded and reviewed before sending it to Particular.
