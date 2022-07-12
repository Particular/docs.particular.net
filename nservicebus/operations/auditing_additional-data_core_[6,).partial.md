## Additional audit information

Additional information can be added to audit messages using a [custom behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) as shown in the following snippet. The additional data will be contained in the audit message headers.

snippet: AddAuditData

Note: Message headers count towards the message size and the additional audit information has to honor the transport's message header limitations.
