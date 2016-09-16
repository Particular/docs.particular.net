## Controlling the prefetch count

When consuming messages from the broker, throughput can be improved by having the consumer [prefetch](http://www.rabbitmq.com/consumer-prefetch.html) additional messages.
The prefetch count is calculated by setting it to a multiple of the [maximum concurrency](/nservicebus/operations/tuning.md#tuning-concurrency) value. The multiplier in the calculation is set to 3 by default, but it can be changed by using the following:

snippet: rabbitmq-config-prefetch-multiplier

Alternatively, the calculation can be overridden by setting the prefetch count directly using the following:

snippet: rabbitmq-config-prefetch-count

NOTE: If the configured value is less than the maximum concurrency, the prefetch count will be set to the maximum concurrency value instead.
