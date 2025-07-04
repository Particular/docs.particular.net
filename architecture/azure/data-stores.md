---
title: Azure data stores
summary: Gives an overview of the data storage options offered by Azure and how to use them with the Particular Service Platform
reviewed: 2025-07-03
callsToAction: ['solution-architect', 'poc-help']
---

Azure offers [several data stores](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/data-store-decision-tree) which can be used with the Particular Service Platform.

## Azure Cosmos DB

[Azure Cosmos DB](https://learn.microsoft.com/en-us/azure/cosmos-db/introduction) is a globally distributed, multi-model, managed database service. It offers high availability, low latency, and automatic scaling to handle massive workloads across various data models, including document, key-value, graph, and column-family. Cosmos DB relies on partitioning to achieve high performance and scalability. When working with Cosmos DB, it is important to be familiar with the [partitioning model](https://learn.microsoft.com/en-us/azure/cosmos-db/partitioning-overview).

:heavy_plus_sign: Pros:

- Built and optimized for scalability
- Multi-region support for high availability deployment
- Supports multiple [data models](https://learn.microsoft.com/en-us/azure/cosmos-db/choose-api)
- Fully managed
- [Cosmos DB Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator) can be used for local development and testing

:heavy_minus_sign: Cons:

- Atomic transactions only within the same partition
- Careful partition planning is required
- Fairly expensive

[**Try the NServiceBus Cosmos DB sample →**](/samples/cosmosdb/simple/)

> [!NOTE]
> Cosmos DB offers _serverless_ and _provisioned throughput_ pricing models. When using _provisioned throughput_, Cosmos DB rejects further requests after the reserved Request Units are used. The [Cosmos DB pricing model documentation](https://learn.microsoft.com/en-us/azure/cosmos-db/how-pricing-works) has further information.

## Azure Table Storage

[Azure Table Storage](https://learn.microsoft.com/en-us/azure/storage/tables/table-storage-overview) is a NoSQL managed database service. It is a cost-effective and scalable solution for storing semi-structured and structured data, using a key-value pair schema. Designed for fast and efficient data retrieval, it is suitable for applications that require high-performance read and write operations, especially for large-scale, low-latency data storage and retrieval needs.

:heavy_plus_sign: Pros:

- Cost-effective
- Partitioning is managed by the service
- [Azurite emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite) can be used for local development and testing

:heavy_minus_sign: Cons:

- Limited number of columns and single entity (row) size
- Limited query capabilities
- Multi-document transactions are limited to the same partition
- No built-in support for complicated relationships

[**Try the NServiceBus Azure Table Storage sample →**](/samples/azure/azure-table/simple/)

> [!NOTE]
> Applications built for Table Storage are compatible with Cosmos DB. See [this table](https://learn.microsoft.com/en-us/azure/cosmos-db/table/support) for a more in-depth comparison.

## Azure SQL Database

[Azure SQL Database](https://azure.microsoft.com/en-us/products/azure-sql/database/) is a fully managed database service based on the [Microsoft SQL Server database engine](https://learn.microsoft.com/en-us/sql/sql-server/?view=sql-server-ver16).

:heavy_plus_sign: Pros:

- Fully managed
- Rich built-in scaling options
- Automatic backups and point-in-time restores
- Active Geo-replication and zone-redundancy
- Different pricing tiers for more cost control

:heavy_minus_sign: Cons:

- Migrating existing SQL server instances might be challenging

Azure SQL Database is supported via the [NServiceBus SQL persistence package](/persistence/sql/).

[**Try the NServiceBus SQL persistence sample →**](/samples/sql-persistence/simple/)

## Azure SQL Managed Instance

[Azure SQL Managed Instance](https://azure.microsoft.com/en-us/products/azure-sql/managed-instance) is a fully managed database service that provides a near-complete compatibility with the [Microsoft SQL Server database engine](https://learn.microsoft.com/en-us/sql/sql-server/?view=sql-server-ver16). It offers a managed solution for migrating and modernizing existing SQL Server applications with minimal code changes.

:heavy_plus_sign: Pros:

- High degree of compatibility with SQL Server
- Supports cross-database queries
- Predictable performance for large databases due to isolation
- More database configuration options compared to Azure SQL Database

:heavy_minus_sign: Cons:

- Higher costs compared to manually managed database servers or Azure SQL Database

Azure SQL Managed Instances is supported via the [NServiceBus SQL persistence package](/persistence/sql/).

[**Try the NServiceBus SQL persistence sample →**](/samples/sql-persistence/simple/)

## Other data store options

Azure provides further relational database services compatible with NServiceBus, e.g. [Azure Database for MySQL](https://azure.microsoft.com/en-us/products/mysql) and [Azure Database for PostgreSQL](https://azure.microsoft.com/en-us/products/postgresql).

While the presented storage options provide the best experience and integration with Azure, other storage options are available and may be valid options depending on the specific requirements. [Explore all supported data stores](/persistence/#supported-persisters).

## Choosing a data store

The following factors should be considered when choosing a data store:

### Existing infrastructure

Using existing infrastructure and know-how can significantly speed up the development process and lower the risks associated with using unfamiliar data store technologies. Unless the existing infrastructure and technologies do not meet the architectural requirements, they are a good default choice.

### Compatibility

When migrating or modernizing existing applications, data stores that are compatible with currently used protocols and languages simplify the switch to cloud-based services significantly. For example, a data store may understand PostgreSQL, SQL Server, and MongoDB dialects which allow applications using any of these dialects to connect with minimal changes required.

### Transactions and consistency model

Transaction capabilities can vary considerably between different data stores. Some only provide transactional consistency within the same partition or have limited transaction isolation levels. It is important to consider whether [eventual consistency](https://en.wikipedia.org/wiki/Eventual_consistency) is acceptable or [ACID](https://en.wikipedia.org/wiki/ACID) guarantees are required.

### Storage model

Data stores may have [different storage models](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/data-store-overview), optimized for different use cases. Data access patterns, the type of information to be stored, and relations between data should be considered when choosing the most appropriate storage model.

### Scalability

It may be important to consider how easily a data store's capacity and performance can be scaled up when required. Scaling requirements may also depend on the expected access patterns (e.g. the number of reads vs writes) or availability goals.

### Legal and compliance

Sometimes, data may be subject to legal requirements (e.g. data privacy) that require additional consideration of how a data store operates to ensure compliance. See the [Azure compliance offerings](https://learn.microsoft.com/en-us/compliance/regulatory/offering-home) for more detailed information on specific national, regional, and industry-specific regulations.

### Costs

Data stores can have significantly different pricing models. Charges may be based on a number of factors such as storage size, read/write operations, data transfer, or higher consistency guarantees. Some serverless pricing models may be optimized for when data is accessed infrequently or in bursts. The licensing model for a specific data store may also significantly impact the overall cost.

### Portability

Some data stores are only available in the cloud, with no on-premises equivalent. This may affect hosting, CI, testing and development. Some data stores support open standards or protocols which allows the use of on-premises or local data stores in those scenarios. For example, Cosmos DB supports PostgreSQL and MongoDB dialects.

### Security

Data stores offer various approaches to securing data using authentication and authorization. This may be provided by the data store itself or the data store may integrate with third party security services such as Azure IAM.

## Additional resources

- [Azure Architecture Center: Criteria for choosing a data store](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/data-store-considerations)
- [What is Azure SQL Database?](https://learn.microsoft.com/en-us/azure/azure-sql/database/sql-database-paas-overview?view=azuresql)
- [What is Azure SQL Managed Instance?](https://learn.microsoft.com/en-us/azure/azure-sql/managed-instance/sql-managed-instance-paas-overview?view=azuresql)
