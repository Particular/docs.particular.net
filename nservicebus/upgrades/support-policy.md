---
title: Support Policy
summary: Versions of NServiceBus and component packages that are currently supported
reviewed: 2026-01-05
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/supported-versions
 - nservicebus/upgrades/supported-platforms
 - servicecontrol/upgrades/support-policy
redirects:
 - previews/support-policy
---

## Supported versions

Supported versions of NServiceBus and its associated packages are determined by the release date of the package that replaces them. After a specific version of a package is replaced by a newer version, the older package will remain supported for a fixed period of time afterward. This enables starting a project with the newest versions at the time, with the confidence that those versions will be supported well into the future, regardless of the release schedule for new components.

### NServiceBus package

- Major versions are supported for two years after the release of the next major version.
- Minor versions of a supported major version are supported for a period of six months after the release of the next minor version.
- Only the latest patch release of each supported minor version is supported.

### Component packages that depend upon NServiceBus

- Major versions are supported for a period of one year after the release of the next major version.
- Minor versions of a supported major version are supported for a period of three months after the release of the next minor version.
- Only the latest patch release of each supported minor version is supported.
- A version can only be supported if the NServiceBus package on which it depends is also currently supported.
- If the version is the most recent version which works with a given version of NServiceBus, the version will be supported for as long as that version of NServiceBus is supported.
- On rare occasions, the end-of-support date for a component may be limited beyond what is described above, usually due to circumstances outside of Particular's control. In these cases, the date and reasoning will be specified on the [NServiceBus packages supported versions page](supported-versions.md).

Particular will apply critical bug fixes to all supported versions. If a bug also impacts a non-supported version, the recommended approach is to upgrade to a supported version which contains the fix.

To be supported by Particular, NServiceBus must be run on a [supported version of .NET](https://dotnet.microsoft.com/en-us/platform/support/policy). See [supported frameworks and platforms](supported-platforms.md) for more information.

If support is required for a version that is not listed or has expired, contact [Particular Software Support](https://particular.net/contactus).

For more information refer to [the release policy](/nservicebus/upgrades/release-policy.md).

include: supported-versions-nservicebus

The following section provides extended support details for all NServiceBus major versions. For other NServiceBus packages, refer to [the list of supported versions of all packages](supported-versions.md).

## Extended support

Extended support is offered on major versions of NServiceBus for a period of 2 years after the expiration of mainstream support, for an additional fee. Because major versions of NServiceBus are generally supported for at least 3 years (or longer, depending upon the cadence of NServiceBus major releases), extended support affords the peace of mind that a major version of NServiceBus will be supported for at least 5 years. To qualify for extended support, a valid support agreement with Particular Software is required.

During the extended support period:

- Documentation is provided for versions under mainstream and extended support.
- Samples for extended support versions are available only for customers with an extended support agreement, upon request.
- Patches for critical bugs on extended support versions will only be provided to customers with an extended support agreement.

The following table describes the extended support status for all major versions:

include: extended-support-table

[Reach out](https://particular.net/contact) for further information or inquiries.

## Compatibility guarantees

All new features are backward compatible by default. In rare cases when this is not possible, the new feature will be disabled by default and require an explicit opt-in in order to be enabled. These features are covered in the [upgrade guide](/nservicebus/upgrades/) for the new target version.

## Upgrading

When upgrading, some features might require data migration (e.g. converting from an old to a new format). The migration might be performed as a one-off automated action done through [installers](/nservicebus/operations/installers.md) or in the background by a running endpoint. If the conversion cannot be automated, the applicable upgrade guide will contain a description of the manual process (e.g. what script to run).

Therefore the recommended approach is to upgrade *one* major version at a time, including a full regression test of the system and deployment to production after each major version upgrade. For example, if a system is using a 4.x.x version and the intention is to upgrade to the latest 6.x.x version, it is recommended to first upgrade to the latest 5.x.x version, following the relevant upgrade guides and deprecation messages. After the system is confirmed to work with the latest 5.x.x version, it may be upgraded to the latest 6.x.x version using the same process.

## FAQ

**Does this policy apply to the [ServiceControl](/servicecontrol/), [ServicePulse](/servicepulse/) applications, or [Tools](/nservicebus/tools/)?**

No, the policy applies only to the supported NServiceBus packages listed on the [NServiceBus Packages Supported Versions](supported-versions.md) page. ServiceControl and ServicePulse are governed by their own support policy, linked below.

- [ServiceControl support policy](/servicecontrol/upgrades/support-policy.md)
- [ServicePulse support policy](/servicepulse/support-policy.md)

**Which version of NServiceBus should be used to start a new project?**

It's recommended to use the newest major version of NServiceBus when starting a new project. The support policy guarantees that the major version will be supported for at least two years after it is replaced by a new major version.

**Why are fixes backported to old minor versions?**

In accordance with the [release policy](release-policy.md), important fixes are backported to older (but still supported) minor versions to reduce the risk of upgrading. A component can be upgraded to a newer patch release without requiring an upgrade to the newest minor version containing new functionality.

**Why are support intervals for NServiceBus longer than for component packages?**

Component packages, such as [transports](/transports/), [persisters](/persistence/), [dependency injection](/nservicebus/dependency-injection/), or [serializers](/nservicebus/serialization/), have much less upgrade risk than the NServiceBus package. Therefore, the commitment to backporting fixes affecting the NServiceBus package is higher than for the many component packages.

The support dates for NServiceBus and each component package can be found on the [supported versions](supported-versions.md) page.

<script type="text/javascript" src="supported-versions.js"></script>
