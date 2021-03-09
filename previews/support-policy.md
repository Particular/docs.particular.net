---
title: Support policy for previews
summary: What is supported for components in the Particular Preview Program
reviewed: 2020-07-09
related:
 - nservicebus/upgrades/support-policy
---

## Supported versions

Product previews are released with a major version of 0. When a product is released with a major version of 1 or greater, it is no longer part of the preview program and is subject to the [regular support policy](/nservicebus/upgrades/support-policy.md).

For example:

| Version | Covered by this policy | Covered by regular support policy
| --- | --- | --- |
| 0.1.0 | Yes | No |
| 0.2.0 | Yes | No |
| 0.2.1 | Yes | No |
| 1.0.0 | No | Yes |

Note: Only the highest minor version is supported i.e. when `0.2.0` is released, `0.1.0` is no longer supported.

## Bugfixes

The support policy for preview pacakges guarantees that fixes for critical bugs are released immediately after being fixed. These fixes are available only for the supported version and as such are released as a patch to the latest minor version.

## Compatibility guarantees

Previews may break backwards compatibility at any time. This includes, but is not limited to:

- API changes
- Features
- Wire compatibility (if applicable)
- Data storage compatibility (if applicable)

## Time limits

Product previews are public for a limited time. When this time has elapsed, they are either:

- Promoted to being part of the Particular Service Platform with a 1.0 release, and supported according to standard support policies.
- Offered via an open-source license to adopters with no further guaranteed maintenance or support by Particular Software.
