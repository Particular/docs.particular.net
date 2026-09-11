---
title: Trimming-safe messaging overloads
summary: Use the strongly-typed messaging overloads and the migration analyzer to make messaging code trimming-safe and AOT-safe
component: Core
versions: '[10,)'
reviewed: 2026-09-11
related:
 - nservicebus/messaging/send-a-message
 - nservicebus/messaging/reply-to-a-message
 - nservicebus/operations/nservicebus-analyzer
 - nservicebus/pipeline/message-mutators
---

Starting in NServiceBus version 10.3.0, the messaging APIs provide strongly-typed overloads that are safe to use with [trimming](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/) and [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/). A migration analyzer included in the NServiceBus package guides existing applications to these overloads.

## What does trimming-safe mean

Trimming removes unreferenced code and metadata from an application at publish time, and Native AOT compiles the application to platform-native code ahead of time. In both scenarios, the runtime type information that reflection-based code depends on may no longer be available.

Code is trimming-safe when the types it needs are known at compile time and preserved in the published application. Code that discovers types at runtime, such as `message.GetType()`, is not trimming-safe because the metadata for the type may have been removed.

## Why messaging needs strongly-typed overloads

The object-based overloads, such as `Send(message, options)` and `Publish(message, options)`, determine the message type at runtime by calling `message.GetType()`. When trimming or Native AOT is enabled, the runtime type information required to route the message may no longer be available, so these overloads cannot be analyzed statically and are annotated with `RequiresUnreferencedCode`.

Strongly-typed overloads carry the message type either in the generic type argument, as in `Send<T>(message, options)`, or as an explicit `Type` parameter, as in `Send(message, messageType, options)`. Because the message type is supplied by the caller instead of being discovered from the message instance at runtime, the trimmer can analyze these overloads. When passing the type explicitly, use a value the trimmer can see through, such as `typeof(MyMessage)`.

## Strongly-typed overloads

The following messaging operations provide generic overloads that are trimming-safe:

| Operation | Generic overload |
| -- | -- |
| Send | `Send<T>(T message, SendOptions options)` |
| Publish | `Publish<T>(T message, PublishOptions options)` |
| Reply | `Reply<T>(T message)` |
| SendLocal | `SendLocal<T>(T message)` |
| UpdateMessage | `UpdateMessage<T>(T newInstance)` |
| ReplyToOriginator | `ReplyToOriginator<T>(IMessageHandlerContext context, T message)` |

## Explicit-type overloads

Middleware and platform code often receives messages as `object` after the compile-time type has been erased. In this case, the generic overload cannot be used, but the logical message type is still known. The explicit-type overloads accept that type directly:

```csharp
object message = CreateMessage(); // compile-time type is erased
Type messageType = typeof(MyMessage); // logical message type is still known

await session.Send(message, messageType, new SendOptions());
```

This is a common pattern for type-erased scenarios. Explicit-type overloads exist for `Send`, `Publish`, `Reply`, `SendLocal`, and `UpdateMessage`.

## Logical message mutators

Logical message mutators can replace the message instance while it is processed in the pipeline. The replacement methods carry the logical message type, so mutators can be trimming-safe:

| Context | Generic overload | Explicit-type overload |
| -- | -- | -- |
| `MutateIncomingMessageContext` | `UpdateMessageInstance<T>(T newMessage)` | `UpdateMessageInstance(object newMessage, Type messageType)` |
| `MutateOutgoingMessageContext` | `UpdateMessage<T>(T newMessage)` | `UpdateMessage(object newMessage, Type messageType)` |
| `IIncomingLogicalMessageContext` | `UpdateMessageInstance<T>(T newInstance)` | `UpdateMessageInstance(object newInstance, Type messageType)` |

The declared type is the logical message type. For outgoing messages, it also determines how the message is routed and which message type is recorded on the message. It can differ from the runtime type of the instance as long as the instance is assignable to the declared type.

On `IIncomingLogicalMessageContext`, the object-based `UpdateMessageInstance(object)` overload is still available and wins overload resolution for a call without a type argument, so pipeline behaviors must pass the type argument explicitly: `context.UpdateMessageInstance<MyMessage>(message)`. The mutator contexts have no competing overload, so there the type argument can be inferred from the message instance.

Starting in NServiceBus version 10.3, prefer the strongly-typed methods above. The object-based `MutateIncomingMessageContext.Message` and `MutateOutgoingMessageContext.OutgoingMessage` setters are obsolete, will be treated as errors from version 11, and will be removed in version 12; the object-only `IIncomingLogicalMessageContext.UpdateMessageInstance(object)` method is annotated with `RequiresUnreferencedCode` and is planned for obsoletion. All of them determine the logical message type from the runtime type of the instance, which trimming cannot analyze.

## Route and publisher registration

Routes and publishers can be registered by message type, by assembly, or by namespace. Registration by assembly or namespace requires scanning assemblies at startup, which is not trimming-safe. Register routes and publishers by message type instead; see [routing](/nservicebus/messaging/routing.md) for details.

## Compatibility

The object-based overloads remain available, and existing calls continue to select them. Recompiling an existing application does not change its routing behavior: messages are still routed using their runtime type until the code is migrated explicitly.

## Migration analyzer

The NServiceBus package ships a migration analyzer that reports three diagnostics:

| Rule ID | Title | Severity | Active by default | Code fix |
| -- | -- | -- | -- | -- |
| NSB0039 | Use the strongly typed message overload | Info | No | Yes |
| NSB0040 | Message routing uses the runtime type | Warning | No | No |
| NSB0041 | The message type must not be System.Object | Warning | Yes | No |

### NSB0039 — Use the strongly typed message overload

This diagnostic fires when the analyzer can prove that the runtime type of the message matches its static type, for example when the message is created directly:

```csharp
await session.Send(new MyMessage(), new SendOptions());
```

The provided code fix rewrites the call to use the generic overload:

```csharp
await session.Send<MyMessage>(new MyMessage(), new SendOptions());
```

### NSB0040 — Message routing uses the runtime type

This diagnostic fires when the runtime type can differ from the static type, for example when the message is passed through an interface, a base class, or a method return value. Changing such a call to a strongly-typed overload can change routing, subscriptions, or logical message identity, so this diagnostic intentionally has no code fix. Treat it as a routing decision rather than a mechanical migration.

### NSB0041 — The message type must not be System.Object

This diagnostic fires when the generic overload is called with `System.Object` as the explicit type argument, for example `Send<object>(message, options)`. The strongly-typed overload would route the message as `System.Object`, which is never the intent. Specify the actual message type instead.

The diagnostics also cover logical message mutators. Assigning to `MutateIncomingMessageContext.Message` or `MutateOutgoingMessageContext.OutgoingMessage` is analyzed like the other messaging operations, and the code fix rewrites the assignment to the typed replacement method:

```csharp
// MutateIncomingMessageContext
context.Message = new MyMessage();

// MutateOutgoingMessageContext
context.OutgoingMessage = new MyEvent();
```

becomes

```csharp
// MutateIncomingMessageContext
context.UpdateMessageInstance<MyMessage>(new MyMessage());

// MutateOutgoingMessageContext
context.UpdateMessage<MyEvent>(new MyEvent());
```

Calling `UpdateMessageInstance` on an incoming logical message context is analyzed in the same way. The code fix keeps the explicit type argument because `IIncomingLogicalMessageContext` also has an object-based `UpdateMessageInstance(object)` overload; for the mutator contexts the type argument can be inferred from the message instance.

## When the migration diagnostics are active

NSB0039 and NSB0040 are quiet by default in ordinary builds. They activate automatically when a project enables trimming or AOT compatibility:

- `PublishTrimmed`
- `PublishAot`
- `IsAotCompatible`
- `IsTrimmable`
- `EnableTrimAnalyzer`

The diagnostics can also be enabled per rule in `.editorconfig`:

```ini
[*.cs]
dotnet_diagnostic.NSB0039.severity = suggestion
dotnet_diagnostic.NSB0040.severity = warning
```

An explicit per-rule severity takes precedence over automatic activation, so `none` deliberately suppresses that rule for the matching files. NSB0041 is always active because calling the generic overload with `System.Object` is always incorrect.

## Migration path

The object-only overloads are removed in a single major version transition:

| Version | Experience |
| -- | -- |
| 10.x | Object-only overloads remain available and continue to win overload resolution. The migration diagnostics are quiet by default and activate for trimming, AOT, or the explicit migration audit. |
| 11 | Object-only overloads are removed. Calls that preserve the runtime-type routing compile unchanged, and calls where generic inference could change routing are flagged by a new diagnostic, enabled as an error by default. Those calls must choose explicitly between a generic argument and an explicit-`Type` overload. |

Users who migrate early may add explicit generic type arguments, such as `Send<MyMessage>(new MyMessage(), options)`. After the object-only overloads are removed, the IDE may flag those type arguments as redundant, because `Send(new MyMessage(), options)` infers the same message type. Whether to keep or remove the explicit type argument is a choice: keeping it routes the message using the explicit type at the call site, while removing it relies on type inference. Both are valid as long as the inferred type matches the intended logical message type.

## Trimming-safe transport and persistence

Starting in NServiceBus version 10.3.0, an endpoint can be published as a trimmed or Native AOT application when it uses a transport and persistence that keep all message state inside the endpoint process or its local environment:

- The [Learning transport](/transports/learning/) and [Learning persistence](/persistence/learning/) ship with the NServiceBus package and are designed for development and testing.
- The [Non-Durable transport](/transports/non-durable/) and [Non-durable persistence](/persistence/non-durable/) are production options when message loss can be tolerated. Messages are held in process memory and are lost when the process ends, but no external infrastructure is required.

Trimmed and Native AOT endpoints discover handler, saga, and message types at build time rather than by scanning assemblies at runtime. See [registering message types](#registering-message-types) for the required configuration.

## Trimming-safe serialization

The [System.Text.Json serializer](/nservicebus/serialization/system-json.md) is the recommended serializer for trimmed and Native AOT applications. Reflection-based serialization is not available in these deployments, so message types must be registered in a source-generated `JsonSerializerContext` and supplied through `JsonSerializerOptions`:

```csharp
configuration.UseSerialization<SystemJsonSerializer>()
    .Options(new JsonSerializerOptions
    {
        TypeInfoResolver = MyMessagesJsonContext.Default
    });
```

The [XML serializer](/nservicebus/serialization/xml.md) is not supported with trimming or Native AOT because it relies on runtime type information and dynamic code generation.

## Registering message types

With assembly scanning disabled and trimming or Native AOT enabled, NServiceBus resolves message metadata only from types registered explicitly. The source-generated [handler and saga registration](/nservicebus/handlers-and-sagas-registration.md) registers the message types handled by the handlers and sagas it adds. Message types that an endpoint only sends, publishes, or replies to, and that no local handler or saga handles, are not covered by that registration and must be registered separately:

snippet: RegisterMessageTypeManually

`AddMessageType<T>()` registers the message type together with its hierarchy of base types and implemented interfaces. The type must already be identified as a message by the endpoint's conventions; the method does not classify arbitrary types as messages. In ordinary applications the hierarchy is inferred at runtime, while under trimming or Native AOT the call is replaced by a source-generated, reflection-free registration. The call must name a concrete message type: the source generator cannot replace a call that forwards a type parameter through a generic helper method.

When a required message type is not registered, message processing fails with an exception that names the missing type and the registration to add. Message types that are known when the endpoint starts fail at startup; message types that appear only later fail on first use.

## Related trimming guidance

Strongly-typed messaging is one part of running an NServiceBus endpoint with trimming or Native AOT:

- [Startup diagnostics](/nservicebus/hosting/startup-diagnostics.md#adding-startup-diagnostics-sections) — register diagnostics sections with type information so the diagnostics document can be serialized without reflection.
- [Registering handlers and sagas](/nservicebus/handlers-and-sagas-registration.md) — source-generated registration is trimming and AOT-friendly; discovery by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) relies on runtime type information.
