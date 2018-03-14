## Delayed Delivery 

**Default**: `enabled`.
 
SQS supports delaying message delivery by up to 15 minutes natively. This feature is enabled by default and cannot be disabled. The timeout manager will not be used for message deferral.

WARNING: SQS delayed delivery cannot be used for messages delayed by more than 15 minutes. Sending messages with delayed deliveries greater than 15 minutes will result in runtime exceptions.
