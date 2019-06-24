---
title: Split ServiceControl instance
summary: Instructions on how to split a ServiceControl instance
isUpgradeGuide: true
---

If the ServiceControl instance is configured to ingest both audit and error messages before the upgrade, then the instance will be split into two separate processes: a ServiceControl instance, and a ServiceControl Audit instance.

All other Particular Service Platform components should continue to connect to the main ServiceControl instance.

### Before upgrade

```mermaid
graph TD

subgraph Transport
  audit(Audit Queue)
  error(Error Queue)

  scq(Particular.ServiceControl)

  auditlog(Audit Log Queue)
  errorlog(Error Log Queue)

end

endpoints[Endpoints] -- audits to --> audit
endpoints --failed messages to --> error
endpoints --heartbeats and custom checks to-->scq

sc[ServiceControl 3.8.2] 

scq -->sc
audit -- ingests audits --> sc
error -- ingests failed messages --> sc
sc --forwards audited messages --> auditlog
sc --forwards failed messages--> errorlog

sp[ServicePulse] -- http/signalr --> sc
si[ServiceInsight] --http --> sc
```

### After upgrade

```mermaid
graph TD

subgraph Transport

  scq(Particular.ServiceControl)

  audit(Audit Queue)
  error(Error Queue)
  auditlog(Audit Log Queue)
  errorlog(Error Log Queue)

end

endpoints[Endpoints] -- audits to --> audit
endpoints --failed messages to --> error
endpoints --heartbeats and custom checks to-->scq


sc[ServiceControl 4] 
sca[ServiceControl Audit 4]

scq -->sc
audit -- ingests audits --> sca
error -- ingests failed messages --> sc
sca --forwards audited messages --> auditlog
sc --forwards failed messages--> errorlog

sc --http queries-->sca
sca--notifications-->scq

sp[ServicePulse] -- http/signalr --> sc
si[ServiceInsight] --http --> sc
```

## Upgrading with ServiceControl Management Studio

TODO

## Upgrading with Powershell

Use the following cmdlet to split an existing ServicControl instance:

```ps
Invoke-ServiceControlInstanceSplit `
  -Name <Name of main instance> `
  -InstallPath <Path for Audit instance binaries> `
  -DBPath <Path for the Audit instance database> `
  -LogPath <Path for the Audit instance logs> `
  -Port <Port for the Audit instance api> `
  -DatabaseMaintenancePort <Port for the Audit instance embedded database> `
  [-ServiceAccountPassword <password for service account>] `
  [-Force]
```

The following information is copied from the existing ServiceControl instance:

- Audit queue
- Audit log queue
- Forward audit messages
- Audit retention period
- Transport
- Connection string
- Host name
- Service account (NOTE: If this instance uses a domain account, the the account password must be supplied)

The name of the new audit instance will be derived from the name of the original instance.

