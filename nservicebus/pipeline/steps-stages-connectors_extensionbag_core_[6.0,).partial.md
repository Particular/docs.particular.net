## Extension bag

The pipeline has an extension bag which can be used to used to create, read, update or delete custom state with a key identifier. For example, this can be used to *set* metadata- or pipeline-specific state in an incoming behavior that can be used in later behavior pipeline stages if needed. State stored via the extension bag will be removed once the extension bag runs out of scope at the end of the pipeline.

State set during the *outgoing* pipeline will not be available in the *incoming* pipeline. If state has to be transferred from the *outgoing* into the *incoming* pipeline, use [message headers](/nservicebus/messaging/headers.md) in combination with serialization.

snippet: SetContextBetweenIncomingAndOutgoing

State set during the *incoming* pipeline, will be available in the *recoverability* pipeline.