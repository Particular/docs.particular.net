---
title: Release policy
summary: The meaning of version numbers in the Particular Software Platform
reviewed: 2021-04-16
redirects:
 - nservicebus/release-policy
related:
 - nservicebus/licensing
---

NServiceBus and the entire Particular Service Platform are designed as a collection of components, each released with regular cadence. Important bug fixes are backported to all [supported versions](/nservicebus/upgrades/support-policy.md), enabling users to upgrade with minimal risk.

While this may result in a large number of releases, only a small fraction usually affect any given system.

## Semantic versioning

All components follow [SemVer 2.0.0](https://semver.org/spec/v2.0.0.html). This helps determine the urgency, risk, and effort of an upgrade.

SemVer is a simple set of rules and requirements that dictate how version numbers are assigned and incremented.

For those not familiar with SemVer, here is a short primer:

Given a version number `{major}.{minor}.{patch}`, increment the:

* `{major}` version when making incompatible API changes,
* `{minor}` version when adding functionality in a backwards compatible manner, and
* `{patch}` version when making backwards compatible bug fixes.

### Interpretations/deviations from SemVer

* Text in log and exception messages is not considered part of the public API.

## Backporting important bug fixes

Although not stipulated by SemVer, important bug fixes are backported to all [supported versions](/nservicebus/upgrades/support-policy.md).

A system using a supported version may receive important bug fixes by upgrading to new patch versions, without the increased risk and effort of upgrading to a new minor or major version. It is strongly recommended to [upgrade](support-policy.md#upgrading) frequently enough to stay on a supported version.

Particular Software [support](https://particular.net/support) should be contacted if a bug fix backport is missing.

## Deprecation

Public APIs are deprecated using [ObsoleteAttribute](https://msdn.microsoft.com/en-us/library/system.obsoleteattribute.aspx) messages. Continued usage of a deprecated API will result in either a compiler warning or error.

A warning message indicates the version in which the deprecation will start to generate a compiler error and in which version the API will be removed. For example:

> warning CS0618: 'CorrelationContextExtensions.SetCorrelationId(SendOptions, string)' is obsolete: 'Setting a custom correlation ID is discouraged and will be removed in the next major version. Will be treated as an error from version 7.0.0. Will be removed in version 8.0.0.'

An error message indicates in which version the API will be removed. For example:

> error CS0619: 'CorrelationContextExtensions.SetCorrelationId(SendOptions, string)' is obsolete: 'This is no longer a public API. Will be removed in version 8.0.0.'

Deprecations in minor versions will always generate a compiler warning and not an error.

## Summary

The following table summarizes the risk, urgency, and effort associated with each version type:

|  | Patch | Minor | Major |
|---------|----------------|--------|-------|
| **Risk** | Extremely low | Medium | High |
| **Urgency** | Medium to High | Low | Low |
| **Effort** | Extremely low | Low | High |

### Patch

Patch versions are released as soon as an important bug is found. The release notes contain details of symptoms and instructions for how to determine if a system is affected.

Patch versions are backwards compatible, as stipulated by SemVer, and contain minimal code changes, so upgrading has extremely low risk and effort. In rare cases, a code change may be required as part of the upgrade.

Minimal testing may also be required, including verification that the patch fixes the bug.

### Minor

Minor versions contain feature enhancements and may also contain bug fixes which are not important enough to warrant a patch version.

Minor versions are backwards compatible, as stipulated by SemVer. They contain more code changes than patch versions so the risk of upgrading is slightly higher. It is also likely that code changes are required as part of the upgrade, to take advantage of the feature enhancements.

Since all important bug fixes are back-ported, upgrades may be done at convenient times. For example, when releasing non-trivial updates to a system.

### Major

Major versions contain breaking changes and may also contain feature enhancements. They may also contain bug fixes which are not important enough to warrant a patch.

Major versions are not backwards compatible. They contain more code changes than minor versions so the risk of upgrading is higher. While the breaking changes may not affect all systems, upgrading is likely to require code changes.

It is recommended to fully regression test a system after upgrading.

## Release quality

The release cycle consists of the following quality stages:

### Alpha

* The component may be not feature complete.
* Binaries are considered unstable.
* APIs may change without notice.
* Packages are used for internal development, updating documentation, etc.

### Beta

* The component is feature complete.
* Likely to contain a number of known or unknown bugs.
* Performance and stability testing has not been fully completed.
* This version does not come with a "go live" license and **is not supported** in production.
* All public APIs should be stable but may change based on feedback from consumers.

#### Open and closed beta

* Closed beta versions are released to a restricted group of Particular Software customers, by invitation.
* Open beta versions are public and available to all users.
* Particular Software may be [contacted](https://particular.net/contactus) for requests to be included in current and future closed beta groups.

### Release Candidate (RC)

* Intended to be a equivalent to a final version, which is ready to release unless significant bugs are discovered.
* In this stage of stabilization, all features have been designed, coded, and tested through one or more beta cycles and the version has no known important bugs.
* There may still be changes to fix discovered bugs, changes to documentation and data files, and peripheral code for test cases or utilities.
* This version comes with a "go live" license and **is supported** in production.

### Release To Market (RTM)

This is the stable production release.
