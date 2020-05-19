SQL Server transport in Azure SQL

Using SQL Server with Azure SQL allows building messaging systems that provide exactly-once message processing guarantees. The same instance of Azure SQL is used both as application data store and as messaging infrastructure. This article discussed throughput characteristic of SQL Server transport in Azure SQL.

Current version:

Version X of SQL Server transport uses clustered index based on an identity column (sequence number). This structure is prone to high contention on the first page of the table. The contention causes severe issues in high-throughput scenarios, especially with Azure SQL. For this reason we recommend upgrading to Y.

Next version:

Prior to X, SQL Server transport used clustered index based on an identity column (sequence number). This structure was prone to high contention on the first page of the table. The contention caused severe issues in high-throughput scenarios, especially with Azure SQL. Version Y changed the structure of the index which is now based on a random unique ID.


### Testing methodology

A small set of stress tests has been done to estimate the maximum throughput of a single queue. It yielded following results:

8 CPU 600 msg/s
16 vCPU 800 msg/s

While these results are interesting, they are not representative of a real-life scenario. In most deployments there are at least 10s or even 100s of endpoints, each processing messages with much smaller throughput.

A set of load tests were designed to measure the CPU usage while processing messages coming at a constant rate. Messages were processed by a chain of 15 endpoints. Each endpoint in the chain was forwarding messages to the next one.

Here are the CPU usage values:

2 vCPU, 15 endpoints

25 msg/s - 30% IO, 35% CPU
50 msg/s - 65% IO, 65% CPU
60 msg/s - 80% IO, 80% CPU

4 vCPU

25 msg/s - 15% IO, 15% CPU
50 msg/s - 35% IO, 35% CPU
75 msg/s - 50% IO, 60% CPU
90 msg/s - 60% IO, 85% CPU (max)

8 vCPU

25 msg/s - 8% IO, 8% CPU
50 msg/s - 17% IO, 17% CPU
75 msg/s - 25% IO, 30% CPU
100 msg/s - 32% IO, 50% CPU
125 msg/s - 40% IO, 80% CPU

16 vCPU

25 msg/s - 9% IO, 4% CPU
50 msg/s - 17% IO, 9% CPU
75 msg/s - 25% IO, 16% CPU
100 msg/s - 33% IO, 25% CPU
125 msg/s - 40-50% IO, 35-45% CPU

### Discussion

The total system throughput for different per-endpoint throughput are following

25 msg/s - 375 msg/s
50 msg/s - 750 msg/s
60 msg/s - 900 msg/s
75 msg/s - 1125 msg/s
100 msg/s - 1500 msg/s
125 msg/s - 1875 msg/s

Because in these scenarios the database is used both as a transport and as a data store for the application state, it is fair to assume that the CPU usage of the transport should not exceed 35%, on average. That means that for a system consisting of 15 endpoints the recommended vCPU count as a function of total throughput is following

0-375 msg/s - 2 vCPU
375-750 msg/s - 4 vCPU
750-1125 msg/s - 8 vCPU
1125-1500 msg/s - 16 vCPU

The exact values for a real system may vary but the table above should provide input for estimations. It is worth noting that the SQL Server transport scales better with number of endpoints than with throughput of each endpoint. That means that higher total throughput can be achieved with higher number of endpoints and lower per-endpoint throughput. In an extreme case, a 16 vCPU database can handle only around 800 messages with a single queue and well over 2000 messages is using 15 queues.

When designing Software-as-a-Service systems consider using separate databases and endpoint sets for groups of tenants rather than for vertical slices of a business flow. With this approach each database would support relatively high number (whole business flow end-to-end) of low-throughput endpoints (a portion of customer base) as opposed to low number of high-throughput endpoints.

When using using multiple instances of SQL Azure consider connecting them with [NServiceBus.Router](/nservicebus/router/). In order to ensure exactly-once message processing semantics, consider [Elastic Transactions](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-elastic-transactions-overview) in the message router.
