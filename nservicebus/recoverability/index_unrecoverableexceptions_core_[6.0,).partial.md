## Unrecoverable exceptions

Recoverability enables declaring an exception type as unrecoverable. When a message processing fails due to a unrecoverable exception being thrown, it does not go through retry process but is moved to the error queue directly instead. By default MessageDeserializationException is the only unrecoverable exception however based on the business domain users can declare additional. For example, a user might declare ValidationExceptions thrown when a message fails validation as unrecoverable. Assuming that validation is deterministic, processing will always fail so extra retries will not bring any benefit.

NOTE: Declaring exception type as unrecoverable declares the whole inheritance tree as unrecoverable i.e. any direct or indirect subclass.

snippet: UnrecoverableExceptions

The above snippets defines `ValidationException` and `ArgumentException` as unrecoverable exceptions. For example if a `ArgumentNullException` is raised during message handling, the message will be automatically moved to the error queue since `ArgumentNullException` is assignable to `ArgumentException`.

snippet: UnrecoverableExceptionsSettings

It is also possible to define certain exceptions as unrecoverable in plugins such as persisters, transports and features. Everywhere there is access to `SettingsHolder` the non-generic `AddUnrecoverableException` can be used.
