## So there is a new version of NServiceBus available, how/when should I update?

To reduce scope risk we have optimized to release small components with a regular cadence. While this may seem to result in a large number of releases only a small fraction of these will actually affect you. This is how you should approach upgrades.


### Semantic Versioning 

[Semver](http://semver.org/) is a simple set of rules and requirements that dictate how version numbers are assigned and incremented. 

For those of you not familiar with SemVer here is a short primer:

Given a version number `{major}.{minor}.{patch}`, increment the:

* `{major}` version when you make incompatible API changes,
* `{minor}` version when you add functionality in a backwards-compatible manner, and
* `{patch}` version when you make backwards-compatible bug fixes.

By following SemVer 2.0 you will be able to quickly determine the urgency, risk and effort of the upgrade. 


### We backport important bugfixes

While not stipulated by SemVer we've made the decision to backport important fixes to all supported versions of NServiceBus. By supported we mean any minor version released with in the last year and all major versions withing the last 3 years. This means that you will get critical bugfixes without the associated risk and effort of upgrading to a higher `{major}.{minor}` version. Please let us know if there are any bugfixes that you believe should be backported to your current version.



## Patch
Patches are put on on demand as soon as issues triaged as important is found
### Risk
Extremely low. Note that patch releases are 100% backwards compatible so you should be able safely upgrade with low risk and effort.

### Urgency

Read the release notes and consider upgrading if you're affected by any of the issues fixed.

Any issues that could affect the production stability of your system will be classified as `hotfix`'s so please pay extra attention to those.

### Effort 

Minimal, in rare case a code change might be required in relation to the patch. Minimal testing required but where possible verify that it fixes the issue in question.

## Minor
Minor versions are released every 1-2 months and contains features and bugfixes. Note that these releases are as stipulated by SemVer 100% backwards compatible.

### Risk
Medium. Since adding new feature requires more code to be added/changed the risk is higher compared to a patch release.

### Urgency
Low. Since all critical issues will be backported. 

### Effort 
Low. Some new features will require code changes to take advantage of them

## Major
Majors version are release rougly once per year.

### Risk
High

### Urgency
Low

### Effort 
High. Since a new major version will contain breaking changes you will need to update code. Because of the extent of code changes in a major version you should expect to do a full regression test of your solution.


## How does this effect my support agreement?

Do we need this in here? 

