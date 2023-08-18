---
title: Azure data stores
summary:
reviewed: 2023-07-18
---

Azure offers [several data stores](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/data-store-decision-tree) which can be used with the Particular Service Platform.

## Azure Cosmos DB

[Azure Cosmos DB](https://learn.microsoft.com/en-us/azure/cosmos-db/introduction) is a globally distributed, multi-model, managed database service. It offers high availability, low latency, and automatic scaling to handle massive workloads across various data models, including document, key-value, graph, and column-family. Cosmos DB relies on partitioning to achieve high performance and scalability. When working with Cosmos DB, it is important to be familiar with the [partitioning model](https://learn.microsoft.com/en-us/azure/cosmos-db/partitioning-overview).

:heavy_plus_sign: Pros:

- Built and optimized for scalability
- Multi-region support for high availability deployment
- Supports multiple [data models](https://learn.microsoft.com/en-us/azure/cosmos-db/choose-api)
- Fully managed

:heavy_minus_sign: Cons:

- Fairly expensive
- Atomic transactions only within the same partition
- Partitioning limitations require careful planning
- No local/on-premises option or emulator

Note: Cosmos DB offers _serverless_ and _provisioned throughput_ pricing models. When using _provisioned throughput_, Cosmos DB rejects further requests after the reserved Request Units are used. The [Cosmos DB pricing model documentation](https://learn.microsoft.com/en-us/azure/cosmos-db/how-pricing-works) has further information.

### Additional content

- [Azure Cosmos DB persistence documentation](/persistence/cosmosdb/)
- [Try the NServiceBus Cosmos DB sample](/samples/cosmosdb/simple/)

## Azure Table Storage

Azure Table Storage is a NoSQL managed database service. It is a cost-effective and scalable solution for storing semi-structured and structured data, using a key-value pair schema. Designed for fast and efficient data retrieval, it is suitable for applications that require high-performance read and write operations, especially for large-scale, low-latency data storage and retrieval needs.

:heavy_plus_sign: Pros:

- Cost-effective
- Partitioning is managed by the service

:heavy_minus_sign: Cons:

- Limited query capabilities
- Multi-document transactions are limited to the same partition
- Not suitable to model complicated relationships
- No local/on-premises option, but [Azurite emulator can be used for local development](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite)

Note: Applications built for Table Storage are compatible with Cosmos DB. See [this table](https://learn.microsoft.com/en-us/azure/cosmos-db/table/support) for a more in-depth comparison.

### Additional content

- [Azure Table Storage persistence documentation](/persistence/azure-table/)
- [Try the NServiceBus Azure Table Storage sample](/samples/azure/azure-table/simple/)

## Azure SQL

- Azure SQL using [NServiceBus SQL Persistence](/persistence/sql/)

## Azure SQL Managed Instances

- Azure SQL Managed Instances using [NServiceBus SQL Persistence](/persistence/sql/)

## Other data store options

[Explore all supported data stores](/persistence/#supported-persisters).

## Choosing a data store

The following factors should be considered when choosing a data store:

### Existing infrastructure

Using existing infrastructure and know-how can significantly speed up the development process and lower the risks associated with using unfamiliar data store technologies. Unless the existing infrastructure and technologies do not meet the architectural requirements, they are a good default choice.

### Compatibility

Data stores that are compatible with existing protocols and languages. For example, a data store may understand PostgreSQL, SQL Server, and MongoDB dialects.

### Transactions and consistency model

Transaction capabilities can vary considerably between different data stores. Some only provide transactional consistency within the same partition or have limited transaction isolation levels. It is important to consider whether [eventual consistency](https://en.wikipedia.org/wiki/Eventual_consistency) is acceptable or [ACID](https://en.wikipedia.org/wiki/ACID) guarantees are required.

### Storage model

Data stores may have [different storage models](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/data-store-overview), optimized for different use cases. Data access patterns, the type of information to be stored, and relations between data should be considered when choosing the most appropriate storage model.

### Scalability

It may be important to consider how easily a data store's capacity and performance can be scaled up when required. Scaling requirements may also depend on the expected access patterns (e.g. the number of reads vs writes) or availability goals.

### Legal and compliance

Sometimes, data may be subject to legal requirements (e.g. data privacy) that require additional consideration of how a data store operates to ensure compliance.

### Costs

Data stores can have significantly different pricing models. Charges may be based on a number of factors such as storage size, read/write operations, data transfer, or higher consistency guarantees. Some serverless pricing models may be optimized for when data is accessed infrequently or in bursts. The licensing model for a specific data store may also significantly impact the overall cost.

### Portability

Some data stores are only available in the cloud, with no on-premises equivalent. This may affect hosting, CI, testing and development. Some data stores support open standards or protocols which allows the use of on-premises or local data stores in those scenarios. For example, Cosmos DB supports PostgreSQL and MongoDB dialects.

### Security

Data stores offer various approaches to securing data using authentication and authorization. This may be provided by the data store itself or the data store may integrate with third party security services such as Azure IAM.

## Additional content

- [Azure Architecture Center: Criteria for choosing a data store](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/data-store-considerations)