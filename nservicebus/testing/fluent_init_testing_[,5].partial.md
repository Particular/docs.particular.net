

## Limiting scanning

To limit the assemblies and types scanned it is possible to use the `Initialize()` overload that accepts a delegate to customize the `ConfigurationBuilder`. The list of assemblies scanned must include `NServiceBus.Testing.dll`

snippet: TestInitializeAssemblies 
