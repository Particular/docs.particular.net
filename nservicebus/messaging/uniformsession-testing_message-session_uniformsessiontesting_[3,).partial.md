### Testing together with IMessageSession

For tests including code paths using both `IMessageSession` and `IUniformSession`, the `TestableUniformSession` can also wrap a `TestableMessageSession`:

snippet: UniformSessionMessageSessionTesting