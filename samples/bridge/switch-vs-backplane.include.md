## Switch vs Backplane

Both [Switch](/samples/bridge/sql-switch) and [Backplane](/samples/bridge/backplane) approaches can be used replace the deprecated multi-instance mode in connecting endpoints that use different SQL Server databases. The following table contains a side-by-side comparison of both approaches

|Switch|Backplane
|----------------
|Single switch for the entire solution|Bridge-per-database, can be co-hosted in a single process
|Requires DTC to ensure *exactly-once* processing|*Exactly-once* processing through de-duplication
|All SQL Server instances must be in the same network|Each SQL Server instance can be in separate network or even data centre
|Centralized forwarding configuration|Distributed forwarding configuration

The Backplane approach, while more complex in terms of deployment, provides more flexibility e.g. some databases might be on-premise while others might be in the cloud.


### Throughput

Both approaches can be used to increase the throughput of the entire system when performance of a single SQL Server instance becomes a bottle neck. The key to thing when using the Switch or Backplane for performance reasons is partitioning. When done wrong, it can have the opposite effect and decrease the overall throughput.

To correctly partition the system when using Switch or Backplane first cluster the endpoints based on the volume of messages exchanged. The more messages endpoint exchange, the closer they are. If all endpoints form a single cluster Switch or Backplane won't help. In a healthy system, however, there will be several clusters of endpoints of highly coupled endpoints. Assign each cluster its own instance of SQL Server. Use Switch or Backplane to connect the clusters.