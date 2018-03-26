## Unrestricted Delayed Delivery 

**Optional**

**Default**: `disabled`.
 
SQS supports delaying message delivery by up to 15 minutes (900 seconds) natively. For message deferrals with a delayed delivery longer than 15 minutes the unrestricted delayed delivery mode has to be enabled.

Enabling this on an endpoint means that a separate FIFO queue is used and continuously polled for due timeouts. For a detailed overview about the unrestricted delayed delivery refer to the [delayed delivery documentation](/transports/sqs/delayed-delivery.md)/.

**Example**

snippet: DelayedDelivery
