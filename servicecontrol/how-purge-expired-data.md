---
title: Automatic Expiration of ServiceControl Data
summary: Configuring ServiceControl data retention
tags:
- ServiceControl
- Expiration
---

ServiceControl stores audit and error data. Any audit and error data that is older than the specified mandatory thresholds is deleted from the embedded RavenDB. The expiration thresholds for both faulted and audited messages needs to be set at installation time. These value can be modified via configuration, but they are mandatory. For more information on these settings refer to [Customizing ServiceControl configuration](creating-config-file.md#data-retention).

Note: The expiration process only curates the data in the embedded RavenDB. Audit and Error forwarding queues are not curated and or managed by ServiceControl. To turn these off see [Customizing ServiceControl configuration](creating-config-file.md#transport).

**Example:**

The following settings would change the expiration for faulted messages to be 10 days and for audit messages to be 5 days.

```
<add key="ServiceControl/ErrorRetentionPeriod" value="10.00:00:00" />
<add key="ServiceControl/AuditRetentionPeriod" value="5.00:00:00" />
```

Note: For the allowed ranges for these settings refer to [Customizing ServiceControl configuration](creating-config-file.md#data-retention).