---
title: Support Policy
summary: Versions that are currently supported
reviewed: 2016-09-24
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
---

## Supported Versions

Supported versions include any major version released within the last 3 years. Within a supported major version, the latest minor version is supported, or any minor version released within the last year.

WARNING: If a version is not listed or has expired, but support is required contact [Particular support](https://particular.net/support).

Particular will apply critical bug fixes to all supported versions. If the bug also impacts a non supported version, the recommended approach is to upgrade to a supported version which contains the fix.

For more information refer to [the release policy](/nservicebus/upgrades/release-policy.md).


include: support-dates


## Compatibility guarantees

All new features are backwards compatible by default. In the rare cases when this is not possible, the new feature would be disabled by default and require an explicit opt-in to be enabled. These types of features are covered in the [Upgrade Guide](/nservicebus/upgrades/) for the new target version.


## Upgrading

NServiceBus versions are wire-compatible, meaning the endpoints using different versions can exchange messages with each other.

However, some features might require data migration (e.g. converting from old to new format), and the conversion might be performed in the background after upgrade.

Therefore the recommended approach is to upgrade 1 major version at a time, including a full regression testing of the system and deployment to production.
