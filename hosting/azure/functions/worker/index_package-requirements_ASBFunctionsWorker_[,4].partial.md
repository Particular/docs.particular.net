## Package requirements

`NServiceBus.AzureFunctions.Worker.ServiceBus` requires Visual Studio 2019 and .NET SDK version `5.0.300` or higher. Older versions of the .NET SDK might display the following warning which prevents the trigger definition from being auto-generated:

```
CSC : warning CS8032: An instance of analyzer NServiceBus.AzureFunctions.SourceGenerator.TriggerFunctionGenerator cannot be created from ServiceBus.AzureFunctions.Worker.SourceGenerator.dll : Could not load file or assembly 'Microsoft.CodeAnalysis, Version=3.10.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'. The system cannot find the file specified..
```

Starting in version 4.1.0 of `NServiceBus.AzureFunctions.Worker.ServiceBus`, warning CS8032 is treated as an error. To suppress this error, update the project's .csproj file to include the following line in the `<PropertyGroup>` section at the top:

```xml
  <WarningsAsErrors></WarningsAsErrors>
```

This will revert CS8032 to a warning status so that it does not stop the build process.