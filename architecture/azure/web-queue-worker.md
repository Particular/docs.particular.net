---
title: Web-queue-worker architectures on Azure
summary:
reviewed: 2023-07-18
---

include: architecture-style-note

![](azure-web-queue-worker.png)

Web-Queue-Worker architectures describe web-focused systems for simple domains that want to primarily use managed services rather than infrastructure as a service. They are easy to deploy and manage while the front end and the worker can be scaled independently. The worker offloads long-running workflows from the front end to keep the system both responsive and resilient.

### Architecture components

The main components are:
* The **web front end** serves client requests, handles authentication and authorization and might even access the database directly to handle time-sensitive requests or access results from the worker.
* The front end queues commands for work intensive or long-running workloads into the **message queue** for the worker to consume. This allows the web application to remain independent from the worker's workload and immediately resume to complete http requests.
* **The worker** picks up the queued up work from the message queue and runs the business process. Results can be stored in the data storage.
* Both worker and front end might access the **data store** directly to access business data and process state.
* A **Content Delivery Network** can help to serve static content even faster and reduce load on the web front end.


### Challenges


### Technology choices

Web-Queue-Worker architectures can make use of Azure's managed services like Azure App Services and Azure Functions, Cosmos DB, Azure SQL, Azure Service Bus and Azure Queue Storage.