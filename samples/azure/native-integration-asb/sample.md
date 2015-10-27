---
title: Native Integration with Azure Service Bus Transport
summary: Shows how to consume messages published by non NServiceBus endpoints
tags:
related:
- nservicebus/azure/azure-servicebus-transport
---

## Prerequisites 

An environment variable named `AzureServiceBus.ConnectionString` that contains the connection string for the Azure Service Bus namespace.


## Azure Service Bus Transport

This sample utilizes the [Azure Service Bus Transport](/nservicebus/azure/azure-servicebus-transport.md).


## Code walk-through

TBD

### TBD

Things to note:

 * The use of the `AzureServiceBus.ConnectionString` environment variable mention above.
 * The use of `UseSingleBrokerQueue` prevents the Azure transport individualizing queue names by appending the machine name.  