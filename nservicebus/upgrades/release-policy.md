---
title: Release Policy
summary: What version numbers mean to Particular.
reviewed: 2016-10-11
redirects:
 - nservicebus/release-policy
related:
 - nservicebus/licensing
---

To reduce scope and risk small components are released with a regular cadence. Particular is committed to backport all important bugfixes to all [supported versions](/nservicebus/upgrades/support-policy.md), this enables staying up to date while keeping the risk of upgrading to a minimum.

While this may seem to result in a large number of releases only a small fraction of these will actually affect the system.


### Semantic Versioning

[SemVer](http://semver.org/) is a simple set of rules and requirements that dictate how version numbers are assigned and incremented.

For those not familiar with SemVer here is a short primer:

Given a version number `{major}.{minor}.{patch}`, increment the:

 * `{major}` version when make incompatible API changes,
 * `{minor}` version when add functionality in a backwards-compatible manner, and
 * `{patch}` version when make backwards-compatible bug fixes.

Following SemVer 2.0 enable quickly determining the urgency, risk and effort of the upgrade.


### Backport important bugfixes

While not stipulated by SemVer all important fixes to all supported versions of NServiceBus are backported.

See [support policy](/nservicebus/upgrades/support-policy.md) for more details on supported versions.

By using a supported version critical bugfixes are received without the associated risk and effort of upgrading to a higher `{major}.{minor}` version.

Some examples:

 * Version 4.0 was released on 2013-07-11 and Version 4 will therefore be supported until 2016-07-11 but only if on the latest minor or a minor version released within the last year.
 * Version 4.6 was released on 2014-05-01 this means that its latest patch release will be supported till 2015-05-01. This means that there will be no fixes minor versions after 2015-05-01. It is required to update to at least a newer minor version that is still supported as this version will receive a patch release.
 * A newer patch release will automatically mean that the previous patch release will be obsolete. Particular will apply a bugfix on the latest patch release but will not officially release a patch for a obsolete patch release. In other words, version X.Y.3 will not be patched (to version X.Y.3.1) when version X.Y.4 is the latest patch. That would then be released as version X.Y.5

It is strongly recommend to upgrade frequently enough to stay on a supported version. For a best upgrade experience upgrade from major version to major version. For example if using NServiceBus Version 3.3.15 and want to upgrade to NServiceBus Version 5.2.3. First upgrade to the latest NServiceBus Version 4.x release (i.e. Version 4.7.6). Follow the suggested API upgrade guides and [ObsoleteAttribute messages](https://msdn.microsoft.com/en-us/library/system.obsoleteattribute.aspx) and then upgrade to NServiceBus Version 5.2.3.

[Contact support](https://particular.net/support) if there are any bugfixes that believe should be back-ported to a specific version.


## Summary

The following table summarize the risk effort and urgency for the different types of releases

|  | Patch | Minor | Major |
|---------|----------------|--------|-------|
| Risk | Extremely low | Medium | High |
| Urgency | High to medium | Low | Low |
| Effort | Minimal | Low | High |
| Frequency | As needed | Every 1-2 Months | Yearly |


### Patch

Patches are released as soon as an important issue is found. Read the release notes to determine if affected.

NOTE: Any issue that could affect the production stability of the system will be classified as `hotfix` and hence extra attention to those.

Patch releases are 100% backwards-compatible so it is safe to upgrade with low risk and effort. In rare cases a code change might be required in relation to the patch. Minimal testing may be required, but where possible verify that it fixes the issue in question.


### Minor

Minor versions contain bugfixes not critical enough to warrant a `patch` as well as some feature enhancements.

Note that these releases are 100% backwards-compatible (as stipulated by SemVer). Since adding new features requires more code to be added/changed, the risk of upgrading to a `minor` is higher compared to a `patch` release and it's likely that code changes will be needed to take advantage of them.

Since all critical issues will be back-ported, choose to upgrade when convenient, for example as part of releasing non-trivial updates to the system.


### Major

Since a new `major` version will contain breaking changes and will likely require modifications to the consuming code. Because of the extent of code changes in a major version, it is recommendation that a full regression test of the system is done.


### Release quality

The release cycle consists of the following quality stages:

 * Alpha
 * Beta (Optional)
 * Release Candidate (Optional)
 * Stable


### Alpha

 * Binaries are considered unstable.
 * APIs might change without notice.
 * Packages are used for internal development, updating docs, etc.


### Beta

 * The component is feature complete.
 * Likely to contain a number of known or unknown bugs.
 * Performance and stability testing has not been fully completed.
 * **No** "go live" license is provided and production use is **not supported**.
 * All public APIs should be stable but may change based on feedback from consumers.


#### Open and closed beta

 * Closed beta versions are released to a restricted group customers by invitation.
 * Open beta versions are public and target the entire user base.


### Release Candidate (RC)

A Release Candidate is a version intended to be a final product, which is ready to release unless significant bugs emerge. In this stage of stabilization, all features have been designed, coded and tested through one or more beta cycles and has no known critical bugs. There could still be source code changes to fix defects, changes to documentation and data files, and peripheral code for test cases or utilities.


### Release To Market (RTM)

This is the stable production release.
