## Controlling the prefetch count

When consuming messages from the broker, throughput can be improved by having the consumer [prefetch](https://www.rabbitmq.com/consumer-prefetch.html) additional messages.
The prefetch count is calculated by multiplying [maximum concurrency](/nservicebus/operations/tuning.md#tuning-concurrency) by the prefetch multiplier. The default value of the multiplier is 3, but it can be changed by using the following:

snippet: rabbitmq-config-prefetch-multiplier

Alternatively, the whole calculation can be overridden by setting the prefetch count directly using the following:

snippet: rabbitmq-config-prefetch-count

NOTE: If the configured value is less than the maximum concurrency, the prefetch count will be set to the maximum concurrency value instead.
