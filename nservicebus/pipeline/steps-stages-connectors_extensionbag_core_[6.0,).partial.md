## Extension bag

Pipeline contexts have an extension bag which can be used to used to create, read, update or delete custom state with a key identifier. For example, this can be used to *set* metadata- or pipeline-specific state in an incoming behavior that can be used in later pipeline stages if needed. State stored via the extension bag will be unavailable once the extension bag runs out of scope at the end of the pipeline.

State set during a *forked* pipeline will not be available to the *forking* pipeline. For example, state changes during the *outgoing* pipeline will not be available in the *incoming* pipeline. If state has to be propagated, have the *forking* pipeline set a context object that the *forked* pipeline later can get and modify as needed.

snippet: SetContextBetweenIncomingAndOutgoing
