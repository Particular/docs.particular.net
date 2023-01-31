snippet: FullyCustomizedPolicyRecoverabilityConfigurationWithDiscard

The snippet below shows a fully custom policy that does the following:

 * For unrecoverable exceptions such as `MyBusinessException` failed messages are immediately moved to a custom error queue. 
 * For exceptions where retrying does not make sense such as `MyBusinessTimedOutException` failed messages will be discarded as they had never happened. The discarded messages will not be moved to the error queue nor forwarded to the audit queue. The reason why the message was discarded is only communicated in logs.
 * For `MyOtherBusinessException` Delayed Retries are performed with a constant time increase of five seconds.
 * For all other cases failed messages are immediately moved to the configured error queue.

snippet: FullyCustomizedPolicyWithDiscard
