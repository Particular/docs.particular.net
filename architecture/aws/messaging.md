---
title: AWS messaging services
summary:
reviewed: 2024-03-14
callsToAction: ['solution-architect', 'poc-help']
---

AWS provides a few messaging options that you can take advantage of when using NServiceBus.

The Particular Service Platform already offers in NServiceBus the Amazon SQS transport that leverages the technologies highlighted below.

### Amazon SNS

[https://aws.amazon.com/sns/](https://aws.amazon.com/sns/)

[Amazon Simple Notification Service](https://aws.amazon.com/sns/) (Amazon SNS)  is a fully managed web service that enables publishers to send messages to subscribers using various endpoint types. Publishers can communicate asynchronously with subscribers by sending messages to a topic, which is a logical access point and communication channel. Subscribers can receive messages using a supported endpoint type, such as Amazon Data Firehose, Amazon SQS, AWS Lambda, HTTP, email, mobile push notifications, and mobile text messages (SMS). Amazon SNS can be used for application-to-application (A2A) messaging, where messages are delivered from one application to another, or for application-to-person (A2P) messaging, where messages are delivered to customers with SMS texts, push notifications, and email. Amazon SNS provides features such as message filtering, batching, ordering, deduplication, encryption, and delivery retries to help developers build reliable, scalable, and secure applications.

:heavy_plus_sign: Pros:

- It provides instantaneous, push-based delivery of messages, which eliminates the need to poll for new information and updates.
- It has simple APIs and easy integration with applications, which reduces the development effort and complexity.
- It supports flexible message delivery over multiple transport protocols, such as HTTP, email, SMS, and mobile push notifications.
- It has an inexpensive, pay-as-you-go model with no up-front costs, which lowers the operational expenses.

:heavy_minus_sign: Cons:

- It is not suitable for ordered message processing, as it does not guarantee the order of delivery or the number of deliveries for each message.
- It has limitations in fine-grained control over retry policies, as it only allows configuring the number of retries and the delay between retries for each endpoint type.
- It may incur additional costs for using other AWS services, such as SQS, Lambda, or S3, to process or store the messages delivered by SNS.
- It may have compatibility issues with some third-party services or applications, as it only supports a subset of protocols and format.

---
Call to action??
---

#### When to use Amazon SNS?

Amazon SNS should be used when you are looking for a solution to achieve higher decoupling between your publishers and your topic subscribers.

### Amazon SQS

[https://aws.amazon.com/sqs/](https://aws.amazon.com/sqs/)

[Amazon Simple Queue Service](https://aws.amazon.com/sqs/) (Amazon SQS) is a scalable and managed message queuing service provided by AWS that enables the decoupling of application components. This service is designed to help developers build robust, distributed applications with decoupled components, making it easier to manage traffic, system failures, and complex workflows. SQS provides a reliable and secure platform for sending, storing, and receiving messages at any volume, thereby streamlining the process of building and scaling microservices, distributed systems, and serverless applications.

:heavy_plus_sign: Pros:

- Highly scalable with an ability to handle large volumes of messages automatically
- Highly reliable as messages are locked during processing to prevent loss and enable  concurrency.
- Easy integration with other AWS services
- Enables decoupling and scalability of microservices, distributed systems and serverless applications
- Cost effective as charges are based on usage with no upfront costs easing up the need to do capacity planning and pre-provisioning
- Secure as it allows you to send sensitive data between applications either by managing your keys using AWS Key management (AWS KMS) and by using Amazon SQS server side encryption (SSE)
- Durable as messages are stored on multiple servers
- Supports message deduplication
- Queues can be fully customizable

:heavy_minus_sign: Cons:

- Limited message size (256Kb per message). NServiceBus mitigates this by allowing you to take advantage, in a seamless way, of S3 to work with larger payloads. For more information review the documentation for the [Amazon SQS transport topology](https://docs.particular.net/transports/sqs/topology#s3) and [Amazon SQS configuration options](https://docs.particular.net/transports/sqs/configuration-options).
- Limited control over retry policies which forces delegation of handling retries to consumers increasing the overall complexity of the system
- Messages are only visible for a configurable period of time which can lead to challenges when failures occur
- As your system grows in complexity, managing a large number of queues can be challenging
- Even with FIFO (First-In-First-Out) queues, strict message ordering can be a challenge increasing complexity and impact system performance
- SQS supports a subset of protocols and formats which can originate compatibility issues with third party applications

---
Call to action

- [Simple AmazonSQS Transport usage • Amazon SQS Transport Samples • Particular Docs](https://docs.particular.net/samples/aws/sqs-simple/) (include DataBus properties info)
- [https://docs.particular.net/samples/aws/sagas-lambda-aurora/](https://docs.particular.net/samples/aws/sagas-lambda-aurora/)
---

#### When to use Amazon SQS?

Use Amazon SQS when you want a solution:

- To decouple your microservices and facilitate asynchronous communication between them
- Where you don’t need replay your events or commands to understand current state of your entities/processes
- When you need to manage workloads that require data processing in batches
- When you need to send notifications or alerts within an application
- When you need to do data ingestion and use it as a buffer for incoming requests
- You’d want to fanout and send identical copies of a message to multiple queues in parallel - combined with Amazon SNS.
