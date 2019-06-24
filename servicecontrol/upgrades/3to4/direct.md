---
title: Direct upgrade
summary: Instructions on how to directly upgrade a ServiceControl instance to version 4
isUpgradeGuide: true
---

If the ServiceControl instance is configured to ingest messages from the error queue, but not from the audit queue, it can be directly upgraded to version 4.0.0.

### Before upgrade

```mermaid
graph TD

subgraph Transport
  error(Error Queue)

  scq(Particular.ServiceControl)

  errorlog(Error Log Queue)

end

endpoints[Endpoints] --failed messages to --> error
endpoints --heartbeats and custom checks to-->scq

sc[ServiceControl 3.8.2] 

scq -->sc
error -- ingests failed messages --> sc
sc --forwards failed messages--> errorlog

sp[ServicePulse] -- http/signalr --> sc
si[ServiceInsight] --http --> sc
```

### After upgrade

```mermaid
graph TD

subgraph Transport
  error(Error Queue)

  scq(Particular.ServiceControl)

  errorlog(Error Log Queue)

end

endpoints[Endpoints] --failed messages to --> error
endpoints --heartbeats and custom checks to-->scq

sc[ServiceControl 4.0.0] 

scq -->sc
error -- ingests failed messages --> sc
sc --forwards failed messages--> errorlog

sp[ServicePulse] -- http/signalr --> sc
si[ServiceInsight] --http --> sc
```

## Upgrading with ServiceControl Management Studio

TODO

## Upgrading with Powershell

Use the following cmdlet to perform a direct upgrade of an existing ServicControl instance:

```ps
Invoke-ServiceControlInstanceUpgrade <Name of instance to upgrade>
```
