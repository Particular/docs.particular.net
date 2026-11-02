## Configuring RabbitMQ management API access

The transport uses the RabbitMQ management API to verify broker requirements and enable [delivery limit validation](#delivery-limit-validation).

The [RabbitMQ management plugin](https://www.rabbitmq.com/docs/management) must be enabled, and the plugin's [statistics and metrics collection must not be disabled](https://www.rabbitmq.com/docs/management#disable-stats). The port that the management API is using needs to be accessible by the transport. The default port is `15672` for HTTP and `15671` for HTTPS.

By default, the transport will infer the settings to use to access the management API from the connection string. If the broker configuration requires different settings to access the management API, custom settings can be provided:

snippet: rabbitmq-management-api-configuration

There are also overloads to specify just the URL or just the credentials and continue to rely on the connection string for the unspecified settings.

### Minimum management API permissions

The transport reads the broker version, the enabled feature flags, and the details of each queue it consumes from. A management API user with the read-only [`management` tag](https://www.rabbitmq.com/docs/management#permissions) is enough for these requests, as long as the user also has permissions on the virtual host.

The only write the transport makes is to create a delivery limit policy for a quorum queue on RabbitMQ 4.0 and above, which requires the `policymaker` tag. When an administrator creates the [delivery limit policies](#delivery-limit-validation-creating-delivery-limit-policies-in-advance) in advance, the transport finds the expected delivery limit already in place and does not attempt to create a policy. In this case, endpoints can run with a `management`-only user while all broker requirement checks and delivery limit validation remain enabled.

> [!NOTE]
> Some older RabbitMQ versions, such as 3.11, require the `administrator` tag to read the feature flags. On RabbitMQ 3.13 and 4.x, the `management` tag is sufficient.

### Disabling broker requirement checks

Starting in version 10.1, it is possible to disable the broker requirement checks that the transport uses the management API to make. This should only be done in extreme circumstances when it is not possible to give the transport any access to the management API. If the concern is granting the transport permission to make changes to the broker, [create the delivery limit policies in advance](#delivery-limit-validation-creating-delivery-limit-policies-in-advance) and use a user with [minimum management API permissions](#configuring-rabbitmq-management-api-access-minimum-management-api-permissions) instead. This keeps the checks enabled, so the transport still detects a policy that is later removed or changed.

snippet: rabbitmq-disable-broker-requirement-checks

> [!CAUTION]
> Using a broker that does not meet all of the requirements can result in message loss or other incorrect operation, so disabling checks is not recommended.
