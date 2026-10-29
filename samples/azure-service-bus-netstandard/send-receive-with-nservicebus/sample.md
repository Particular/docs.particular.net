---
title: Build message-driven business applications with NServiceBus
summary: Build message-driven business applications with NServiceBus and Azure Service Bus
reviewed: 2026-10-28
component: ASBS
related:
- transports/azure-service-bus
---

The companion code for [this article](https://learn.microsoft.com/en-us/azure/service-bus-messaging/build-message-driven-apps-nservicebus) on using NServiceBus with Azure Service Bus. Refer to the linked article for a full walkthrough of the code.

## Running the sample

Both endpoints read the Azure Service Bus connection string from the `ConnectionStrings:AzureServiceBusConnectionString` setting. Set it in each project's `appsettings.json`, or set the `ConnectionStrings__AzureServiceBusConnectionString` environment variable to keep the connection string out of source files.
