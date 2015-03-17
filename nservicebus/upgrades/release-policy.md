---
title: Release Policy
summary: What version numbers mean to Particular
tags: []
redirects:
 - nservicebus/release-policy
---

To reduce scope and risk we have optimized to release small components with a regular cadence. We're also committed to backport all important bugfixes to your specific version, this allows you to stay updated while keeping the risk of upgrading to a minimum.

While this may seem to result in a large number of releases only a small fraction of these will actually affect your system.

### Semantic Versioning 

[SemVer](http://semver.org/) is a simple set of rules and requirements that dictate how version numbers are assigned and incremented. 

For those of you not familiar with SemVer here is a short primer:

Given a version number `{major}.{minor}.{patch}`, increment the:

* `{major}` version when you make incompatible API changes,
* `{minor}` version when you add functionality in a backwards-compatible manner, and
* `{patch}` version when you make backwards-compatible bug fixes.

By following SemVer 2.0 you will be able to quickly determine the urgency, risk and effort of the upgrade. 


### We backport important bugfixes

While not stipulated by SemVer we've made the decision to backport important fixes to all supported versions of NServiceBus.

By "supported" we mean any minor version released within the last year and all major versions released within the last 3 years. This means that you will get critical bugfixes without the associated risk and effort of upgrading to a higher `{major}.{minor}` version. 

We strongly recommend you upgrade frequently enough to stay on a supported version.

Please let us know if there are any bugfixes that you believe should be back-ported to your current version by emailing [support@particular.net](mailto:support@particular.net).

## Summary
The following table summarize the risk effort and urgency for the different types of releases

|  | Patch | Minor | Major |
|---------|----------------|--------|-------|
| Risk | Extremely low | Medium | High |
| Urgency | High to medium | Low | Low |
| Effort | Minimal | Low | High |
| Frequency | As needed | Every 1-2 Months | Yearly |


### Patch
Patches are released as soon as an important issue is found. Read the release notes to determine if you're affected. 

NOTE: Any issue that could affect the production stability of your system will be classified as `hotfix` so please pay extra attention to those.

Patch releases are 100% backwards-compatible so you should be able safely upgrade with low risk and effort. In rare cases a code change might be required in relation to the patch. Minimal testing may be required, but where possible verify that it fixes the issue in question. 

### Minor
Minor versions contain bugfixes not critical enough to warrant a `patch` as well as some feature enhancements. 

Note that these releases are 100% backwards-compatible (as stipulated by SemVer). Since adding new features requires more code to be added/changed, the risk of upgrading to a `minor` is higher compared to a `patch` release and it's likely that code changes will be needed to take advantage of them. 

Since all critical issues will be back-ported, you can choose to upgrade when it's suitable for you, for example as part of releasing non-trivial updates to your own system.

### Major

Since a new `major` version will contain breaking changes you will most probably need to update your code. Because of the extent of code changes in a major version, it is recommendation that you do a full regression test of your system.
