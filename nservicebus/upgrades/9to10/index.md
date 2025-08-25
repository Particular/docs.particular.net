---
title: Upgrade Version 9 to 10
summary: Instructions on how to upgrade NServiceBus from version 9 to version 10.
reviewed: 2025-08-11
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

include: upgrade-major

## Nullability

### ContextBag

The ContextBag no longer allows storing `null` as a value. In order to store `null` it is required to decorate the value with wrapper class. So instead of

```csharp
context.Set<string>("YourKey", null);
```

use

```csharp
context.Set<string>("YourKey", new YourKeyValue());

public record YourKeyValue(string? someValueThatMightBeNull = null);
```
