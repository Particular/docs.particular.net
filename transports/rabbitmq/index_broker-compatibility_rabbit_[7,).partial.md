The transport is compatible with RabbitMQ broker version 3.10.0 or higher either self-hosted or running on [Amazon MQ](https://aws.amazon.com/amazon-mq/) or [CloudAMQP](https://www.cloudamqp.com/).

In order to enable at [least once dead lettering](https://blog.rabbitmq.com/posts/2022/03/at-least-once-dead-lettering/) the `stream_queue` and `quorum_queue` [feature flags](https://www.rabbitmq.com/feature-flags.html) must be enabled.
