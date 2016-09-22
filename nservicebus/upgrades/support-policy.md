---
title: Support Policy
summary: Versions that are currently supported
reviewed: 2016-06-29
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
---

## Supported Versions

Supported versions include any major version released within the last 3 years. Within a supported major version, the latest minor version is supported, or any minor version released within the last year.

WARNING: If a version is not listed or has expired, but support is required contact [Particular support](http://particular.net/support). 

Particular guarantees patching all supported versions with critical bug fixes, even if the specific issue issue was raised only for one version. In case the bug also impacts non supported version, the recommended approach is to upgrade to the supported version which contains the fix.

For more information refer to [the release policy](/nservicebus/upgrades/release-policy.md).


### NServiceBus 5.x

| Version | Major Released | Minor Released | Support Expires |
|:-------:|----------------|----------------|:---------------:|
|   5.x   | 2014-09-24     | 2015-02-12     |    2017-09-24   |


### NServiceBus 4.x

As of 11 July 2016, Version 4 is no longer supported.


### NServiceBus 3.x

As of 10 October 2015, Version 3 is no longer supported.


## Compatibility guarantees

All new features are backwards compatible by default. In the rare cases when it's not true, enabling the new features requires an explicit opt-in. 


## Upgrading

The NServiceBus versions are wire-compatible, meaning the endpoints using different versions can exchange messages with each other. 

However, some features might require data migration (e.g. converting from old to new format), and the conversion might be performed in the background after upgrade. 

Therefore the recommended approach is to upgrade 1 major version at a time, including a full regression testing of the system and deployment to production.