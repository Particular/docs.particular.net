The short answer is **no**. The longer answer is: Queues are remote, instead of local, and this has several implications.

 * A message has to cross the network boundaries before it is persisted, this implies that it is subject to all kinds of network related issues like latency, timeouts, connection loss, network partitioning etc.
 * Remote queues do not play along in transactions, as transactions are very brittle because of the possible network issues mentioned in the previous point, but also because they would require server-side locks to function properly and allowing anyone to take unbound locks on a service is a very good way to get a denial of service situation. Hence Azure services typically don't allow transactions.

For more details refer to the [Transactions in Azure](/nservicebus/azure/understanding-transactionality-in-azure.md) article.
