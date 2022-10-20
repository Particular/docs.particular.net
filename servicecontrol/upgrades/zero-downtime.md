---
title: Zero downtime upgrades
summary: Instructions on how to upgrade ServiceControl instances with zero downtime
isUpgradeGuide: true
reviewed: 2022-10-20
---

The [ServiceControl remotes feature](/servicecontrol/servicecontrol-instances/remotes.md) can be used to upgrade a ServiceControl installation without taking it offline.

## Audit instances

The process follows these steps:

1. Add a new audit instance as a remote
1. Disable audit queue management on the old audit instance
1. Decommission the old audit instance, when it is empty

### Initial state

Before doing anything, the deployment looks like this:

```mermaid
graph TD
endpoints -- send errors to --> errorQ[Error Queue]
endpoints -- send audits to --> auditQ[Audit Queue]
errorQ -- ingested by --> sc[ServiceControl<br/>primary]
auditQ -- ingested by --> sca[Original<br/>ServiceControl<br/>audit]
sc -. connected to .-> sca
sp[ServicePulse] -. connected to .-> sc
si[ServiceInsight] -. connected to .-> sc

classDef Endpoints fill:#00A3C4,stroke:#00729C,color:#FFFFFF
classDef ServiceInsight fill:#878CAA,stroke:#585D80,color:#FFFFFF
classDef ServicePulse fill:#409393,stroke:#205B5D,color:#FFFFFF
classDef ServiceControlPrimary fill:#A84198,stroke:#92117E,color:#FFFFFF,stroke-width:4px
classDef ServiceControlRemote fill:#A84198,stroke:#92117E,color:#FFFFFF

class endpoints Endpoints
class si ServiceInsight
class sp ServicePulse
class sc ServiceControlPrimary
class sca ServiceControlRemote
```

### Add a new audit instance

Create a new audit instance, and configure it as a remote instance of the primary instance.

```mermaid
graph TD
endpoints -- send errors to --> errorQ[Error Queue]
endpoints -- send audits to --> auditQ[Audit Queue]
errorQ -- ingested by --> sc[ServiceControl<br/>primary]
auditQ -- ingested by --> sca[Original<br/>ServiceControl<br/>audit]
auditQ -- ingested by --> sca2[New<br/>ServiceControl<br/>audit]
sc -. connected to .-> sca
sc -. connected to .-> sca2
sp[ServicePulse] -. connected to .-> sc
si[ServiceInsight] -. connected to .-> sc

classDef Endpoints fill:#00A3C4,stroke:#00729C,color:#FFFFFF
classDef ServiceInsight fill:#878CAA,stroke:#585D80,color:#FFFFFF
classDef ServicePulse fill:#409393,stroke:#205B5D,color:#FFFFFF
classDef ServiceControlPrimary fill:#A84198,stroke:#92117E,color:#FFFFFF,stroke-width:4px
classDef ServiceControlRemote fill:#A84198,stroke:#92117E,color:#FFFFFF

class endpoints Endpoints
class si ServiceInsight
class sp ServicePulse
class sc ServiceControlPrimary
class sca,sca2 ServiceControlRemote
```

Although both ServiceControl Audit instances ingest messages from the audit queue, each message only ends up in a single instance. The primary instance queries both transparently.

### Disable audit queue management on the old instance

Update the audit queue configuration on the original Audit instance to the value `!disable`.

```mermaid
graph TD
endpoints -- send errors to --> errorQ[Error Queue]
endpoints -- send audits to --> auditQ[Audit Queue]
errorQ -- ingested by --> sc[ServiceControl<br/>primary]
auditQ -- ingested by --> sca2[New<br/>ServiceControl<br/>audit]
sc -. connected to .-> sca[Original<br/>ServiceControl<br/>audit]
sc -. connected to .-> sca2
sp[ServicePulse] -. connected to .-> sc
si[ServiceInsight] -. connected to .-> sc

classDef Endpoints fill:#00A3C4,stroke:#00729C,color:#FFFFFF
classDef ServiceInsight fill:#878CAA,stroke:#585D80,color:#FFFFFF
classDef ServicePulse fill:#409393,stroke:#205B5D,color:#FFFFFF
classDef ServiceControlPrimary fill:#A84198,stroke:#92117E,color:#FFFFFF,stroke-width:4px
classDef ServiceControlRemote fill:#A84198,stroke:#92117E,color:#FFFFFF

class endpoints Endpoints
class si ServiceInsight
class sp ServicePulse
class sc ServiceControlPrimary
class sca,sca2 ServiceControlRemote
```

The primary instance continues to query both instances but the original Audit instance no longer reads new messages.

### Decommission the old audit instance, when it is empty

As the original audit instance is no longer ingesting messages, it will be empty after the audit retention period has elapsed and can be removed.

```mermaid
graph TD
endpoints -- send errors to --> errorQ[Error Queue]
endpoints -- send audits to --> auditQ[Audit Queue]
errorQ -- ingested by --> sc[ServiceControl<br/>primary]
auditQ -- ingested by --> sca2[New<br/>ServiceControl<br/>audit]
sc -. connected to .-> sca2
sp[ServicePulse] -. connected to .-> sc
si[ServiceInsight] -. connected to .-> sc

classDef Endpoints fill:#00A3C4,stroke:#00729C,color:#FFFFFF
classDef ServiceInsight fill:#878CAA,stroke:#585D80,color:#FFFFFF
classDef ServicePulse fill:#409393,stroke:#205B5D,color:#FFFFFF
classDef ServiceControlPrimary fill:#A84198,stroke:#92117E,color:#FFFFFF,stroke-width:4px
classDef ServiceControlRemote fill:#A84198,stroke:#92117E,color:#FFFFFF

class endpoints Endpoints
class si ServiceInsight
class sp ServicePulse
class sc ServiceControlPrimary
class sca2 ServiceControlRemote
```