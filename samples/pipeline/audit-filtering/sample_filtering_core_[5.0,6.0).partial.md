To implement the desired filtering logic, a custom audit behavior is required:

snippet:auditRulesBehavior

NOTE: Since the behavior is going to replace the existing audit behavior, it is necessary to ensure the audit logic is handled completely by the new behavior. This is done by copying the logic from the core's `AuditBehavior` to the custom behavior.