---
title: 'ServiceControl: Automatic Expiration of Data'
summary: Configuring ServiceControl data retention
related:
 - nservicebus/recoverability
 - nservicebus/operations/auditing
tags:
- ServiceControl
- Expiration
---

ServiceControl stores audit and error data. Any audit and error data that is older than the specified mandatory thresholds is deleted from the embedded RavenDB. The expiration thresholds for both faulted and audited messages needs to be set at installation time. These value can be modified later on by launching the ServiceControl Management Utility and editing the configuration settings for the instance.

Note: The expiration process only curates the data in the embedded RavenDB. Audit and Error forwarding queues are not curated and or managed by ServiceControl. To turn these off launch the ServiceControl Management Utility and edit the configuration settings for the instance.