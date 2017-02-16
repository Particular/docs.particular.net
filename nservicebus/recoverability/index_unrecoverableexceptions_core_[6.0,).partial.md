## Unrecoverable exceptions

By default every exception, unless it is NServiceBus' built-in `MessageDeserializationException`, will go through the recoverability as defined in the endpoint configuration. Depending on the business domain some exceptions might indicate that is never useful to go through Recoverability since all subsequent retries would lead to the same exception over and over again. An example is message validation. When a validator would raise a `ValidationExceptions`, or derivatives of that exception base type, subsequent retries of the same message would fail the validation again. In such scenarios, it is possible to mark certain exception types as unrecoverable. When an exception is marked as unrecoverable, then message handling that raises such an exception type will immediately move the message to the error queue and not even try to attempt to do [Immediate](/nservicebus/recoverability/configure-immediate-retries) and [Delayed Retries](/nservicebus/messaging/delayed-delivery). 

snippet: UnrecoverableExceptions

The above snippets defines `ValidationException` and `ArgumentException` as unrecoverable exceptions. For example if a `ArgumentNullException` is raised during message handling, the message will be automatically moved to the error queue since `ArgumentNullException` is assignable to `ArgumentException`.

snippet: UnrecoverableExceptionsSettings

It is also possible to define certain exceptions as unrecoverable in plugins such as persisters, transports and features. Everywhere there is access to `SettingsHolder` the non-generic `AddUnrecoverableException` can be used.