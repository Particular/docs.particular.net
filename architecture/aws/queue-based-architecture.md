---
title: Queue-based architecture style on AWS
summary: Gives a description of queue-based architecture including the components, challenges, and technology options for AWS
reviewed: 2026-01-05
callsToAction: ['solution-architect', 'ADSD']
---

The queue-based architectural style is a way to offload compute intensive operations from clients. It is ideal as a model to quickly scale and respond to bursts of requests in a short period of time to support processing of periodic tasks, background jobs, asynchronous flows, etc. The architecture is composed of clients sending commands to a queue which are processed by a dedicated worker.

The following diagram is an example of what a queue based architecture would look like.

!["Queue based architecture sample"](/architecture/aws/images/aws-queue-based-architecture.png)

## Components

- Users initiate requests through their **frontend system**. This frontend system can be a web application, a terminal, a mobile device, or a third party system.
- Requests are stored and queued as messages in Amazon SQS.
- Workers are hosted in EC2 instances running within an autoscaling group. The workers use NServiceBus to pull messages from the Amazon SQS queue, process the messages, and store business data in a data store.

## Challenges

The queue-based architectural style is suitable for simple business domains. Without careful design, the front end systems and the worker can become complex, monolithic components that are difficult to maintain. For more complex business domains, [consider event-driven](event-driven-architecture.md) or [microservices](microservices.md) architectural styles.

## Technology choices

The queue-based architectural style can make use of AWS managed services such as [AWS EC2 Instances](https://aws.amazon.com/ec2/), [AWS Lambda](https://aws.amazon.com/lambda/), [AWS Amplify](https://aws.amazon.com/amplify/), [Amazon Cloud Watch](https://aws.amazon.com/cloudwatch/), [Amazon DynamoDB](https://aws.amazon.com/dynamodb/), [Amazon DocumentDB](https://aws.amazon.com/documentdb/), [Amazon Aurora](https://aws.amazon.com/rds/aurora/), [Amazon S3](https://aws.amazon.com/s3/), amongst others. [Amazon SQS](https://aws.amazon.com/sqs/) is a powerful messaging technology choice.

## Additional resources

- [AWS Well-Architected Framework](https://aws.amazon.com/architecture/well-architected/)
- [High Performance Computing Lens (amazon.com)](https://docs.aws.amazon.com/wellarchitected/latest/high-performance-computing-lens/high-performance-computing-lens.html)
