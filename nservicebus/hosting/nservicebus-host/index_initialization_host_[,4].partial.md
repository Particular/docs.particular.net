To change core settings such as assembly scanning, container, and serialization format, implement `IWantCustomInitialization` on the endpoint configuration class (the same class that implements `IConfigureThisEndpoint`). Start the configuration expression with

```cs
Configure.With()
```

NOTE: Do not perform any startup behaviors in the `Init` method.

After the custom initialization is done the regular core `INeedInitalization` implementations found will be called in the same way as when self hosting.


include:host-startup