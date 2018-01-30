### Enabling the timeout manager

If an endpoint was migrated from a version of the transport that didn't have native delayed delivery support, it could have delayed messages in its persistence database that still need to be delivered. To ensure those messages are properly delivered, call the following API:

snippet: rabbitmq-delay-enable-timeout-manager

NOTE: Even when the timeout manager is enabled, new delayed messages will be sent through the delay infrastructure and not the timeout manager.