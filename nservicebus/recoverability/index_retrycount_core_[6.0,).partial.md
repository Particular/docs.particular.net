
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
| 3                | 5              | 24                      |

### Scale-out multiplier

NOTE: When scaled-out this behavior can be interpreted as if retries result in duplicates. Although this behavior can result in excessive retries no duplicate messages are created. Ensure that logging uses unique identifiers for each instance.

If an endpoint is scaled-out the number of attempts increases if instance are retrieving messages from the same queue and the transport does not have a native delivery counter.

Affected transports:

- Azure Storage Queues
- SQL Server
- RabbitMQ
- Amazon SQS
- MSMQ (only if running multiple instance on the same machine)

Unaffected transports:

- Azure Service Bus
- Azure Service Bus Legacy

Azure Service Bus transports use a native delivery counter for immediate retries which guarantees that the retry number is the same regardless if the endpoint is scaled out.


The number of instances act as a multiplier for the maximum number of attempts.

```txt
Mininum Attempts = (ImmediateRetries:NumberOfRetries + 1) * (DelayedRetries:NumberOfRetries + 1)
Maximum Attempts = MininumAttempts * NumberOfInstances
```

Example:

When taking the default values for immediate (5) and delayed retries (3) and 5 instances the total number of attempts will be a minumum of (5+1)*(3+1)=24 attempts and a maximum of 120.

