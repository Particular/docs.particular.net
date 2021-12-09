RavenDB version 5.2 and higher support cluster-wide transactions.

Cluster-wide transactions should be enabled when the database is replicated accross multiple nodes in a cluster. If the database only resides on a single node, cluster-wide transactions are not required.

To enable cluster-wide transaction support use:

snippet: ravendb-persistence-cluster-wide-transactions
