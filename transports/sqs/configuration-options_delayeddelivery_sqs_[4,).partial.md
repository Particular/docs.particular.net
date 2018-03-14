## Unrestricted Delayed Delivery 

**Optional**

**Default**: `disabled`.
 
SQS supports delaying message delivery by up to 15 minutes natively. For message deferrals with a delayed delivery greater than 15 minutes the unrestricted delayed delivery mode has to be enabled.

Enabling this on an endpoint means that a separate fifo queue is used and continuously polled for due timeouts. For a detailed overview about the unrestricted delayed delivery refer to the [delayed delivery documentation](/transports/sqs/delayed-delivery)/.

**Example**

snippet: DelayedDelivery
