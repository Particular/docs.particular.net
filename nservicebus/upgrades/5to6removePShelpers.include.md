## Visual Studio PowerShell helpers removed  

In NServiceBus Version 3 and higher, there was a NuGet package that allowed to generate xml configurations sections using the PowerShell console in Visual Studio. 

For example, running the `Add-NServiceBusAuditConfig ProjectName` command would add the following section to the `app.config` file:

```
<configuration>
  <configSections>
    <section name="AuditConfig" 
             type="NServiceBus.Config.AuditConfig, NServiceBus.Core" />
  </configSections>
  <AuditConfig QueueName="audit" />
</configuration>
```

In Version 6 and higher these helpers have been removed. The configuration helpers encouraged creating a more complex xml configuration than was necessary, making it difficult to manage in the long-run. 

The recommended configuration approach is "code-first", which is more flexible and less error prone than using Powershell helpers. The configuration can be read from any location at runtime, including the `app.config`. For more information about the API refer to the configuration documentation dedicated to the particular functionality. For example, see how [Error](/nservicebus/errors/#configure-your-error-queue-using-code) and  [Audit](/nservicebus/operations/auditing.md#configuring-auditing-using-code) queues can be configured using code API. 