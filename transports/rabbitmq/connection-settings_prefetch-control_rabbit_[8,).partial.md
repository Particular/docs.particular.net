## Controlling the prefetch count

When consuming messages from the broker, throughput can be improved by having the consumer [prefetch](https://www.rabbitmq.com/consumer-prefetch.html) additional messages. By default, the prefetch count is calculated by multiplying [maximum concurrency](/nservicebus/operations/tuning.md#tuning-concurrency) by the prefetch multiplier of 3. The default prefetch count calculation can be overridden with a custom algorithm. The configured concurrency setting is passed as an input to the calculation function.

snippet: rabbitmq-config-prefetch-multiplier

Alternatively, the whole calculation can be overridden by setting the prefetch count directly using the following:

snippet: rabbitmq-config-prefetch-count

NOTE: If the configured value is less than the maximum concurrency, the prefetch count will be set to the maximum concurrency value instead.
