## Publisher confirms

[Publisher confirms](https://www.rabbitmq.com/confirms.html) are enabled by default, but they can be disabled by using the following:

snippet: rabbitmq-config-disablepublisherconfirms

Note: When publisher confirms are disabled send operations will not verify if relevant exchanges and/or queues exists and will not fail.
