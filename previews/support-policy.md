---
title: Support policy for previews
summary: Describes the details for the support policy for previews
reviewed: 2020-04-07
related:
 - nservicebus/upgrades/support-policy
---

## Supported versions

Product previews are released with a major version of 0. When a product is released with a major version of 1 or greater, it is no longer part of the preview program and does not follow this support policy.

For example:

| Version | Covered by this policy |
| --- | --- |
| 0.1.0 | Yes |
| 0.2.0 | Yes |
| 0.2.1 | Yes |
| 1.0.0 | No |

WARN: Only the latest minor version of a preview is supported.

## Compatibility guarantees

Previews may break backwards compatibility at any time. This includes, but is not limited to:

- API changes
- Features
- Wire compatibility (if applicable)
- Data storage compatibility (if applicable)

## Time limits

Product previews are public for a limited time. When this time has elapsed, they are either:

- Promoted to being part of the Particular Service Platform with a 1.0 release, and supported according to standard support policies.
- Retired and no further maintenance or support is provided.
