Amazon DynamoDB supports two read models: strongly consistent and eventually consistent. By default, the AWS SDK uses eventual consistency, which does not guarantee that the latest write is visible at the time of the read, which may result in a stale read. While this model is sufficient for many applications, it can introduce correctness issues when used in conjunction with concurrency control mechanisms, such as optimistic locking.

To ensure safety and accuracy, the NServiceBus DynamoDB saga persister has historically defaulted to strongly consistent reads. This ensures that the most up-to-date saga data is always retrieved, reducing the chance of version mismatches or conflicting writes. However, this approach comes with a cost: strongly consistent reads consume twice the read capacity units compared to eventually consistent reads.

To provide more flexibility, Version 2.1 of the persistence introduces a new saga configuration option that allows to opt in to eventual consistent reads for sagas when using optimistic concurrency:

snippet: DynamoDBSagaEventualConsistentReads

When enabled, this setting allows reads to be performed using DynamoDB’s eventual consistency model, reducing read costs—particularly beneficial in high-throughput or cost-sensitive environments where occasional retries are acceptable.