The Delayed Retries policy is only applied on the distributor for both *NumberOfRetries* and *TimeIncrease* settings.

The distributor has a **.retries** queue where a message is forwarded to in case of an error. Then the distributor processes this message, when the retry limit has been reached the message will be forwarded to the error queue or else scheduled for retry by the distributor.
