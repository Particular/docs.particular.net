---
title: Support Policy
summary: Versions that are currently supported
reviewed: 2017-04-01
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/supported-versions
 - nservicebus/upgrades/supported-platforms
---

## Supported Versions

Supported versions of NServiceBus and its associated packages are determined by the release date of the package that replaces them. After a specific version of a package is replaced by a newer version, the older package will remain supported for a fixed period of time afterward. This enables starting a project with the newest versions at the time, with the confidence that those versions will be supported well into the future, regardless of the release schedule for new components.

### NServiceBus package

* Major versions are supported for a period of 2 years after the release of the next major version.
* Minor versions of a supported major version are supported for a period of 6 months after the release of the next minor version.
* Only the latest patch release of each supported minor version is supported.

### Component packages that depend upon NServiceBus

* Major versions are supported for a period of 1 year after the release of the next major version.
* Minor versions of a supported major version are supported for a period of 3 months after the release of the next minor version.
* Only the latest patch release of each supported minor version is supported.
* A version can only be supported if the NServiceBus package on which it depends is also currently supported.
* If the version is the most recent version which works with a given version of NServiceBus, the version will be supported for as long as that version of NServiceBus is supported.

WARNING: If support is required for a version that is not listed or has expired, contact [Particular Software Support](https://particular.net/support).

Particular will apply critical bug fixes to all supported versions. If the bug also impacts a non supported version, the recommended approach is to upgrade to a supported version which contains the fix.

For more information refer to [the release policy](/nservicebus/upgrades/release-policy.md).


include: supported-versions-nservicebus

For other NServiceBus packages, refer to [the list of supported versions of all packages](supported-versions.md).


## Compatibility guarantees

All new features are backwards compatible by default. In the rare cases when this is not possible, the new feature would be disabled by default and require an explicit opt-in to be enabled. These types of features are covered in the [Upgrade Guide](/nservicebus/upgrades/) for the new target version.


## Upgrading

NServiceBus versions are wire-compatible, meaning the endpoints using different versions can exchange messages with each other.

However, some features might require data migration (e.g. converting from old to new format), and the conversion might be performed in the background after upgrade.

Therefore the recommended approach is to upgrade 1 major version at a time, including a full regression testing of the system and deployment to production.


## FAQ

**Does this policy apply to the [ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/), or [ServicePulse](/servicepulse/) applications?**

No, the policy applies only to the supported NServiceBus packages listed on the [NServiceBus Packages Supported Versions](supported-versions.md) page.

**Which version of NServiceBus should be used to start a new project?**

It's preferable to use the newest major version of NServiceBus when starting a new project. The support policy guarantees that the major version will be supported for at least 2 years after it is replaced by a new major version.

**Why are fixes backported to old minor versions?**

In accordance with the [release policy](release-policy.md), important fixes are backported to older (but still supported) minor versions to reduce the risk of upgrading. A component can be upgraded to a newer patch release without requiring an upgrade to the newest minor version containing new functionality.

**Why are support intervals for NServiceBus longer than for component packages?**

Component packages, such as [transports](/transports/), [persistence](/persistence/), [dependency injection](/nservicebus/dependency-injection/), or [serializers](/nservicebus/serialization/), have much less upgrade risk than the NServiceBus package. Therefore, the commitment to backporting fixes affecting the NServiceBus package is higher than for the many component packages.

The support dates for NServiceBus and each component package can be found on the [supported versions](supported-versions.md) page.
