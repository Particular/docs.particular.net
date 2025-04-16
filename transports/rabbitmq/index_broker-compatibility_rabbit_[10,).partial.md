The transport is compatible with RabbitMQ broker version 3.10.0 or higher.

The `stream_queue` and `quorum_queue` [feature flags](https://www.rabbitmq.com/feature-flags.html) must be enabled because the [delay infrastructure](delayed-delivery.md) requires [at-least-once dead lettering](https://blog.rabbitmq.com/posts/2022/03/at-least-once-dead-lettering/).

The [RabbitMQ management plugin](https://www.rabbitmq.com/docs/management) must be enabled, and the plugin's [statistics and metrics collection must not be disabled](https://www.rabbitmq.com/docs/management#disable-stats). The port that the management API is using needs to be accessible by the transport. The default port is `15672` for HTTP and `15671` for HTTPS. See [Configuring RabbitMQ management API access](connection-settings.md#configuring-rabbitmq-management-api-access) for configuration options.

The broker requirements can be verified with the [`delays verify`](operations-scripting.md#delays-verify) command provided by the command line tool.
