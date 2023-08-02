The transport is compatible with RabbitMQ broker version 3.10.0 or higher either, self-hosted or running on [CloudAMQP](https://www.cloudamqp.com/).

 The `stream_queue` and `quorum_queue` [feature flags](https://www.rabbitmq.com/feature-flags.html) must be enabled because the [delay infrastructure](delayed-delivery.md) requires [at-least once dead lettering](https://blog.rabbitmq.com/posts/2022/03/at-least-once-dead-lettering/).

 The broker requirements can be verified with the [`delays verify`](operations-scripting.md#delays-verify) command provided by the command line tool.

WARNING: Running on [Amazon MQ](https://aws.amazon.com/amazon-mq/) is not supported because it does not support [quorum queues or streams](https://docs.aws.amazon.com/amazon-mq/latest/developer-guide/best-practices-rabbitmq.html).
