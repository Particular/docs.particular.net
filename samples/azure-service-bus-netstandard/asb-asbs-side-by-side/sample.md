---
title: ASBS to ASB Side By Side
reviewed: 2017-10-17
component: ASBS
related:
- transports/azure-service-bus
- nservicebus/bridge
---


TBD...

NOTE:
```
  <Target Name="ChangeAliasesOfNugetRefs" BeforeTargets="FindReferenceAssembliesForReferences;ResolveReferences">
    <ItemGroup>
      <ReferencePath Condition="'%(FileName)' == 'NServiceBus.Transport.AzureServiceBus'">
        <Aliases>TransportASBS</Aliases>
      </ReferencePath>
    </ItemGroup>
  </Target>
 ```


