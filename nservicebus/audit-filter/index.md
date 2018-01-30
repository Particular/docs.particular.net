---
title: Audit Filter
summary: Filtering what messages are sent to the audit Queue
component: AuditFilter
reviewed: 2018-01-26
related:
 - samples/audit-filter
---

`NServiceBus.AuditFilter` adds support for filtering which messages are sent to the [Audit Queue](/nservicebus/operations/auditing.md).


## Usage


### Decorate messages with attributes

snippet: MessageToIncludeAudit

snippet: MessageToExcludeFromAudit


### Add to EndpointConfiguration

With include by default

snippet: DefaultIncludeInAudit

With exclude by default

snippet: DefaultExcludeFromAudit


### Delegate filter fallback

The fallback/default value can also be controlled by a delegate.

snippet: FilterAuditByDelegate


## Include/Exclude logic flow

```mermaid

graph TD

HasExcludeAttribute{Has Exclude<br />Attribute?}
HasIncludeAttribute{Has Include<br />Attribute?}

Include[Include in Audit]
Exclude[Exclude From Audit]
Default{What is the<br /> default?}

HasFilter{Has Filter<br />Defined?}
WhatFilter{Filter return<br /> value?}

HasIncludeAttribute --Yes--> Include
HasIncludeAttribute --No--> HasExcludeAttribute
HasExcludeAttribute --No--> HasFilter
HasExcludeAttribute --Yes--> Exclude
HasFilter--Yes--> WhatFilter
WhatFilter--Include--> Include
WhatFilter--Exclude--> Exclude
HasFilter--No--> Default
Default--Include--> Include
Default--Exclude--> Exclude

```
