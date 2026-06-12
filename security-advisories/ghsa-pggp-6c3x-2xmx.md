---
title: "GHSA-pggp-6c3x-2xmx"
summary: "Snappier has an infinite loop during SnappyStream decompression with malformed framed input"
reviewed: "2026-05-13"
---

## Security Advisory Id GHSA-pggp-6c3x-2xmx

This advisory discloses a security vulnerability 
Patches for components to update their dependencies to avoid references that have the [GHSA-pggp-6c3x-2xmx](https://github.com/advisories/ghsa-pggp-6c3x-2xmx) security advisory: Snappier has an infinite loop during SnappyStream decompression with malformed framed input.

### Patch releases

| Component | Version | Where to get it |
| --------- | ------- | --------------- |
|NServiceBus.Storage.MongoDB|3.0.7|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB/3.0.7)|
|NServiceBus.Storage.MongoDB.TransactionalSession|3.0.7|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB.TransactionalSession/3.0.7)|
|NServiceBus.Storage.MongoDB|4.2.2|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB/4.2.2)|
|NServiceBus.Storage.MongoDB.TransactionalSession|4.2.2|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB.TransactionalSession/4.2.2)|
|NServiceBus.Storage.MongoDB|5.0.2|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB/5.0.2)|
|NServiceBus.Storage.MongoDB.TransactionalSession|5.0.2|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB.TransactionalSession/5.0.2)|
|NServiceBus.Storage.MongoDB|6.0.3|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB/6.0.3)|
|NServiceBus.Storage.MongoDB.TransactionalSession|6.0.3|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB.TransactionalSession/6.0.3)|
|NServiceBus.Storage.MongoDB|7.0.2|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB/7.0.2)|
|NServiceBus.Storage.MongoDB.TransactionalSession|7.0.2|[NuGet](https://www.nuget.org/packages/NServiceBus.Storage.MongoDB.TransactionalSession/7.0.2)|

### How to know if you are affected

You are affected if you are using previous versions of any of these components, but this doesn't necessarily mean you are vulnerable.

### Symptoms

For NuGet packages your projects have the setting `NuGetAuditMode` set to `all` and see transitive dependency warnings at build time that mention Particular packages.

Other components of the platform will not have any symptoms.

### When to upgrade

You should upgrade immediately if you are affected. Otherwise, you should upgrade during your next maintenance window.
