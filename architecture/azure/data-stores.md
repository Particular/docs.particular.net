---
title: Azure data stores
summary:
reviewed: 2023-07-18
---

The most common NServiceBus compatible storage options on Azure are:

## Azure CosmosDB

Azure Cosmos DB is a globally distributed, multi-model, managed database service. It offers high availability, low latency, and automatic scaling to handle massive workloads across various data models, including document, key-value, graph, and column-family. CosmosDB relies on partitioning to achieve high performance and scalability. When working with CosmosDB, make sure become familiar with the [partitioning model](https://learn.microsoft.com/en-us/azure/cosmos-db/partitioning-overview).

:heavy_plus_sign: Pros:
- Built and optimized for scalability
- Multi-region support for high availability deployments
- Supports multiple [data models](https://learn.microsoft.com/en-us/azure/cosmos-db/choose-api)
- Fully managed

:heavy_minus_sign: Cons:
- Fairly expensive
- Atomic transactions only within the same partition
- Partitioning limitations requires careful planning
- No local/on-premises option or emulator

Note: CosmosDB offers _serverless_ and _provisioned throughput_ pricing models. When using _provisioned throughput_, exceeding the reserved Request Units will lead to CosmosDB rejecting further requests. Review the [CosmosDB pricing model documentation](https://learn.microsoft.com/en-us/azure/cosmos-db/how-pricing-works) carefully for further information.

### Additional content

* [Azure CosmosDB persistence documentation](/persistence/cosmosdb/)
* [Try the NServiceBus CosmosDB sample](/samples/cosmosdb/simple/)


## Azure Table Storage

Azure Table Storage is a NoSQL managed database service. It offers a cost-effective and scalable solution for storing semi-structured and structured data, using a key-value pair schema. Designed for fast and efficient data retrieval, it is suitable for applications that require high-performance read and write operations, especially for large-scale, low-latency data storage and retrieval needs.

:heavy_plus_sign: Pros:
- Cost-effective
- Partioning is managed by the service

:heavy_minus_sign: Cons:
- Limited query capabilities
- multi-document transactions limited to the same partition
- Not suitable to model complicated relationships
- No local/on-premises option, but [Azurite emulator can be used for local development](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite)

Note: Applications built for Table Storage are compatible with CosmosDB. See [this table](https://learn.microsoft.com/en-us/azure/cosmos-db/table/support) for a more in-depth comparison.

### Additional content

* [Azure Table Storage persistence documentation](/persistence/azure-table/)
* [Try the NServiceBus Azure Table Storage sample](/samples/azure/azure-table/simple/)

## Azure SQL

* Azure SQL using [NServiceBus SQL Persistence](/persistence/sql/)

## Azure SQL Managed Instances

* Azure SQL Managed Instances using [NServiceBus SQL Persistence](/persistence/sql/)

## Other storage options

[Explore all supported data storages](/persistence/#supported-persisters)

## Choosing a storage

The following categories should be considered when chosing a storage option:

### Existing Infrastructure

Using existing infrastructure and knowhow can severly speed up the development process and mitigate risks of unfamiliar storage technologies. Unless the existing infrastructure and technologies aren't capable to meet the required architectural needs, they are a good default choice.

### Compatibility

Storage option that are compatible with existing protocols and languages (e.g. PostgreSQL dialect)
e.g. compatibility with Postgre, sql, mongo, etc.

### Transactions, Consistency Model

Transaction capabilities can heavily vary between different storage options. Some storage options might only provide transactional consistency within the same partition or have limited transaction isolation levels. Consider if eventual consistency is acceptable or of ACID guarantees are necessary.

### Storage Model

Different database storage models are optimized for different use cases. Review the expected access patterns to the database and the type of information and data relations that are being stored. Some common storage models are: Relational, Key/Value, Document DB, Graph DBs, BigData/Analytics, Column-family, Time-series, Object/blob storage

### Scalability

How easy can the data storage capacity and performance be scaled up if needed? The scaling requirements might also depend on the expected access patterns (e.g. reads vs writes) or availability goals.

### Legal and Compliance

Sometimes, the data stored might be subject to legal requirements (e.g. data privacy) that require additional consideration to ensure compliance.

### Costs

Databases can have different pricing models, charging not only for storage but also read and write operations, data transfer, or higher consistency guarantees. On the other hand, serverless pricing models might be leveraged to optimize cloud spending when data is accessed infrequently or in bursts. Also take into account the database licensing model which can severly impact the costs.

### Portability

Some managed database services are only available in the cloud without the possibility to use them in offline scenarios. Database portability may also affect the local development and CI experience. Some database services support open standards or protocols. Applications can then transparently switch the actual database engine without code changes to adjust for the necessary environments, e.g. CosmosDB also provides MongoDB and PostgreSQL compatibility.

### Security

Storage options offer various approaches to securing data access using authorization and authentication features. This might be completely handled by the database product/service or integrate with existing security models of cloud providers like Azure IAM.


## Additional content

* [Azure Architecture Center: Criteria for choosing a data store](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/data-store-considerations)


