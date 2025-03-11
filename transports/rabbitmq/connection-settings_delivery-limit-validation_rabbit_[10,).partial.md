## Delivery limit validation

Quorum queues have a delivery limit setting that controls how many times the broker will redeliver a message before it considers the message poison and deletes the message. While the transport does use the `x-delivery-count` header that is part of this feature, it requires the delivery limit to be set to `unlimited` in order to ensure that a message is not unexpectedly deleted by the broker while retrying the message based on the endpoint's [recoverability](/nservicebus/recoverability/) settings.

In RabbitMQ version 4.0 and above, the default delivery limit has been changed from `unlimited` to `20`. Because of this change, the transport now validates that queue being consumed by the endpoint has been properly configured to have an `unlimited` delivery limit.

As part of this validation process, the transport will attempt to use the management API to create a policy that applies to the queue to set the delivery limit to `unlimited`. This policy will only be created if the transport does not detect another policy already applying to a queue.

If the transport cannot validate that the queue has an `unlimited` delivery limit and cannot create a policy to change the setting, the validation will fail and prevent the endpoint from starting. 

> [!NOTE]
> The credentials used to access the RabbitMQ management API require [policymaker permissions](https://www.rabbitmq.com/docs/management#permissions) in order for the transport to be be able to create `unlimited` delivery limit policies.

If the endpoint will not start because delivery limit validation is failing, manual intervention is required. If there is already a policy applied to the queue, it either needs to be updated to include setting the delivery limit on the queue, or the policy needs to be removed from the queue entirely. Removing the policy will let the transport successfully create its policy the next time the endpoint is started.

> [!WARNING]
> If it not possible to allow the validation to pass, the `DoNotValidateDeliveryLimits` configuration method can be used to skip the validation entirely. This is not recommended because it leaves the endpoint open to the possibility of message loss in the case where the number of immediate retries is greater than the delivery limit of the queue.

The [`queue validate-delivery-limit`](operations-scripting.md#queue-validate-delivery-limit) command can also be used to validate the delivery limit of a queue.