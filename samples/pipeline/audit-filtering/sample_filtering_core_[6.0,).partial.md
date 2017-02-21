Three behaviors are added to the message processing pipeline to implement the desired filtering logic:


### AuditFilterContextBehavior

This behavior adds a class to the pipeline contexts `Extensions` bag. This class can later be accessed by the other behaviors to share state across behaviors. The state needs to be added early in the pipeline because anything added to the `Extensions` after the `IIncomingPhysicalMessageContext` is invisible to the `IAuditContext`.

snippet:auditFilterContextBehavior


### AuditRulesBehavior

The `AuditRulesBehavior` uses the `IIncomingLogicalMessageContext` to inspect the incoming message type and applies its rules to determine whether that message should be audited or not. If the message should not be audited, it retrieves the shared state from the context's `Extensions` and marks the message.

snippet:auditRulesBehavior


### AuditFilterBehavior

This behavior is invoked for every message which is sent to the audit queue. By retrieving the shared state and checking its value, this behavior can stop the auditing pipeline by not invoking the next step.

snippet:auditFilterBehavior