### Optional pipeline stages

Behaviors in the _audit_ stage have access to the message to be audited/sent to the audit queue and audit address. Behaviors should use `IAuditContext` to enlist in this stage. This stage is only entered if [message auditing](/nservicebus/operations/auditing.md) is enabled.
