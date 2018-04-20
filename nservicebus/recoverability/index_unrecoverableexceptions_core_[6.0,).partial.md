## Unrecoverable exceptions

Note: This is added to NServiceBus 6.2

If a message processing fails due to an unrecoverable exception being thrown, then the retry process is skipped. The failed message is then immediately moved to the error queue after the first failure. 

According to the default policy, only exceptions of type `MessageDeserializationException` are considered unrecoverable. However, it's possible to customize the policy and declare additional types as unrecoverable exceptions. That allows to skip retries for certain exceptions, when it's known in advance that retries won't resolve the issue.

For example, messages might need validation to ensure they contain all required information and are well-formed. If a message fails validation, then exception of type `ValidationExceptions` will be thrown. If the validation process is deterministic, then `ValidationExceptions` exceptions might be configured as unrecoverable, as every retry attempt will fail anyway.

snippet: UnrecoverableExceptions

NOTE: Declaring exception type as unrecoverable declares the whole inheritance tree as unrecoverable i.e. any direct or indirect subclasses.

In the example above, `ValidationException` and `ArgumentException` are defined as unrecoverable. If an `ArgumentNullException` is raised during message processing, then according to this policy the failed message will be immediately moved to the error queue without retries, since `ArgumentNullException` inherits from the `ArgumentException` type.

snippet: UnrecoverableExceptionsSettings

It is also possible to define exceptions as unrecoverable in plugins such as persisters, transports and features, using the `AddUnrecoverableException` method exposed on the `SettingsHolder` property.
