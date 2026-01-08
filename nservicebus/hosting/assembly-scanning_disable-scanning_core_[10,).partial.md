## Disable assembly scanning

Assembly scanning can be completely disabled. When disabled, no assemblies are scanned, and the endpoint will not automatically discover message types, handlers, features, or installers.

snippet: DisableAssemblyScanning

> [!WARNING]
> When assembly scanning is disabled, all required types (message handlers, sagas, features, etc.) must be explicitly registered. Failure to register required types will cause the endpoint to fail at startup or behave incorrectly.

