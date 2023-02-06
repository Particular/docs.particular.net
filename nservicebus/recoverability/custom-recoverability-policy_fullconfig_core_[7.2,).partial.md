snippet: FullyCustomizedPolicyRecoverabilityConfigurationWithDiscard

The snippet below shows a fully custom policy that does the following:

 * For unrecoverable exceptions, such as `MyBusinessException`, failed messages are immediately moved to a custom error queue. 
 * For exceptions where retrying is not required, such as `MyBusinessTimedOutException`, failed messages will be discarded as if they had not occurred. The discarded messages will neither be moved to the error queue nor forwarded to the audit queue. However, the reason for their failure will appear in the logs.
 * For `MyOtherBusinessException`, delayed retries are performed with a constant time increase of five seconds.
 * For all other cases, failed messages are immediately moved to the configured error queue.

snippet: FullyCustomizedPolicyWithDiscard
