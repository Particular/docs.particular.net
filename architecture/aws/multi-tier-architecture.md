---
title: Multi tier architecture style on AWS
summary:
reviewed: 2024-03-14
callsToAction: ['solution-architect', 'ADSD']
---

The AWS documentation describes [Multi-Tier architectures](https://docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/), a well-known architecture pattern which divides applications into physical, logical and presentation tiers.

From https:&#47;/docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/three-tier-architecture-overview.html

![alt_text](/architecture/aws/images/image1.png "image_tooltip")

Messaging can help evolving and modernizing existing applications that have been built using layered architectures:

- Use asynchronous communication and clearly defined message contracts to more clearly separate layers.
- Use messaging to route existing messages to [extracted components](https://codeopinion.com/splitting-up-a-monolith-into-microservices/) running in separate processes.
- Use messaging to [get rid of batch jobs](https://particular.net/blog/death-to-the-batch-job).
- Use messaging to implement long-running or [time-dependent business processes](https://particular.net/webinars/got-the-time).

### Components

- Front end layer: Often these are web applications or Desktop UIs physically separated from the other layers.
- Business logic layer: Contains the business logic.
- Data layer: One or more databases containing all data models of the application.
- Message queue: used for sending commands or publishing events between layers.

### Challenges

- Physically separating the tiers improves scalability of the application but also introduces higher exposure to network related issues that might affect availability. Message queues help to decouple the tiers and increase resilience across the layers.
- The tiers in a Multi-tier architecture style often communicate synchronously to execute business processes. Long-running or heavy workloads can negatively impact the user experience and overall system performance. Asynchronous communication, using messaging, decouples the tiers interacting with the user from the tiers processing the workload.
- Front end tiers often need to reflect changes made by other users or processes. Notifications from lower layers must respect the constraint that lower layers must not reference upper layers, and may be hosted separately from upper layers. Messaging may be used to provide event-based notifications from lower layers to upper layers, while following these constraints.
- As a technically partitioned architecture, business logic is spread across layers. Changes to business logic are more difficult as often more than one layer has to be changed. Therefore, this architectural style may not work well with domain-driven design.

### Technology choices

Systems using layered architectures are often limited in their technology choices due to existing dependencies. [Infrastructure-as-a-Service services](/architecture/azure/compute#infrastructure-as-a-service) offer flexibility for creating environments that meet these requirements. Web-focused front-end or API layers might use managed hosted options like [Elastic Beanstalk](https://aws.amazon.com/elasticbeanstalk) or [LightSail](https://aws.amazon.com/lightsail) without major changes. AWS also offers [multiple data store options](link-to-data-stores) which allow moving data persistence to the cloud with little effort, unlocking more flexible scaling opportunities.

### Related content

- [https:&#47;/docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/welcome.html](https://docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/welcome.html)
- [https:&#47;/aws.amazon.com/cloud-migration/how-to-migrate/](https://aws.amazon.com/cloud-migration/how-to-migrate/)