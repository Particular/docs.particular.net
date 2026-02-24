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

### Amazon Aurora DSQL

[Amazon Aurora DSQL](https://docs.aws.amazon.com/aurora-dsql/latest/userguide/what-is-aurora-dsql.html) is a distributed SQL database service that provides active-active multi-region replication with strong consistency. It offers PostgreSQL compatibility while enabling globally distributed applications to read and write with low latency across regions.

:heavy_plus_sign: Pros:

- Active-active multi-region deployments with strong consistency
- PostgreSQL-compatible with zero latency reads in each region
- Automatic sharding and scaling without application awareness
- High availability with automatic failover

:heavy_minus_sign: Cons:

- Newer service with evolving feature set
- Multi-region cost implications
- Less suitable for single-region workloads

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

## Choosing a data store

The following factors should be considered when choosing a data store:

### Existing infrastructure

Using existing infrastructure and know-how can significantly speed up the development process and lower the risks associated with using unfamiliar data store technologies. They are a good default choice, unless the existing infrastructure and technologies do not meet the architectural requirements.

### Compatibility

When migrating or modernizing existing applications, data stores that are compatible with currently used protocols and languages significantly simplify the switch to cloud-based services. For example, a data store may understand PostgreSQL, MySQL, or MongoDB dialects which allow applications using any of these dialects to connect with minimal changes required.

### Transactions and consistency model

Transaction capabilities can vary considerably between different data stores. Some only provide transactional consistency within the same partition or have limited transaction isolation levels. It is important to consider whether [eventual consistency](https://en.wikipedia.org/wiki/Eventual_consistency) is acceptable or [ACID](https://en.wikipedia.org/wiki/ACID) guarantees are required.

### Storage model

Data stores may have different storage models, optimized for different use cases. Data access patterns, the type of information to be stored, and relations between data should be considered when choosing the most appropriate storage model.

### Scalability

It may be important to consider how easily a data store's capacity and performance can be scaled up when required. Scaling requirements may also depend on the expected access patterns (e.g. the number of reads vs writes) or availability goals.

### Legal and compliance

Sometimes, data may be subject to legal requirements (e.g. data privacy) that require additional consideration of how a data store operates to ensure compliance. See the [AWS compliance offerings](https://aws.amazon.com/compliance/) for more detailed information on specific national, regional, and industry-specific regulations.

### Costs

Data stores can have significantly different pricing models. Charges may be based on a number of factors such as storage size, read/write operations, data transfer, or higher consistency guarantees. Some serverless pricing models may be optimized for when data is accessed infrequently or in bursts.

### Portability

Some data stores are only available in the cloud, with no on-premises equivalent. This may affect hosting, CI, testing and development. Some data stores support open standards or protocols, allowing for the use of on-premises or local data stores.

### Security

Data stores offer various approaches to securing data using authentication and authorization. This may be provided by the data store itself or the data store may integrate with third party security services such as AWS IAM.
