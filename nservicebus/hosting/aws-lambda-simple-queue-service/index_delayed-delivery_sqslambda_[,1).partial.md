### Delayed Retries

[Delayed retries](/nservicebus/recoverability/configure-delayed-retries.md) are disabled by default when using AWS Lambdas. Delayed retries may be enabled as follows:

snippet: aws-delayed-retries

If the accumulated time increase is expected to be greater than [15 minutes](/transports/sqs/delayed-delivery.md#enable-unrestricted-delayed-delivery), `UnrestrictedDurationDelayedDelivery` must be enabled:

snippet: aws-unrestricted-delayed-delivery

Note: Automatic creation of the required queues for unrestricted delayed delivery is not supported. The creation of the required queues may be scripted using the [CLI](/transports/sqs/delayed-delivery.md#enable-unrestricted-delayed-delivery-manual-fifo-queue-creation).
