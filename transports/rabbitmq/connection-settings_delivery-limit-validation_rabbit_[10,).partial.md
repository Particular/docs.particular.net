## Delivery limit validation

Quorum queues have a delivery limit setting that controls how many times the broker will redeliver a message before it considers the message poison and deletes the message. While the transport does use the `x-delivery-count` header that is part of this feature, it requires the delivery limit to be effectively unlimited in order to ensure that a message is not unexpectedly deleted by the broker while retrying the message based on the endpoint's [recoverability](/nservicebus/recoverability/) settings.

The transport considers a delivery limit of `100000` to be unlimited. Any other delivery limit set by queue arguments, user policies, or operator policies, including `-1`, causes validation to fail.

In RabbitMQ version 4.0 and above, the default delivery limit has been changed from unlimited to `20`. Because of this change, the transport validates that each quorum queue the endpoint consumes from has a delivery limit of `100000`. Classic queues do not have a delivery limit, so they are not validated.

As part of this validation process, the transport will attempt to use the management API to create a policy that applies to the queue to set the delivery limit to `100000`. This policy will only be created if the transport does not detect another policy already applying to the queue. Policies are only created on RabbitMQ 4.0 and above. On earlier versions, the transport only reads the queue details.

If the transport cannot validate that the queue has the expected delivery limit and cannot create a policy to change the setting, the validation will fail and prevent the endpoint from starting.

> [!NOTE]
> The credentials used to access the RabbitMQ management API require [policymaker permissions](https://www.rabbitmq.com/docs/management#permissions) only if the transport has to create the delivery limit policies itself. If the policies are [created in advance](#delivery-limit-validation-creating-delivery-limit-policies-in-advance), the transport only reads from the management API, and a user with the `management` tag is sufficient.

### Creating delivery limit policies in advance

Instead of granting endpoints permission to create policies, an administrator can create a policy that covers the endpoint queues before the endpoints are started:

```bash
rabbitmqctl set_policy -p <vhost> <policyName> '<pattern>' '{"delivery-limit":100000}' --apply-to quorum_queues
```

- Use `-p <vhost>` to create the policy in the virtual host that the endpoints use. Without it, the policy is created in the default `/` virtual host.
- The pattern must match every queue the endpoints consume from, for example `'^(Sales|Billing|Shipping)$'`.
- The `--apply-to` value is `quorum_queues`, with an underscore.
- Do not use a policy name that starts with `nsb.` and ends with `.delivery-limit`. The transport treats policies with names in this format as its own.

RabbitMQ applies only one policy to a queue, the matching policy with the highest priority. If another policy already applies to the endpoint queues, add `"delivery-limit":100000` to the definition of that policy instead of creating a separate one. The transport will not create its own policy for a queue that already has a policy applied.

Since validation remains enabled, the transport still detects when a policy is later removed or changed, and prevents the endpoint from starting until the delivery limit is corrected.

Alternatively, the [`queue validate-delivery-limit`](operations-scripting.md#queue-validate-delivery-limit) command can validate each queue and create a policy where needed. It can run from a deployment pipeline that has `policymaker` credentials, separate from the hosts that run the endpoints.

### Resolving validation failures

If the endpoint will not start because delivery limit validation is failing, manual intervention is required. If there is already a policy applied to the queue, it either needs to be updated to set the delivery limit to `100000`, or the policy needs to be removed from the queue entirely. Removing the policy will let the transport successfully create its policy the next time the endpoint is started, provided the management API credentials have `policymaker` permissions.

> [!WARNING]
> If it is not possible to allow the validation to pass, the `DoNotValidateDeliveryLimits` configuration method can be used to skip the validation entirely. This is not recommended because it leaves the endpoint open to the possibility of message loss in the case where the number of immediate retries is greater than the delivery limit of the queue.