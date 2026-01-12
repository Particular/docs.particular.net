## Disable assembly scanning

Assembly scanning can be completely disabled. When disabled, no assemblies are scanned, and the endpoint will not automatically discover message types, handlers, features, or installers.

snippet: DisableAssemblyScanning

> [!WARNING]
> When assembly scanning is disabled, message handlers, sagas, features, and installers must be explicitly registered. Messages received without a registered handler or saga [will fail and be moved to the error queue](/nservicebus/handlers/?version=core_10#no-handler-for-a-message).

> [!NOTE]
> Manual registration APIs work alongside assembly scanning, enabling a hybrid approach. Use [assembly exclusion options](#assemblies-to-scan) to limit what gets scanned, then manually register specific components as needed.

When assembly scanning is disabled, register components with the following manual registration APIs.

### Manual handler registration

Use `AddHandler<THandler>()` to register message handlers:

snippet: RegisterHandlerManually

### Manual saga registration

Use `AddSaga<TSaga>()` to register sagas:

snippet: RegisterSagaManually

### Manual feature registration

Use `EnableFeature<TFeature>()` to enable features:

snippet: EnableFeatureManually

### Manual installer registration

Use `AddInstaller<TInstaller>()` to register installers:

snippet: RegisterInstallerManually

> [!NOTE]
> Some NServiceBus packages and extensions provide their own manual registration APIs beyond the core APIs listed above. For example, custom checks have their own registration methods. When using additional NServiceBus packages, consult their documentation for any manual registration requirements.
