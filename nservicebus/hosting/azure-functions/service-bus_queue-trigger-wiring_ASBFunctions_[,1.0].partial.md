To access `IFunctionEndpoint` from the Azure Function trigger, inject the `IFunctionEndpoint` via constructor-injection into the containing class and pass the incoming `Message` to the injected `IFunctionEndpoint`:

snippet: asb-function-hostbuilder-trigger