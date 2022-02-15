### Optional Pipeline Stages

* Audit: Behaviors in the _audit_ stage have access to the message to be audited/sent to the audit queue and audit address. Behaviors should use `IAuditContext` to enlist in this stage. This stage is only entered if [Message Auditing](/nservicebus/operations/auditing.md) is enabled.
* Forwarding: Behaviors in the _forwarding_ stage have access to the message to be sent to the forwarding queue and the address of the forwarding queue. Behaviors should use `IForwardingContext` to enlist in this stage. This stage is only entered if [message forwarding](/nservicebus/messaging/forwarding.md) is enabled.
