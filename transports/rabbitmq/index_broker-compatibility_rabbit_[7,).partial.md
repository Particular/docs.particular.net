This version of the transport is compatible with RabbitMQ broker version 3.10.0 or higher either self-hosted or running on [Amazon MQ](https://aws.amazon.com/amazon-mq/) or [CloudAMQP](https://www.cloudamqp.com/). For previous versions of RabbitMQ it is recommended to upgrade RabbitMQ or select an earlier version of the transport.

 The `stream_queue` and `quorum_queue` [feature flags](https://www.rabbitmq.com/feature-flags.html) must be enabled because the [delay infrastructure](delayed-delivery.md) requires [at-least once dead lettering](https://blog.rabbitmq.com/posts/2022/03/at-least-once-dead-lettering/).

 The broker requirements can be verified with the [`delays verify`](operations-scripting.md#delays-verify) command provided by the command line tool.
