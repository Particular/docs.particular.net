---
title: NHibernate Persistence Upgrade Version 10 to 11
summary: Migration instructions on how to upgrade the NHibernate persistence from version 10 to 11.
reviewed: 2026-02-04
component: NHibernate
related:
- persistence/nhibernate
- nservicebus/upgrades/9to10
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

## Saga mapping loading with disabled assembly scanning

When [assembly scanning is disabled](/nservicebus/hosting/assembly-scanning.md#disable-assembly-scanning), saga mapping discovery has been tightened. Previously, all assemblies discovered via GetAvailableTypes() were registered with NHibernate, allowing saga mappings to be picked up from any scanned assembly, even if it did not contain saga entities. Now, saga entity types are resolved first from the registered saga metadata, and only assemblies that actually contain those saga entities are treated as the authoritative source of mappings.

As a result, NHibernate no longer pulls in unrelated mappings from arbitrary scanned assemblies. If you rely on saga-related mappings that live in assemblies which do not directly contain saga entities, those mappings must now be manually registered with NHibernate to ensure they are included.
