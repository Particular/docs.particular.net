## Publisher confirms

[Publisher confirms](https://www.rabbitmq.com/confirms.html) are enabled by default, but they can be disabled by using the following:

snippet: rabbitmq-config-disablepublisherconfirms

Note: When publisher confirms are disabled, send operations might not fail with an exception when the destination exchanges/queues don't exist.
