# AWS guidance


# Introduction



* [https://docs.particular.net/samples/aws/](https://docs.particular.net/samples/aws/)


# AWS Well-Architected

The [AWS Well-Architected](https://aws.amazon.com/architecture/well-architected) Framework helps cloud architects build secure, high-performing, resilient, and efficient infrastructure for a variety of applications and workloads. It’s built around six pillars: operational excellence, security, reliability, performance efficiency, cost optimization, and sustainability. The framework provides a consistent approach for customers and partners to evaluate architectures and implement scalable designs.

NServiceBus helps you achieve the six pillars of the AWS Well-architected Framework in a number of ways.


### Reliability



* NServiceBus handles unexpected failures and provides the[ recoverability features](https://docs.particular.net/nservicebus/recoverability/) required by self-healing systems.
* NServiceBus provides health metrics which can be monitored using[ ServicePulse](https://docs.particular.net/servicepulse/) and[ OpenTelemetry](https://docs.particular.net/nservicebus/operations/opentelemetry).


### Performance Efficiency



* NServiceBus endpoints can be scaled out easily using methods such as the built-in[ competing consumers mechanism](https://docs.particular.net/nservicebus/scaling#scaling-out-to-multiple-nodes-competing-consumers) and scaled up while tuning for[ concurrency](https://docs.particular.net/nservicebus/operations/tuning).
* NServiceBus is designed and tested for[ high-performance and memory efficiency](https://particular.net/blog/pipeline-and-closure-allocations).
* [Monitoring](https://docs.particular.net/monitoring/) allows observation of individual endpoint performance and identification of bottlenecks.


### Security



* NServiceBus provides data encryption in transit with[ message encryption](https://docs.particular.net/nservicebus/security/property-encryption).
* NServiceBus supports the[ least privilege](https://docs.particular.net/nservicebus/operations/installers#when-to-run-installers) approach during application deployment and runtime.


### Cost Optimization



* Costs may be optimized by[ choosing the most appropriate technology](https://docs.particular.net/architecture/aws/#technology-choices).


### Operational Excellence



* The Particular Service Platform[ creates required infrastructure components](https://docs.particular.net/nservicebus/operations/installers) using dedicated installation APIs or infrastructure scripting tools.
* ServicePulse provides[ detailed insights](https://docs.particular.net/servicepulse/) into the operational health of the system.
* NServiceBus supports[ OpenTelemetry](https://docs.particular.net/nservicebus/operations/opentelemetry) to integrate with 3rd-party monitoring and tracing tools.
* [Messaging](https://docs.particular.net/nservicebus/messaging/) allows loosely coupled architectures with autonomous and independent services.
* NServiceBus APIs are designed for[ unit testing](https://docs.particular.net/nservicebus/testing/).


### Sustainability



* By abstracting you from the underlying [technology choices of AWS](https://docs.particular.net/architecture/aws/#technology-choices), NServiceBus allows you to change to more efficient hardware and software offerings when they become available with minimal changes.

By integrating NServiceBus with AWS services according to well-architected principles, you can build a robust, scalable, and reliable distributed system that delivers optimal performance, security, and cost efficiency on the AWS cloud platform.


# Architectural styles


## Event-driven architecture on AWS

The AWS Architecture Center describes the[ event-driven architecture](https://aws.amazon.com/event-driven-architecture/) as using events to trigger and communicate between decoupled services.

The Particular Service Platform implements[ pub/sub](https://docs.particular.net/nservicebus/messaging/publish-subscribe/), with each[ NServiceBus endpoint](https://docs.particular.net/nservicebus/endpoints/) acting as a publisher (event producer) and/or subscriber (endpoint consumer).


### Components



* NServiceBus publisher (event producer): Publishes events with business meaning in a reliable fire-and-forget style and has no knowledge of subscribers (there may be none).
* NServiceBus subscriber (event consumer): Subscribed to a specific event type and reacts to it. A subscriber may also be an event publisher, since processing an event may lead to publishing more events.
* Simple Queue Service: The messaging service that brings together publisher and subscriber without explicitly referencing or depending on each other.


### Challenges



* Events order: Subscribers cannot rely on the order they receive published events, which may be affected by many factors such as concurrency, scaling, retries, partitioning, etc. Events and subscribers should be designed so that they[ do not rely on strict ordering to execute business processes](https://particular.net/blog/you-dont-need-ordered-delivery).
* Event data: Putting too much data on messages couples publishers and subscribers, defeating one of the main benefits of messaging in the first place. Bloated event contracts indicate sub-optimal service boundaries, perhaps drawn along technical constraints rather than business rules, or[ data distribution](https://docs.particular.net/nservicebus/concepts/data-distribution) over messaging. Well designed events are[ lightweight contracts](https://particular.net/blog/putting-your-events-on-a-diet), focusing on sharing IDs rather than data.


### Technology choices

In event-driven architectures, components are decoupled, allowing choice of the most suitable[ compute](https://docs.particular.net/architecture/aws/compute) and[ data store](https://docs.particular.net/architecture/aws/data-stores) options for a specific component or set of components.

An event-driven approach requires support for the publish-subscribe model. NServiceBus supports the publish-subscribe model for[ AWS SQS](https://docs.particular.net/architecture/AWS/messaging), independent of the underlying service capabilities.


## Microservices

According to the [AWS guidance](https://aws.amazon.com/microservices/), microservices are an architectural approach where software is composed of small, independent services communicating over well-defined APIs.

The Particular Service Platform makes it easy to use microservices by defining [NServiceBus endpoints](https://docs.particular.net/nservicebus/endpoints/) that act as one of these independent services. These endpoints use [messaging patterns](https://docs.particular.net/nservicebus/messaging/) to ensure the services remain autonomous.


### Components



* [NServiceBus endpoint](https://docs.particular.net/nservicebus/endpoints/) (service): Each service is an autonomously deployable and scalable unit with a private [data store](https://docs.particular.net/architecture/azure/data-stores).
* Message bus: The message bus provides an asynchronous, reliable, and fault-tolerant communication channel which decouples the services.
* Gateway: A gateway is a facade which allows user technologies such as web browsers to decouple from service implementations. Gateways may also provide further operational facilities, but do not contain business logic. In AWS, APIs can be managed with [AWS API Gateway](https://aws.amazon.com/api-gateway/).


### Service boundaries

Finding good service boundaries is one of the biggest challenges with the microservice architectural style. Suboptimal boundaries often lead to a lack of data isolation and excessive inter-service communication. This often leads to high coupling between services that implement business processes, sometimes referred to as a distributed monolith. To define autonomous services, it is crucial to focus on business boundaries rather than technical boundaries.

– EMBED UDI’S PRESENTATION HERE

In this presentation, Udi Dahan demonstrates the process of finding good service boundaries. He explains the challenges of traditional layered architectures and covers an approach that cuts across all application layers, outlining the natural lines of loose and tight coupling. Finally, Udi shows how these vertical slices collaborate using events, enabling flexible and high performance business processes.

[Blog post: Goodbye microservices, hello right-sized services →](https://particular.net/blog/goodbye-microservices-hello-right-sized-services)


### RPC vs. messaging



* Copy from [https://docs.particular.net/architecture/azure/microservices](https://docs.particular.net/architecture/azure/microservices)


### User interfaces



* Copy from [https://docs.particular.net/architecture/azure/microservices](https://docs.particular.net/architecture/azure/microservices)


### Microservice technologies



* Copy from [https://docs.particular.net/architecture/azure/microservices](https://docs.particular.net/architecture/azure/microservices) EXCEPT last paragraph

Some common technology options for building microservices in AWS include:



* Fully managed services: Amazon Lambda and Amazon Elastic Beanstalk.
* Container services: Amazon Elastic Container Service (ECS), Amazon Elastic Kubernetes Service (EKS)
* Data stores: Amazon DynamoDB, Amazon Aurora, Amazon Relational Database Service (RDS)
* Messaging: Amazon Simple Queue Service (SQS), Amazon Simple Notification Service (SNS)


## Multi-tier

The AWS documentation describes [Multi-Tier architectures](https://docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/), a well-known architecture pattern which divides applications into physical, logical and presentation tiers. 

From https://docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/three-tier-architecture-overview.html

![alt_text](images/image1.png "image_tooltip")


Messaging can help evolving and modernizing existing applications that have been built using layered architectures:



* Use asynchronous communication and clearly defined message contracts to more clearly separate layers.
* Use messaging to route existing messages to [extracted components](https://codeopinion.com/splitting-up-a-monolith-into-microservices/) running in separate processes.
* Use messaging to [get rid of batch jobs](https://particular.net/blog/death-to-the-batch-job).
* Use messaging to implement long-running or [time-dependent business processes](https://particular.net/webinars/got-the-time).


### Components



* Front end layer: Often these are web applications or Desktop UIs physically separated from the other layers.
* Business logic layer: Contains the business logic.
* Data layer: One or more databases containing all data models of the application.
* Message queue: used for sending commands or publishing events between layers.


### Challenges



* Physically separating the tiers improves scalability of the application but also introduces higher exposure to network related issues that might affect availability. Message queues help to decouple the tiers and increase resilience across the layers.
* The tiers in a Multi-tier architecture style often communicate synchronously to execute business processes. Long-running or heavy workloads can negatively impact the user experience and overall system performance. Asynchronous communication, using messaging, decouples the tiers interacting with the user from the tiers processing the workload.
* Front end tiers often need to reflect changes made by other users or processes. Notifications from lower layers must respect the constraint that lower layers must not reference upper layers, and may be hosted separately from upper layers. Messaging may be used to provide event-based notifications from lower layers to upper layers, while following these constraints.
* As a technically partitioned architecture, business logic is spread across layers. Changes to business logic are more difficult as often more than one layer has to be changed. Therefore, this architectural style may not work well with domain-driven design.


### Technology choices

Systems using layered architectures are often limited in their technology choices due to existing dependencies. [Infrastructure-as-a-Service services](https://docs.particular.net/architecture/azure/compute#infrastructure-as-a-service) offer flexibility for creating environments that meet these requirements. Web-focused front-end or API layers might use managed hosted options like [Elastic Beanstalk](https://aws.amazon.com/elasticbeanstalk) or [LightSail](https://aws.amazon.com/lightsail) without major changes. AWS also offers [multiple data store options](link-to-data-stores) which allow moving data persistence to the cloud with little effort, unlocking more flexible scaling opportunities.


### Related content



* [https://docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/welcome.html](https://docs.aws.amazon.com/whitepapers/latest/serverless-multi-tier-architectures-api-gateway-lambda/welcome.html)
* [https://aws.amazon.com/cloud-migration/how-to-migrate/](https://aws.amazon.com/cloud-migration/how-to-migrate/)


## Queue Worker Architecture (or Queue-Based Architecture to align with AWS docs?)

The AWS Well-Architected framework describes the [queue-worker architecture style](https://docs.aws.amazon.com/wellarchitected/latest/high-performance-computing-lens/queue-based-architecture.html) as a way to offload compute intensive operations from clients. The architecture would be composed of any client sending commands to a queue which will be processed by a dedicated worker.

The following diagram is an example of what a queue based architecture would look like.


![alt_text](images/image2.png "image_tooltip")



### Components



* Users initiate requests through their **frontend system**. This frontend system can be a web application, a terminal, a mobile device, or a third party system. For the purpose of this example, it will be responsible for handling authentication and authorization.
* An AWS CLI or **Amazon SDK** that allows you to interact with Amazon SQS and other AWS products within your ecosystem.
* Requests are queued as messages in Amazon SQS through the Amazon SDK
* Workers hosted in EC2 instances running within an autoscaling group using NServiceBus to pull data from the Amazon SQS queue, process and store it on Amazon DB. Please see the technology choices section for other component alternatives.


### Challenges



* In a system composed of multiple components, not every operation on the database has to go through a worker. Workers are designed for resource-intensive tasks or long-running workflows.
* This style is suitable for simple business domains. Without careful design, the front end systems and the worker can become complex, monolithic components that are difficult to maintain. Consider event-driven and microservices architectural styles for more complex business domains.


### Technology choices

The queue-worker architecture style can make use of AWS managed services like AWS Fargate, AWS App Runner, AWS Amplify Hosting, AWS AppSync, AWS Lambda, Amazon Cloud Watch, Amazon EC2 Instances, Amazon DynamoDB, Amazon DocumentDB, Amazon Aurora, Amazon S3, Amazon SNS/SQS, Amazon MQ, Amazon Event Bridge and AWS Kinesis amongst others. When relying on a Infrastructure-as-a-Service model, queue-worker architectures might provide fewer benefits.

[Queue-Based Architecture - High Performance Computing Lens (amazon.com)](https://docs.aws.amazon.com/wellarchitected/latest/high-performance-computing-lens/queue-based-architecture.html)


## Technology choices


### Compute

AWS provides a range of hosting options, spanning the range from Serverless all the way through to EC2 instances.

The Particular Service Platform can be hosted using: 

![alt_text](images/image3.png "image_tooltip")


#### Serverless

[AWS Lambda](https://aws.amazon.com/lambda/) is a serverless compute service offered by AWS that allows developers to run code in response to events without the need to manage servers. NServiceBus supports AWS Lambda so that new or existing applications can directly consume messages from [SQS queues](https://aws.amazon.com/sqs/).

[Host NServiceBus applications on AWS Lambda](https://docs.particular.net/nservicebus/hosting/aws-lambda-simple-queue-service/).

#### PaaS

[Platform as a Service (PaaS)](https://en.wikipedia.org/wiki/Platform_as_a_service) models provide managed hosting environments where applications can be deployed without having to manage the underlying infrastructure, operating system, or runtime environments.

[AWS Elastic Beanstalk](https://aws.amazon.com/elasticbeanstalk/) allows you to deploy your .NET web applications to either IIS or Nginx and have Beanstalk provision and manage your infrastructure. NServiceBus applications can be integrated into these .NET Core web applications.


##### Containers

[Containers](https://en.wikipedia.org/wiki/Containerization_(computing)) are a popular mechanism to deploy and host applications in PaaS services. NServiceBus can be used by containerized applications and deployed to services like:



* [Amazon Elastic Container Service](https://aws.amazon.com/ecs/)
* [Amazon Elastic Kubernetes Service](https://aws.amazon.com/eks/)


#### IaaS

Infrastructure as a Service (IaaS) provides virtualized computing resources like virtual machines, storage, and networking that can be used to build and manage the required infrastructure.

NServiceBus applications can easily be hosted on virtual machines. Popular techniques include:



* [Integrating NServiceBus with the Microsoft Generic Host](https://docs.particular.net/nservicebus/hosting/extensions-hosting)
* [Custom hosted web applications](https://docs.particular.net/nservicebus/hosting/web-application)
* [Installing NServiceBus endpoints as Windows Services](https://docs.particular.net/nservicebus/hosting/windows-service)
* [Manually controlling NServiceBus lifecycle in an executable (e.g. Console or GUI applications)](https://docs.particular.net/nservicebus/hosting/#self-hosting)
* [Custom-managed Kubernetes clusters hosting container applications](https://docs.particular.net/nservicebus/hosting/docker-host/)


#### Choosing a hosting model

The best choice of hosting model depends on the desired characteristics, such as:



* **Scalability**: Different hosting options offer different approaches to scaling. Managed solutions are typically easier to scale on demand and can scale on more granular levels. In addition to the scalability, elasticity (the time required to scale up or down) may also factor into the choice.
* **Pricing:** Managed services typically offer more dynamic pricing models that adjust with the demands of an application, in comparison with more rigid pricing models for infrastructure services. However, managed services typically charge more for their pricing units, so infrastructure services may be more economical for consistent demand. AWS offers a[ pricing calculator](https://calculator.aws/) to help understand a given service's pricing model.
* **Portability:** Serverless models are primarily built on proprietary programming models, heavily tied to the cloud service vendor. Hosting models built on open standards make it easier to run components in other hosting environments. Additionally, it may also be desirable to run components using on-premises servers or workstations.
* **Flexibility:** Lower-level infrastructure provides more control over the configuration and management of applications. Serverless offerings offer less flexibility due to the higher level of abstractions exposed to the code.
* **Manageability:** Serverless and PaaS models remove the concerns about the underlying infrastructure challenges (e.g. automatic scaling, OS updates, load balancing, etc.), typically at the cost of flexibility. Managing and maintaining infrastructure using other models may require significant resources and knowledge.


### Data stores

AWS offers a number of [data storage services](https://aws.amazon.com/getting-started/decision-guides/databases-on-aws-how-to-choose/) that integrate well with the Particular Service Platform.


#### Amazon DynamoDB

[Amazon DynamoDB](https://aws.amazon.com/dynamodb/) is a fully managed, serverless, NoSQL database service. It is designed for high-performance and scalable applications, offering low-latency access to data regardless of the scale. DynamoDB supports both document and key-value data models and automatically scales to handle the throughput and storage requirements of applications. Its features include automatic partitioning, backup and restore, and global replication for data distribution across multiple regions.

➕ Pros:



* Built and optimized for scalability
* Multi-region support for high availability scenarios
* Single-digit millisecond performance
* Fully managed

➖ Cons:



* Can be difficult to predict costs
* Limited storage capacity for individual items
* Can not query if no indexes are available

[Try the NServiceBus DynamoDB sample →](https://docs.particular.net/samples/aws/dynamodb-simple/)


#### Amazon Aurora

[Amazon Aurora](https://aws.amazon.com/rds/aurora/) is a relational database service provided by Amazon Web Services (AWS), compatible with MySQL and PostgreSQL. It is designed to offer high performance, durability, and availability for database applications. Aurora uses a distributed and fault-tolerant architecture, providing automatic replication and failover capabilities to enhance data reliability and reduce downtime.

➕ Pros:



* High-performance and low-latency database operations
* Compatible with MySQL and PostgreSQL, allowing for easy migration of existing applications without major code modifications
* Can automatically scale both read and write operations to handle varying workloads
* Fully managed

➖ Cons:



* Can be more expensive than traditional relational databases for smaller applications.
* More complex to manage for users that aren’t familiar with distributed and replicated database systems

[Try NServiceBus sagas with Aurora, SQS, and Lambda →](https://docs.particular.net/samples/aws/sagas-lambda-aurora/)


#### Amazon RDS 

[Amazon RDS](https://aws.amazon.com/rds/) (Relational Database Service) is a managed database service that supports multiple database engines such as MySQL, PostgreSQL, Oracle, Microsoft SQL Server, and others. It simplifies many database administration tasks, including backups, software patching, and scaling, allowing users to focus on application development. RDS provides a reliable and scalable solution with features like backup retention, automatic software patching, and multi-region deployments for high availability and fault tolerance. 

➕ Pros:



* Fully managed
* Supports a variety of database engines
* Easily scaled vertically or horizontally
* Supports multi-AZ deployments for high availability

➖ Cons:



* Potential latency between primary instance and read replicas
* Can be more expensive than self-managed solutions for smaller applications
* May not update to the latest version of database engines right away

[See how to use NServiceBus sagas with the relational database of your choice →](https://docs.particular.net/samples/sql-persistence/simple/)

[https://aws.amazon.com/dynamodb/](https://aws.amazon.com/dynamodb/)



* [https://docs.particular.net/persistence/dynamodb/](https://docs.particular.net/persistence/dynamodb/)
* [https://docs.particular.net/nservicebus/transactional-session/persistences/dynamodb](https://docs.particular.net/nservicebus/transactional-session/persistences/dynamodb)
* [https://docs.particular.net/samples/aws/dynamodb-transactions/](https://docs.particular.net/samples/aws/dynamodb-transactions/)
* [https://docs.particular.net/samples/aws/dynamodb-simple/](https://docs.particular.net/samples/aws/dynamodb-simple/)
* [https://docs.particular.net/persistence/dynamodb/outbox](https://docs.particular.net/persistence/dynamodb/outbox)
* [https://docs.particular.net/samples/aws/sagas/](https://docs.particular.net/samples/aws/sagas/)
* [https://docs.particular.net/persistence/dynamodb/transactions](https://docs.particular.net/persistence/dynamodb/transactions)
* [https://docs.particular.net/persistence/dynamodb/sagas](https://docs.particular.net/persistence/dynamodb/sagas)

[https://aws.amazon.com/rds/aurora/](https://aws.amazon.com/rds/aurora/)



* [https://docs.particular.net/samples/aws/sagas-lambda-aurora/](https://docs.particular.net/samples/aws/sagas-lambda-aurora/)

[https://aws.amazon.com/s3/](https://aws.amazon.com/s3/)



* [https://docs.particular.net/samples/aws/sqs-simple/](https://docs.particular.net/samples/aws/sqs-simple/)

[https://aws.amazon.com/rds/](https://aws.amazon.com/rds/) - point to SQLP samples


## Messaging


### Amazon SNS

[https://aws.amazon.com/sns/](https://aws.amazon.com/sns/)

[Amazon Simple Notification Service](https://aws.amazon.com/sns/) (Amazon SNS)  is a fully managed web service that enables publishers to send messages to subscribers using various endpoint types. Publishers can communicate asynchronously with subscribers by sending messages to a topic, which is a logical access point and communication channel. Subscribers can receive messages using a supported endpoint type, such as Amazon Data Firehose, Amazon SQS, AWS Lambda, HTTP, email, mobile push notifications, and mobile text messages (SMS). Amazon SNS can be used for application-to-application (A2A) messaging, where messages are delivered from one application to another, or for application-to-person (A2P) messaging, where messages are delivered to customers with SMS texts, push notifications, and email. Amazon SNS provides features such as message filtering, batching, ordering, deduplication, encryption, and delivery retries to help developers build reliable, scalable, and secure applications.

➕ Pros:



* It provides instantaneous, push-based delivery of messages, which eliminates the need to poll for new information and updates.
* It has simple APIs and easy integration with applications, which reduces the development effort and complexity.
* It supports flexible message delivery over multiple transport protocols, such as HTTP, email, SMS, and mobile push notifications.
* It has an inexpensive, pay-as-you-go model with no up-front costs, which lowers the operational expenses.

➖ Cons:



* It is not suitable for ordered message processing, as it does not guarantee the order of delivery or the number of deliveries for each message.
* It has limitations in fine-grained control over retry policies, as it only allows configuring the number of retries and the delay between retries for each endpoint type.
* It may incur additional costs for using other AWS services, such as SQS, Lambda, or S3, to process or store the messages delivered by SNS.
* It may have compatibility issues with some third-party services or applications, as it only supports a subset of protocols and format.

Call to action??


#### When to use Amazon SNS?

Amazon SNS should be used when you are looking for a solution to achieve higher decoupling between your publishers and your topic subscribers.


### Amazon SQS

[https://aws.amazon.com/sqs/](https://aws.amazon.com/sqs/)

[Amazon Simple Queue Service](https://aws.amazon.com/sqs/) (Amazon SQS) is a scalable and managed message queuing service provided by AWS that enables the decoupling of application components. This service is designed to help developers build robust, distributed applications with decoupled components, making it easier to manage traffic, system failures, and complex workflows. SQS provides a reliable and secure platform for sending, storing, and receiving messages at any volume, thereby streamlining the process of building and scaling microservices, distributed systems, and serverless applications.

➕ Pros:



* Highly scalable with an ability to handle large volumes of messages automatically
* Highly reliable as messages are locked during processing to prevent loss and enable  concurrency.
* Easy integration with other AWS services
* Enables decoupling and scalability of microservices, distributed systems and serverless applications
* Cost effective as charges are based on usage with no upfront costs easing up the need to do capacity planning and pre-provisioning
* Secure as it allows you to send sensitive data between applications either by managing your keys using AWS Key management (AWS KMS) and by using Amazon SQS server side encryption (SSE)
* Durable as messages are stored on multiple servers
* Supports message deduplication
* Queues can be fully customizable

➖ Cons:



* Limited message size (256Kb per message)
* Limited control over retry policies which forces delegation of handling retries to consumers increasing the overall complexity of the system
* Messages are only visible for a configurable period of time which can lead to challenges when failures occur
* As your system grows in complexity, managing a large number of queues can be challenging
* Even with FIFO (First-In-First-Out) queues, strict message ordering can be a challenge increasing complexity and impact system performance
* SQS supports a subset of protocols and formats which can originate compatibility issues with third party applications

 \
Call to action



* [Simple AmazonSQS Transport usage • Amazon SQS Transport Samples • Particular Docs](https://docs.particular.net/samples/aws/sqs-simple/) (include DataBus properties info)
* [https://docs.particular.net/samples/aws/sagas-lambda-aurora/](https://docs.particular.net/samples/aws/sagas-lambda-aurora/)


#### When to use Amazon SQS?

Use Amazon SQS when you want a solution:

* To decouple your microservices and facilitate asynchronous communication between them
* Where you don’t need replay your events or commands to understand current state of your entities/processes
* When you need to manage workloads that require data processing in batches
* When you need to send notifications or alerts within an application
* When you need to do data ingestion and use it as a buffer for incoming requests
* You’d want to fanout and send identical copies of a message to multiple queues in parallel - combined with Amazon SNS.

## Observability

AWS offers several observability solutions which can be used with the Particular Service Platform. They provide native monitoring, logging, alarming, and dashboards with Amazon CloudWatch, tracing through AWS X-Ray, as well as other open-source based managed services, together providing [three pillars observability signals](https://opentelemetry.io/docs/concepts/signals/).


#### Amazon CloudWatch

[Amazon CloudWatch]([https://aws.amazon.com/cloudwatch/](https://aws.amazon.com/cloudwatch/)) is a monitoring and observability service that allows you to collect and access performance and operational data in the form of logs and metrics on a single platform.

Pros:



* A single tool to visualize metrics and logs emitted by your system
* Fully managed service
* Tightly integrated with other AWS services, such as AWS Lambda and Amazon EC2
* Supports querying data from multiple sources, allowing you to monitor metrics on AWS, on premises or other clouds
* Customizable dashboards and automated alarms and actions
* Real-time telemetry

Cons:



* Amazon CloudWatch does not support traces
* AWS CloudWatch offers a [free tier](https://aws.amazon.com/cloudwatch/pricing/), but costs may escalate with increases usage, requiring dedicated monitoring
* 
* 

The Particular Service Platform collects metrics in two forms:



* OpenTelemetry-based metrics, which can be collected by enabling OpenTelemetry and exporting the metrics to Amazon CloudWatch
* Custom metrics with NServiceBus.Metrics which can be exported to Amazon CloudWatch.

Try the Amazon CloudWatch sample for NServiceBus metrics and logs →

**TODO**:



1. Create a sample that uses NSB metrics, which integrates with Cloudwatch
    1. Collect NSB metrics, send to cloudwatch
    2. Collect [NLog logs with extensions.logging](https://docs.particular.net/samples/logging/extensions-logging/), [send to Cloudwatch](https://docs.aws.amazon.com/prescriptive-guidance/latest/patterns/configure-logging-for-net-applications-in-amazon-cloudwatch-logs-by-using-nlog.html) 


#### Amazon X-Ray

[Amazon X-Ray]([https://aws.amazon.com/xray/](https://aws.amazon.com/xray/)) is a service that can collect trace data from your applications, providing insights that can help identify issues or bottlenecks that could benefit from optimization.

Pros:

* A single tool to visualize application-level traces and infrastructure-level traces
* AWS X-Ray creates a service map using trace data
* Integrates with other AWS services, such as AWS Lambda, Amazon EC2, Amazon ECS and AWS Elastic Beanstalk
* Fully managed service

Cons:

* AWS X-Ray offers a [free tier](https://aws.amazon.com/xray/pricing/), but costs may escalate with increases usage, requiring dedicated monitoring
* Pricing is based on the amount and the type of telemetry collected
* AWS X-Ray is designed to work within the AWS ecosystem
* AWS X-Ray’s `traceId` format differs from the [W3C format](https://www.w3.org/TR/trace-context/#trace-id), and requires mapping for compatibility reasons which should be considered when using [OpenTelemetry](link-to-otel-section-under-observability)

The Particular Service Platform allows you to capture spans emitted by NServiceBus, providing insights into message processing, retries, and more.

&lt;link to presentation> [https://particular.net/videos/message-processing-failed](https://particular.net/videos/message-processing-failed)

In this presentation, Laila Bougria discusses the need for distributed tracing in distributed systems, as well as the [ADOT collector (AWS Distro for OpenTelemetry Collector)](https://aws-otel.github.io/docs/getting-started/collector), AWS’ OpenTelemetry Collector implementation that simplifies the export of distributed traces from applications to AWS X-Ray, amongst others.

[Try the OpenTelemetry sample with export to Amazon X-Ray →](https://github.com/lailabougria/talks/tree/main/message-processing-failed-but-whats-the-root-cause/samples/aws) 


### Concepts - Observability (or monitoring)

Observability is the idea that we should be able to understand our system’s behavior by investigating telemetry information, without the need to change any code or configuration, or investigate any system-specific data stores which may not be accessible and/or contain sensitive information. By collecting telemetry data from our systems, we’re able to query and analyze that information, with the end-goal of extracting actionable insights that can help us improve our systems.

Observability can help address multiple concerns in a system:


#### Root-cause analysis

Distributed systems are complex system landscapes where multiple components interact with each other through different mechanisms in different time frames (asynchronous and synchronous communication). This makes it a lot harder to pinpoint the root cause of failures, as multiple components and infrastructure may be involved. It’s quite common in message-based systems, that the cause of a failure observed in one component, is caused by another component further upstream. By collecting traces and logs from our systems, we can gather insights into what’s happening in the system and make it easier to pinpoint the root cause of failures.


#### Health monitoring

Is my system healthy and operable? The ability to answer this question becomes more complex in distributed systems as our system is composed of multiple components that operate autonomously. This requires us to pay more attention to the health and operability of individual components. By emitting telemetry (in the form of metrics or otherwise) from all components that make up our systems, we can understand the health of our system and gain insights into individual components that may be struggling.


#### Performance monitoring

One component of performance monitoring that is harder to understand in production environments, is latency. Latency can severely affect the performance of our overall system, even when individual components have been performance-tested during development. Emitting telemetry, specifically traces, can help us gain insight into the entire request execution, across all the relevant components in the system.


#### OpenTelemetry

With the increasing need for observability, multiple standards started emerging in the industry to standardize the collection of telemetry data from distributed systems, including Google’s [OpenCensus standard](https://opencensus.io/) and [OpenTracing](https://opentracing.io/). In an effort to standardize in a cross-platform, cross-runtime, cross-language manner, the [Cloud Native Computing Foundation](https://www.cncf.io/) set up the [OpenTelemetry project](https://opentelemetry.io), effectively merging the OpenCensus and OpenTracing standards into one. Its goal is to help standardize how we instrument, generate, collect, and export telemetry data from our applications, by providing multiple tools and SDK for multiple languages and platforms. Most observability vendors have adopted the OpenTelemetry standard, enabling customers to store their telemetry data in a vendor-agnostic format.

Video: Message processing failed, but what’s the root cause? https://www.youtube.com/watch?v=U8Aame0akl4&pp=ygUSbGFpbGEgYm91Z3JpYSBvc2xv


#### The Particular Service Platform

The Particular Service Platform offers multiple capabilities that allow you to observe the message flows that are occurring in the system. ServiceInsight has an endpoint explorer view, allowing you to understand all components that are sending and receiving messages in the system. More importantly, ServiceInsight offers multiple views that offer insights into message and [saga flows](https://docs.particular.net/architecture/workflows#orchestration-implementing-orchestrated-workflows), based on audit messages. These message-based conversations that are occurring in the systems, are visualized in [flow diagrams](https://docs.particular.net/serviceinsight/#flow-diagram), [sequence diagrams](https://docs.particular.net/serviceinsight/#sequence-diagram), and [saga views](https://docs.particular.net/serviceinsight/#the-saga-view).
