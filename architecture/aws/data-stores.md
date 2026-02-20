---
title: AWS data stores
summary: Gives an overview of the data storage options offered by AWS and how to use them with the Particular Service Platform
reviewed: 2026-02-20
callsToAction: ['solution-architect', 'poc-help']
---

Amazon Web Services (AWS) offers a number of [data storage services](https://aws.amazon.com/getting-started/decision-guides/databases-on-aws-how-to-choose/) that integrate well with the Particular Service Platform.

## Amazon DynamoDB

[Amazon DynamoDB](https://aws.amazon.com/dynamodb/) is a fully managed, serverless, NoSQL database service. It is designed for high-performance and scalable applications, offering low-latency access to data regardless of the scale. DynamoDB supports both document and key-value data models and automatically scales to handle the throughput and storage requirements of applications. Features include automatic partitioning, backup and restore, as well as global replication for data distribution across multiple regions.

:heavy_plus_sign: Pros:

- Built and optimized for scalability
- Multi-region support for high availability scenarios
- Single-digit millisecond performance
- Fully managed

:heavy_minus_sign: Cons:
 
- Data modeling as well as [saga design](/nservicebus/sagas/concurrency.md#high-load-scenarios-redesign-the-sagas) can impact cost, making it difficult to plan financially
- Limited storage capacity for [individual items](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ServiceQuotas.html#limits-items)
- Queries and scans require indexes to be created

[**Try the NServiceBus DynamoDB sample →**](/samples/aws/dynamodb-simple/)

## Amazon Aurora

[Amazon Aurora](https://aws.amazon.com/rds/aurora/) is a relational database service, compatible with MySQL and PostgreSQL. It is designed to offer high performance, durability, and availability for database applications. Aurora uses a distributed and fault-tolerant architecture, providing automatic replication and failover capabilities to enhance data reliability and reduce downtime.

:heavy_plus_sign: Pros:

- High-performance and low-latency database operations
- Compatible with MySQL and PostgreSQL, allowing for easy migration of existing applications without major code modifications
- Can automatically scale both read and write operations to handle varying workloads
- Fully managed

:heavy_minus_sign: Cons:

- Likely to be more expensive than other options
- Only supports MySQL and PostgreSQL which could impact migration from other database systems

[**Try NServiceBus sagas with Aurora, SQS, and Lambda →**](/samples/aws/sagas-lambda-aurora/)

## Amazon RDS

[Amazon RDS](https://aws.amazon.com/rds/) (Relational Database Service) is a managed database service that supports multiple database engines such as MySQL, PostgreSQL, Oracle, Microsoft SQL Server, and others. It simplifies many database administration tasks, including backups, software patching, and scaling, allowing users to focus on application development. RDS provides a reliable and scalable solution with features like backup retention, automatic software patching, and multi-region deployments for high availability and fault tolerance.

:heavy_plus_sign: Pros:

- Fully managed
- Supports a variety of database engines
- Scales easily
- Supports high availability through Multi-AZ deployments

:heavy_minus_sign: Cons:

- Potential latency between primary instance and read replicas
- Can be more expensive than self-managed solutions for smaller applications

[**Try NServiceBus sagas with Aurora, SQS, and Lambda →**](/samples/aws/sagas-lambda-aurora/)
