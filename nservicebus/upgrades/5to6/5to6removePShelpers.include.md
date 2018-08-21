## Visual Studio PowerShell helpers removed

In NServiceBus versions 3 through 5, there is a NuGet package that provides helpers to generate XML configuration sections using the PowerShell console in Visual Studio.

For example, running the `Add-NServiceBusAuditConfig ProjectName` command would add the following section to the `app.config` file:

```xml
<configuration>
  <configSections>
    <section name="AuditConfig"
             type="NServiceBus.Config.AuditConfig, NServiceBus.Core" />
  </configSections>
  <AuditConfig QueueName="audit" />
</configuration>
```

These helpers have been removed in version 6. The configuration helpers encouraged creating a more complex XML configuration than was necessary, making it difficult to manage in the long run.

The recommended configuration approach is "code-first", which is more flexible and less error-prone than using the PowerShell helpers. The configuration can be read from any location at runtime, including the `app.config`. For more information about the API, refer to the configuration documentation dedicated to the particular functionality. For example, see how [recoverability](/nservicebus/recoverability/) and [audit](/nservicebus/operations/auditing.md#configuring-auditing) queues can be configured using code API.
