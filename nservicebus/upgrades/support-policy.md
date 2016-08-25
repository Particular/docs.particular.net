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

If a version is not listed or has expired but support is required [contact support](https://particular.net/contactus). For more information on the relationship between releases and support see [the release policy](/nservicebus/upgrades/release-policy.md).

WARNING: If support is required for **any version** [contact support](https://particular.net/contactus).


### NServiceBus 5.x

| Version | Major Released | Minor Released | Support Expires |
|:-------:|----------------|----------------|:---------------:|
|   5.x   | 2014-09-24     | 2015-02-12     |    2017-09-24   |


### NServiceBus 4.x

As of 11 July 2016, Version 4 is no longer supported.


### NServiceBus 3.x

As of 10 October 2015, Version 3 is no longer supported.


## Compatibility guarantees

The compatibility guarantees cover two aspects: upgrades and communication between endpoints in various versions (so called `wire compatibility`).

The recommended approach is to upgrade 1 major version at a time, with deployment to production. That guarantees that the necessary transformations of data are performed and no information will be lost.

The wire compatibility is guaranteed across 1 major version. Additionally, there's the best effort and extensive testing to cover compatibility across all versions, starting with NServiceBus Version 3.
