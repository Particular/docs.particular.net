---
title: Multi-tier architecture style on AWS
summary: Gives a description of multi-tier architecture including the components, challenges, and technology options for AWS
reviewed: 2026-01-16
callsToAction: ['solution-architect', 'ADSD']
---

The AWS documentation describes [Multi-Tier architectures](https://docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/introduction.html) as a well-known architecture pattern which divides applications into physical, logical and presentation tiers.

Within the context of a multi-tier application, messaging can help evolve and modernize existing applications to be more reliable, scalable, and maintainable. Some ways messaging can do this are:

- More clearly separate tiers through the use of clearly defined message contracts.
- Reduce the need for [batch jobs](https://particular.net/blog/death-to-the-batch-job).
- Implement long-running or [time-dependent business processes](https://particular.net/webinars/got-the-time).


![Layered architecture on AWS](/architecture/aws/images/aws-multi-tier-architecture.png)

## Components

Typical components within a multi-tier application are:

- Front end: Examples of these are web applications, desktop UIs, or mobile applications.
- Business logic: The business rules that model the solution space.
- Data: One or more databases containing all data models of the application.
- Message queue: Used for sending commands or publishing events between tiers.

## Challenges

Multi-tier architectures come with many benefits, but there are also trade-offs involved in adopting it:

- Physically separating the tiers introduces higher exposure to network related issues that might affect availability. The use of message queues helps to mitigate some of this by decoupling the tiers and increasing resilience across the layers.
- The tiers in a multi-tier architecture style often communicate synchronously to execute business processes. Long-running or heavy workloads can negatively impact the user experience and overall system performance. Asynchronous communication using messaging decouples the tiers interacting with the user from the tiers processing the workload.
- Front end tiers often need to reflect changes made by other users or processes. Communication is generally initiated from front end layers to back end layers, but not the other way around. The use of messaging may be used to provide event-based notifications from the back end to the front end without introducing exceptions to this communication flow.
- Systems using multi-tier architectures are often limited in their technology choices due to existing dependencies.

## Technology choices

Technology choices are often dictated by the tier you are working in. For example:

- [Infrastructure-as-a-Service](/architecture/azure/compute.md#infrastructure-as-a-service) services offer flexibility for creating environments.
- Web-focused front-end or API layers might use managed hosted options like [Elastic Beanstalk](https://aws.amazon.com/elasticbeanstalk) or [LightSail](https://aws.amazon.com/lightsail).
- [Multiple data store options](/architecture/aws/data-stores.md) exist within AWS which allow moving data persistence to the cloud with little effort.
- [Messaging options](/architecture/aws/messaging.md) allow for asynchronous communication between tiers.

## Related content

- [AWS Serverless Multi-Tier Architectures with Amazon API Gateway and AWS Lambda](https://docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/welcome.html)
- [Migrate and Modernize with AWS](https://aws.amazon.com/cloud-migration/how-to-migrate/)
