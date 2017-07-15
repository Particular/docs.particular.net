---
title: Release Policy
summary: What version numbers mean to Particular.
reviewed: 2017-07-15
redirects:
 - nservicebus/release-policy
related:
 - nservicebus/licensing
---

NServiceBus and the entire Service Platform are designed as a collection of small components, each of which are then released on a regular cadence. All important bugfixes are backported to all [supported versions](/nservicebus/upgrades/support-policy.md) which enables users to stay up to date while keeping the risk of upgrading to a minimum.

While this may seem to result in a large number of releases, only a small fraction of these will actually affect any given system.


## Semantic Versioning

[SemVer](http://semver.org/) is a simple set of rules and requirements that dictate how version numbers are assigned and incremented.

For those not familiar with SemVer here is a short primer:

Given a version number `{major}.{minor}.{patch}`, increment the:

 * `{major}` version when making incompatible API changes,
 * `{minor}` version when adding functionality in a backwards-compatible manner, and
 * `{patch}` version when making backwards-compatible bug fixes.

Following SemVer 2.0 enables quickly determining the urgency, risk and effort of the upgrade.


## Backport important bugfixes

While not stipulated by SemVer, all important fixes to all supported versions are backported.

See [the support policy](/nservicebus/upgrades/support-policy.md) for more details on supported versions.

By using a supported version, critical bugfixes are received without the associated risk and effort of upgrading to a higher `{major}.{minor}` version.

It is strongly recommended to upgrade frequently enough to stay on a supported version. For the best upgrade experience, upgrade from one major version to the next major version without skipping to the one after that. For example, when using NServiceBus Version 4.7.2 and intending to upgrade to NServiceBus Version 6.0.0, first upgrade to the latest NServiceBus Version 5.x release (e.g. Version 5.2.19) and follow the suggested API upgrade guides and deprecation messages. Then repeat the exercise to upgrade from Version 5.x to Version 6.0.0.

[Contact support](https://particular.net/support) if you find any bugfixes that need to be back-ported to a specific version.


## Deprecation

Public APIs are deprecated using [ObsoleteAttribute](https://msdn.microsoft.com/en-us/library/system.obsoleteattribute.aspx) messages. Continued usage of a deprecated API will result in either a compiler warning or error.

A warning message indicates the version in which the deprecation will start to generate a compiler error and in which version the API will be removed. For example:

> warning CS0618: 'CorrelationContextExtensions.SetCorrelationId(SendOptions, string)' is obsolete: 'Setting a custom correlation ID is discouraged and will be removed in the next major version. Will be treated as an error from version 7.0.0. Will be removed in version 8.0.0.'

An error message indicates in which version the API will be removed. For example:

> error CS0619: 'Configure' is obsolete: 'This is no longer a public API. Will be removed in version 7.0.0.'

Deprecations in minor releases will always generate a compiler warning and not an error.


## Summary

The following table summarize the risk effort and urgency for the different types of releases

|  | Patch | Minor | Major |
|---------|----------------|--------|-------|
| Risk | Extremely low | Medium | High |
| Urgency | High to medium | Low | Low |
| Effort | Minimal | Low | High |


### Patch

Patches are released as soon as an important issue is found. Read the release notes to determine if affected.

NOTE: Any issue that could affect the production stability of the system will be classified as `hotfix` and hence extra attention to those.

Patch releases are 100% backwards-compatible so it is safe to upgrade with low risk and effort. In rare cases a code change might be required in relation to the patch. Minimal testing may be required, but where possible verify that it fixes the issue in question.


### Minor

Minor versions contain bugfixes not critical enough to warrant a `patch` as well as some feature enhancements.

Note that these releases are 100% backwards-compatible (as stipulated by SemVer). Since adding new features requires more code to be added/changed, the risk of upgrading to a `minor` is slightly higher compared to a `patch` release and it's likely that some code changes may be needed to take advantage of them.

Since all critical issues will be back-ported, choose to upgrade when convenient, for example as part of releasing non-trivial updates to the system.


### Major

Since a new `major` version will contain breaking changes, upgrading will likely require modifications to the consuming code. Because of the extent of code changes in a major version, it is recommendation that a full regression test of a system is done.


## Release quality

The release cycle consists of the following quality stages:


### Alpha

 * Binaries are considered unstable.
 * APIs might change without notice.
 * Packages are used for internal development, updating docs, etc.


### Beta

 * The component is feature complete.
 * Likely to contain a number of known or unknown bugs.
 * Performance and stability testing has not been fully completed.
 * This version does not come with a "go live" license and will **not be supported** in Production.
 * All public APIs should be stable but may change based on feedback from consumers.


#### Open and closed beta

 * Closed beta versions are released to a restricted group customers by invitation.
 * Open beta versions are public and target the entire user base.
 * [Contact us](https://particular.net/contactus) to join the closed beta customer group and to be included for evaluating future beta versions.


### Release Candidate (RC)

 * A Release Candidate is a version intended to be a final product, which is ready to release unless significant bugs emerge. 
 * In this stage of stabilization, all features have been designed, coded, and tested through one or more beta cycles and has no known critical bugs. 
 * There could still be source code changes to fix defects, changes to documentation and data files, and peripheral code for test cases or utilities.
 * This version comes with a "go live" license and will **be supported** in Production.


### Release To Market (RTM)

This is the stable production release.
