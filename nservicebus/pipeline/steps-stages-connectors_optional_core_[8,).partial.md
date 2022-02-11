### Optional Pipeline Stages

Behaviors in the Audit stage have access to the message to be audited/sent to the audit queue and audit address. Behaviors should use `IAuditContext` to enlist in this stage. This stage is only entered if [Message Auditing](/nservicebus/operations/auditing.md) is enabled.
