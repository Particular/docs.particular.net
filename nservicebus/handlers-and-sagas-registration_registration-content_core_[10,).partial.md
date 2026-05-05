## Source-generated registration (recommended)

Starting in NServiceBus version 10.2, a source generation package can scan assemblies at build time and produce a strongly typed registration API. This keeps NServiceBus registrations componentized without requiring runtime assembly scanning.

The generated API is composable: each scanned assembly produces extension methods that attach to a registry. For example, after referencing the source generator package, the registration code looks like this:

```csharp
endpointConfiguration.Handlers.MyAssembly.MyNamespace.AddAll();
```

Source-generated registration is trimming and AOT-friendly because it references handler and saga types directly from the composition root (the host). It also eliminates runtime surprises: any type that is discoverable at build time is discoverable at runtime with no additional scanning.

### Default behavior

Without the `[HandlerRegistryExtensions]` attribute, the source generator produces an entry point named after the assembly and creates registration methods from the discovered class names:

| Type kind | Class name | Generated method |
|---|---|---|
| Handler | `OrderShippedHandler` | `AddOrderShippedHandler` |
| Handler | `OrderShipped` | `AddOrderShippedHandler` |
| Saga | `OrderShippingSaga` | `AddOrderShippingSaga` |
| Saga | `OrderShippingPolicy` | `AddOrderShippingPolicy` |

For handlers, the generator prepends `Add` and guarantees the method name ends with `Handler`. If the class name does not already end with `Handler`, the suffix is appended automatically. For sagas, the generator prepends `Add` and ensures the name ends with `Saga`, except when the class already ends with `Saga` or `Policy` (case-insensitive).

### Advanced source generation configuration

The `[HandlerRegistryExtensions]` attribute can be applied to a `partial static class` to customize the source-generated handler registration API:

#### `EntryPointName`

Overrides the default assembly-based entry point name. The value must be a valid C# identifier.

```csharp
[HandlerRegistryExtensions(EntryPointName = "Shipping")]
public static partial class RegistrationExtensions;
```

When the attribute above is applied, `registry.Handlers.Core...` becomes `registry.Handlers.Shipping...`.

#### `RegistrationMethodNamePatterns`

The default `Add<TypeName>` convention mirrors the class name exactly, which can produce repetitive or unnecessarily long method names. For example, `OrderShippedHandler` generates `AddOrderShippedHandler`, duplicating the `Handler` suffix on every call site.

Use `RegistrationMethodNamePatterns` to remap class names to shorter or more idiomatic method names. Each pattern is a regex replacement in the form `pattern=>replacement`. The pattern is matched against the original class name, and the replacement string becomes the final method name, so any desired prefix must be included in the replacement itself. Patterns are evaluated in order and the first match wins.

Invalid pattern format or regular expression syntax is caught by a build-time analyzer and reported as a compiler error.

Examples:

- `^(.*)Handler$=>Add$1` transforms `OrderShippedHandler` to `AddOrderShipped`.
- `Handler$=>Register` transforms `OrderShippedHandler` to `OrderShippedRegister`.
- `^(.*)Saga$=>Register$1Saga` transforms `OrderShippingSaga` to `RegisterOrderShippingSaga`.

#### Visibility control

The source generator automatically matches the visibility of the generated extension methods to the visibility of the attributed `partial class`. Declaring the class as `internal` hides the registration methods from the public API.

### Relationship to other attributes

`[HandlerRegistryExtensions]` works alongside `[Handler]` (for marking message handlers) and `[Saga]` (for marking sagas) to enable source generation.

## Manual registration

When source generation is not available or when only a few components need to be registered, individual types can be added explicitly.

### Register a message handler

Use `AddHandler<THandler>()` to register a message handler:

snippet: RegisterHandlerManually

### Register a saga

Use `AddSaga<TSaga>()` to register a saga:

snippet: RegisterSagaManually

### Enable a feature

Use `EnableFeature<TFeature>()` to enable a feature:

snippet: EnableFeatureManually

### Register an installer

Use `AddInstaller<TInstaller>()` to register an installer:

snippet: RegisterInstallerManually

## Assembly scanning

Automatic runtime discovery of handlers, sagas, features, and installers is performed by scanning assemblies. Assembly scanning is useful for plugin architectures, shared deployment directories, or other dynamic discovery scenarios.

### Disable assembly scanning

Assembly scanning can be completely disabled. When disabled, no assemblies are scanned, and all components must be explicitly registered.

snippet: DisableAssemblyScanning

When assembly scanning is disabled, message handlers, sagas, features, and installers must be explicitly registered. Messages received without a registered handler or saga [will fail and be moved to the error queue](/nservicebus/handlers/?version=core_10#no-handler-for-a-message).

### Fine-grained scanning configuration

Scanning can be configured with exclusions, additional paths, nested directories, and exception handling. See [Assembly scanning](/nservicebus/hosting/assembly-scanning.md) for all configuration options.

## Hybrid approach

Assembly scanning and manual registration can be combined. For example, scan for plugins in a shared directory and then explicitly register a set of core handlers. When scanning is disabled entirely, all components must be registered explicitly.

## Choosing an approach

- Prefer source-generated or manual explicit registration for new endpoints.
- Prefer assembly scanning when dynamic plugin discovery is required.
- Mixed mode is valid when gradual migration is needed.
