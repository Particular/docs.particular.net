Configure the target audit queue using the configuration API.

snippet: AuditWithCode

Starting from version 10.1.0, auditing configuration can also be done via environment variables.

#### NSERVICEBUS__AUDIT__DISABLED

The `NSERVICEBUS__AUDIT__DISABLED` environment variable can be set to `TRUE` to disable auditing, even if it was enabled in code.  This allows for auditing to be disabled without needing to recompile and redeploy the code.

#### NSERVICEBUS__AUDIT__ADDRESS

The `NSERVICEBUS__AUDIT__ADDRESS` environment variable can be used to configure the audit queue name. This allows for auditing to be enabled on endpoints that did not call the `AuditProcessedMessagesTo` method on the `EndpointConfiguration` object, and avoids needing to recompile and redeploy the code.

> [!NOTE]
> If auditing was configured in code by calling the `AuditProcessedMessagesTo` method, the queue name specified in that method call will take precedence, and cannot be overridden by the `NSERVICEBUS__AUDIT__ADDRESS` environment variable. In this case, the `NSERVICEBUS__AUDIT__ADDRESS` environment variable will be ignored.
