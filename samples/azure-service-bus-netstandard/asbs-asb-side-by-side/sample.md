---
title: Azure Service Bus Standard and Azure Service Bus side by side
reviewed: 2017-10-17
component: ASBS
related:
- transports/azure-service-bus
- nservicebus/bridge
---

If it's needed to run, in the same process and `AppDomain`, an endpoint instance using the Azure Service Bus Standard transport and an endpoint instance using the Azure Service Bus one there is the risk of name collision between classes coming from the 2 different transports.

To avoid any name clash use aliases to reference types of one of the transports. For example, to define an alias for the Azure service Bus transport edit the project file, adding the following snippet:

```xml
<Target Name="ChangeAliasesOfNugetRefs" BeforeTargets="FindReferenceAssembliesForReferences;ResolveReferences">
  <ItemGroup>
    <ReferencePath Condition="'%(FileName)' == 'NServiceBus.Azure.Transports.WindowsAzureServiceBus'">
      <Aliases>TransportASB</Aliases>
    </ReferencePath>
  </ItemGroup>
</Target>
```

Once the alias is defined reference the alias in the C# source files where it is needed by adding it to the usings list:

snippet: transport-asb-alias-definition

The alias can now be used to reference types from the Azure Service Bus transport assembly:

snippet: transport-asb-alias-usage