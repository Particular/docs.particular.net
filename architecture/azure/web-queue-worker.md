---
title: Web-queue-worker architecture style on Azure
summary: Gives a description of web-queue-worker including the components, challenges, and technology options for Azure
reviewed: 2025-07-15
callsToAction: ['solution-architect', 'ADSD']
---

The Azure Architecture Center describes the [web-queue-worker architecture style](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/web-queue-worker) as having core components that are a web front end that serves client requests, and a worker that performs resource-intensive tasks, long-running workflows, or batch jobs, with the web front end communicating with the worker through a message queue.

![Overview of Azure web-queue-worker style](azure-web-queue-worker.png)

## Components

* The **web front end** serves client requests, handles authentication and authorization.
* The front end uses a send-only NServiceBus endpoint to queue commands for intensive or long-running workloads in a **message queue**, such as Azure Service Bus or Azure Storage Queues, for the worker to consume. This allows the web application to remain independent of the worker's workload and to immediately respond to HTTP requests.
* **The worker**, hosted in services like Azure App Service, Azure Container Apps, Azure Functions, or Virtual Machines, runs a full NServiceBus endpoint that receives work from the message queue. The worker processes messages using business logic and stores results in a data store such as Azure SQL or Cosmos DB.
* Both worker and front end might access the **data store** directly to access business data and process state. An optional **cache**, such as Azure Cache for Redis, might be used for performance optimizations.
* A **Content Delivery Network**, like Azure CDN, can help to serve static content even faster and reduce load on the web front end.

## Challenges

This style is suitable for simple business domains. Without careful design, the front end and the worker can become complex, monolithic components that are difficult to maintain. Consider [event-driven](event-driven-architecture.md) and [microservices](microservices.md) architectural styles for more complex business domains.

## Technology choices

The web-queue-worker architecture style can make use of Azure's managed services like [Azure App Services](/architecture/azure/compute.md#platform-as-a-service-azure-app-services), [Azure Static Web Apps](https://azure.microsoft.com/en-us/products/app-service/static), [Azure Functions](/architecture/azure/compute.md#platform-as-a-service-azure-app-services), and [Cosmos DB](/architecture/azure/data-stores.md#azure-cosmos-db).

For containerized or [more flexible deployments](/architecture/azure/compute), [Azure Container Apps](https://learn.microsoft.com/en-us/azure/container-apps/) and [Azure Virtual Machines](/architecture/azure/compute.md#infrastructure-as-a-service) can be used to host web or worker services.

[Azure Storage Queues](https://learn.microsoft.com/en-us/azure/storage/queues/) is a good messaging solution for sending small messages. [Azure Service Bus](/architecture/azure/messaging.md#azure-service-bus) is a powerful alternative which caters to larger messages and provides additional advanced features.

Web and worker services can persist data using [Azure SQL Database](/architecture/azure/data-stores.md#azure-sql-database), [Azure Cosmos DB](/architecture/azure/data-stores.md#azure-cosmos-db), or other Azure-supported [data stores](/architecture/azure/data-stores).

## Additional resources

* [Azure Architecture Center—Web-queue-worker architecture style](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/web-queue-worker)
