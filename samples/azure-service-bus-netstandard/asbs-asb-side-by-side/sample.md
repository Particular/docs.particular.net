---
title: Side-by-side with legacy Azure Service Bus
reviewed: 2020-06-17
component: ASBS
related:
- transports/azure-service-bus
- transports/azure-service-bus/legacy
---

If it's necessary to run an endpoint instance using the Azure Service Bus transport and an endpoint instance using the legacy Azure Service Bus transport in the same process and `AppDomain`, there is the risk of name collision between classes coming from the two transports.

To avoid name clashes, use aliases to reference types of one of the transports. For example, to define an alias for the legacy Azure Service Bus transport, add the following snippet to the project file:

```xml
<Target Name="ChangeAliasesOfNugetRefs" BeforeTargets="FindReferenceAssembliesForReferences;ResolveReferences">
  <ItemGroup>
    <ReferencePath Condition="'%(FileName)' == 'NServiceBus.Azure.Transports.WindowsAzureServiceBus'">
      <Aliases>LegacyASB</Aliases>
    </ReferencePath>
  </ItemGroup>
</Target>
```

Once the alias is defined, reference the alias in the C# source files where it is needed by adding it to the `using`s list:

snippet: transport-asb-alias-definition

The alias can now be used to reference types from the Azure Service Bus transport assembly:

snippet: transport-asb-alias-usage
