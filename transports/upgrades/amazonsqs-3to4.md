---
title: Upgrade AmazonSQS Transport Version 3 to 4
summary: Instructions on how to upgrade AmazonSQS Transport Version 3 to 4
component: SQS
isUpgradeGuide: true
reviewed: 2017-10-12
upgradeGuideCoreVersions:
 - 7
---

## MaxTTLDays renamed MaxTimeToLive

The `MaxTTLDays` method has been renamed to `MaxTimeToLive` and now takes a [TimeSpan](https://msdn.microsoft.com/en-us/library/system.timespan.aspx)

snippet: 3to4_MaxTTL