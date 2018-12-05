---
title: Support Policy
summary: Versions of NServiceBus and component packages that are currently supported
reviewed: 2018-11-23
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/supported-versions
 - nservicebus/upgrades/supported-platforms
---

## Supported versions

Supported versions of NServiceBus and its associated packages are determined by the release date of the package that replaces them. After a specific version of a package is replaced by a newer version, the older package will remain supported for a fixed period of time afterward. This enables starting a project with the newest versions at the time, with the confidence that those versions will be supported well into the future, regardless of the release schedule for new components.

### NServiceBus package

- Major versions are supported for a period of two years after the release of the next major version.
- Minor versions of a supported major version are supported for a period of six months after the release of the next minor version.
- Only the latest patch release of each supported minor version is supported.

### Component packages that depend upon NServiceBus

- Major versions are supported for a period of one year after the release of the next major version.
- Minor versions of a supported major version are supported for a period of three months after the release of the next minor version.
- Only the latest patch release of each supported minor version is supported.
- A version can only be supported if the NServiceBus package on which it depends is also currently supported.
- If the version is the most recent version which works with a given version of NServiceBus, the version will be supported for as long as that version of NServiceBus is supported.

WARNING: If support is required for a version that is not listed or has expired, contact [Particular Software Support](https://particular.net/contactus).

Particular will apply critical bug fixes to all supported versions. If a bug also impacts a non-supported version, the recommended approach is to upgrade to a supported version which contains the fix.

For more information refer to [the release policy](/nservicebus/upgrades/release-policy.md).

include: supported-versions-nservicebus

For other NServiceBus packages, refer to [the list of supported versions of all packages](supported-versions.md).


## Extended support for NServiceBus version 5

Extended support is helpful when more time is required to upgrade an existing system from an unsupported version of NServiceBus to a supported version. To qualify for extended support, a valid support agreement with Particular Software is required. Extended support provides an extra two years at the end of mainstream support. During this period:

- Samples for unsupported versions are still provided for all customers.
- If a critical bug in an unsupported version is detected and reported by a customer with an extended support agreement, Particular Software will provide a patch release.


## Compatibility guarantees

All new features are backward compatible by default. In rare cases when this is not possible, the new feature will be disabled by default and require an explicit opt-in in order to be enabled. These features are covered in the [upgrade guide](/nservicebus/upgrades/) for the new target version.


## Upgrading

NServiceBus versions are wire-compatible; endpoints using different versions of NServiceBus can exchange messages with each other.

However, some features might require data migration (e.g. converting from an old to a new format), and the conversion might be performed in the background after upgrade.

Therefore the recommended approach is to upgrade *one* major version at a time, including a full regression test of the system and deployment to production after each major version upgrade.


## FAQ

**Does this policy apply to the [ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/), or [ServicePulse](/servicepulse/) applications?**

No, the policy applies only to the supported NServiceBus packages listed on the [NServiceBus Packages Supported Versions](supported-versions.md) page.

**Which version of NServiceBus should be used to start a new project?**

It's recommended to use the newest major version of NServiceBus when starting a new project. The support policy guarantees that the major version will be supported for at least two years after it is replaced by a new major version.

**Why are fixes backported to old minor versions?**

In accordance with the [release policy](release-policy.md), important fixes are backported to older (but still supported) minor versions to reduce the risk of upgrading. A component can be upgraded to a newer patch release without requiring an upgrade to the newest minor version containing new functionality.

**Why are support intervals for NServiceBus longer than for component packages?**

Component packages, such as [transports](/transports/), [persisters](/persistence/), [dependency injection](/nservicebus/dependency-injection/), or [serializers](/nservicebus/serialization/), have much less upgrade risk than the NServiceBus package. Therefore, the commitment to backporting fixes affecting the NServiceBus package is higher than for the many component packages.

The support dates for NServiceBus and each component package can be found on the [supported versions](supported-versions.md) page.
