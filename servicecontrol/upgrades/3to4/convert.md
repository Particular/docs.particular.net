---
title: Convert to ServiceControl Audit instance
summary: Instructions on how to convert a ServiceControl instance into a ServiceControl Audit instance
isUpgradeGuide: true
---

If the ServiceControl instance is configured to ingest messages from the audit queue, but not from the error queue, it can be converted to a ServiceControl Audit instance.

NOTE: In this case, it is assumed that this instance was a part of a multi-instance installation. There must be a main instance (not pictured below) for the ServiceControl Audit instance to connect to.

### Before upgrade

```mermaid
graph TD

subgraph Transport
  audit(Audit Queue)

  scq(Particular.ServiceControl)

  auditlog(Audit Log Queue)

end

endpoints[Endpoints] --audits to --> audit
endpoints --heartbeats and custom checks to-->scq

sc[ServiceControl 3.8.2] 

scq -->sc
audit -- ingests audited messages --> sc
sc --forwards audited messages--> auditlog

sp[ServicePulse] -- http/signalr --> sc
si[ServiceInsight] --http --> sc
```

### After upgrade

```mermaid
graph TD

subgraph Transport
  audit(Audit Queue)

  scq(Particular.ServiceControl)

  auditlog(Audit Log Queue)

end

endpoints[Endpoints] --audits to --> audit

sca[ServiceControl Audit 4.0.0] 

scq -->sc
audit -- ingests audited messages --> sc
sc --forwards audited messages--> auditlog
```

## Upgrading with ServiceControl Management Studio

TODO

## Upgrading with Powershell

Use the following cmdlet to convert an existing ServicControl instance into a ServiceControl Audit instance:

```ps
Invoke-ServiceControlInstanceConvert `
  -Name <Name of instance to convert> `
  -ServiceControlAddeess <Name of the main instance to report to>`
  [-ServiceAccountPassword <password for service account>]
```

NOTE: If this instance uses a domain account, the the account password must be supplied