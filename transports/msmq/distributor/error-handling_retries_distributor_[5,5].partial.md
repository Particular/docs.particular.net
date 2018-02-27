The Delayed Retries policy *NumberOfRetries* setting is applied on *both* the distributor and workers, and the *TimeIncrease* setting is only applied on the distributor.

When an error occurs the Delayed Retries policy is invoked immediately by the fault manager. The message will not be forwarded to the retries queue which was the behavior prior to version 5.

When the retry limit is reached the message is forwarded immediately to the error queue or else forwarded to the **.retries** queue and scheduled for retry. If the Delayed Retries policy output is that it needs to be retried then the message is forwarded to the **.retries** queue.
