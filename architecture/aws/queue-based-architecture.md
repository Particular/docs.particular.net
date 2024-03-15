---
title: Queue based architecture style on AWS
summary:
reviewed: 2024-03-14
callsToAction: ['solution-architect', 'ADSD']
---

The AWS Well-Architected framework describes the [queue-worker architecture style](https://docs.aws.amazon.com/wellarchitected/latest/high-performance-computing-lens/queue-based-architecture.html) as a way to offload compute intensive operations from clients. It is ideal as a model to quickly scale and respond to quick bursts of client requests in a short period of time, to support processing of periodic tasks, background jobs and asynchronous flows. The architecture would be composed of any client sending commands to a queue which will be processed by a dedicated worker.

The following diagram is an example of what a queue based architecture would look like.

![alt_text](images/image2.png "image_tooltip")

### Components

- Users initiate requests through their **frontend system**. This frontend system can be a web application, a terminal, a mobile device, or a third party system. For the purpose of this example, it will be responsible for handling authentication and authorization.
- An AWS CLI or **Amazon SDK** that allows the system to interact with Amazon SQS and other AWS products within company's ecosystem.
- Requests are stored and queued as messages in Amazon SQS through the Amazon SDK
- Workers are hosted in EC2 instances running within an autoscaling group. The workers use NServiceBus to pull data from the Amazon SQS queue, process and store it on Amazon DB. See the [technology choices section]() for other component alternatives.

### Challenges

- In a system composed of multiple components, not every operation on the database has to go through a worker. Workers are designed for resource-intensive tasks or long-running workflows.
- This style is suitable for simple business domains. Without careful design, the front end systems and the worker can become complex, monolithic components that are difficult to maintain. Consider event-driven and microservices architectural styles for more complex business domains.

### Technology choices

The queue-worker architecture style can make use of AWS managed services like AWS Fargate, AWS App Runner, AWS Amplify Hosting, AWS AppSync, AWS Lambda, Amazon Cloud Watch, Amazon EC2 Instances, Amazon DynamoDB, Amazon DocumentDB, Amazon Aurora, Amazon S3, Amazon SNS/SQS, Amazon MQ, Amazon Event Bridge and AWS Kinesis amongst others. When relying on a Infrastructure-as-a-Service model, queue-worker architectures might provide fewer benefits.

## Additional resources

* [Queue-Based Architecture - High Performance Computing Lens (amazon.com)](https://docs.aws.amazon.com/wellarchitected/latest/high-performance-computing-lens/queue-based-architecture.html)