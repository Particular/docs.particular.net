
The total number of possible retries can be calculated with the following formula

```txt
Attempts = (ImmediateRetries:NumberOfRetries + 1) * (DelayedRetries:NumberOfRetries + 1)
```

So for example given a variety of Immediate and Delayed here are the resultant possible attempts.

| ImmediateRetries | DelayedRetries | Total possible attempts |
|------------------|----------------|-------------------------|
| 0                | 0              | 1                       |
| 0                | 1              | 2                       |
| 0                | 2              | 3                       |
| 0                | 3              | 4                       |
| 1                | 0              | 2                       |
| 1                | 1              | 4                       |
| 1                | 1              | 4                       |
| 1                | 2              | 6                       |
| 1                | 3              | 8                       |
| 2                | 1              | 6                       |
| 2                | 2              | 9                       |
| 3                | 1              | 8                       |

### Scale-out multiplier

If an endpoint is scaled-out the number of attempts increases if instance are retrieving messages from the same queue and the transport does not have a native delivery counter.

Affected transports:

- Azure Storage Queues
- SQL Server
- RabbitMQ
- Amazon SQS
- Learning (Only if running multiple instance on the same machine which is not advised)
- MSMQ (Only if running multiple instance on the same machine which is not advised)

Unaffected transports:

- Azure Service Bus (native delivery counter)
- Azure Service Bus Legacy (native delivery counter)

The number of instances act as a multiplier for the maximum number of attempts.

```txt
Mininum Attempts = (ImmediateRetries:NumberOfRetries + 1) * (DelayedRetries:NumberOfRetries + 1)
Maximum Attempts = MininumAttempts * NumberOfInstances
```
