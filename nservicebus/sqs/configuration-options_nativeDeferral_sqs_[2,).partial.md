## NativeDeferral 

**Optional**

**Default**: `false`.
 
SQS supports delaying message delivery by up to 15 minutes. When this option is set to `true`, The SQS transport will not use the timeout manager for message deferral and will use the delayed delivery feature of SQS instead. 

Enabling this on an endpoint means that a separate timeout queue is not used, thus saving the cost of continuously polling it. However when the need arises to delay the delivery of messages by a time span of more than 15 minutes, the SQS delayed delivery cannot be used.

**Example**

snippet: NativeDeferral