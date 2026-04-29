---
title: Convention-based handlers
summary: How to create message handlers that don't implement IHandleMessages<T>
component: Core
versions: '[10,)'
reviewed: 2026-05-04
related:
---

Starting with NServiceBus version 10.2.0, NServiceBus supports **convention-based message handlers** that do not implement `IHandleMessages<T>`. These handlers are not discovered through traditional assembly scanning, but instead are either added to an NServiceBus endpoint declaratively, or with help from Roslyn analyzers and source generators.

## Handler structure

A convention-based handler can look exactly like a regula rmessage handler, but does not implement the interface:

snippet: SimpleConventionBasedHandler

Without the rigid structure of the `IHandleMessages<T>` interface, additional parameters can be added to the `Handle` method:

snippet: ConventionsBasedHandlerExtraParams

These requirements must be met for a class to be recongizable as a convention-based message handler:

- Must contain a handler method named `Handle` which returns `Task`.
- The handler method's first parameter must be a message class.
- The hadnler method's second parameter must be an `IMessageHandlerContext`.
- After the first two parameters, any additional parameters must be either a `CancellationToken` or registered in host's `IServiceCollection`.
- The method can be an instance or static method.
- A handler class may contain multiple handler methods, differing by the message type, but these methods become an inseparable unit. It is not possible to register one handler method on a class but not the other.

TODO: What happens if there are 2 handler methods for the same message type but with different additional params?

## Registering handlers

Because handlers not using a marker interface cannot be found by assembly scanning, they must be added to the endpoint. This can be done manually:

snippet: ConventionHandlerRegistrationWithoutAttribute

However, decorating a handler class with the `NServiceBus.HandlerAttribute` (or `NServiceBus.SagaAttribute` for [sagas](/nservicebus/sagas/)) enables source generation that enables all decorated handlers and/or sagas in an entire project to be added with one line of configuration.

First, decorate handlers with `[Handler]`, or sagas with `[Saga]`:

snippet: DecoratedConventionHandler

This generates source code that allows all handlers and sagas from an assembly to be registered at once:

snippet: ConventionHandlerAddAllFromAssembly

However, the generated source is flexible and allows registering just all handlers, just all sagas, or both from any level of the namespace hierarchy, or at the top level of the assembly:

snippet: ConventionHandlerAllGeneratedAddMethods

## Analyzers

While the source generation simplifies registering multiple handlers or sagas to an endpoint with one line of code, the generation relies on the `[Handler]` and `[Saga]` attributes to identify what qualifies as a handler or a saga.

> [!TIP]
> The reason the source generation works on the `[Handler]` and `[Saga]` attributes is because source generators, which may run in your editor up to every time you press a key, must be very fast and efficient. Identifying generation targets using a marker attribute is the most optimized method availble when using the Roslyn SDK. On the other hand, using marker interfaces to identify generation targets is [explicitly called out as an anti-pattern in the source generator documentation](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md).

Roslyn analyzers help to ensure that handlers or sagas don't accidentally escape identification, causing them to remain unregistered accidentally:

- **NSB0034**: Mark convention-based handlers with HandlerAttribute to enable source generation
- **NSB0025**: Mark sagas with SagaAttribute to enable source generation

These diagnostics default to `DiagnosticSeverity.Info` but can be upgraded to ensure handlers and sagas are not missed.

The following `.editorconfig` settings will upgrade both diagnostics to errors so that the build will fail if the attributes are not added:

```ini
[*.cs]

# Ensure message handlers are decorated with [Handler] to enable source generation
dotnet_diagnostic.NSB0034.severity = error
# Ensure sagas are decorated with [Saga] to enable source generation
dotnet_diagnostic.NSB0034.severity = error
```