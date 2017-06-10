## MaxReceiveMessageBatchSize 

**Optional**

**Default**: 10.
 
This is the maximum number of messages that a receiving thread will attempt to receive in a single API call. The maximum value permitted is 10. 

If you leave this set to 10, and 10 messages are available in the queue, then all 10 messages are received at once, you will only be charged for a single API call, and those 10 messages will be processed *serially*.

On the other hand, if you set this to 1, and 10 messages are available in the queue, then 10 messages are received in 10 separate API calls, you will be charged for 10 API calls, and those 10 messages will be processed *in parallel*.

**Example**: To set this to 1, you would specify:

    MaxReceiveMessageBatchSize=1;

***