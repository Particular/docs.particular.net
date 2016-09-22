

## Limiting assemblies scanned by the test framework

To limit the assemblies and types scanned by the test framework it is possible to use the `Initialize()` overload that accepts a delegate to customize the `ConfigurationBuilder`. The list of assemblies scanned must include `NServiceBus.Testing.dll`

snippet: TestInitializeAssemblies 
