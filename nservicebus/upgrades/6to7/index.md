---
title: Upgrade Version 6 to 7
summary: Instructions on how to upgrade NServiceBus Version 6 to 7.
component: Core
reviewed: 2017-05-23
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


include: upgrade-major

## Config


### Audit


#### ProvideConfiguration

snippet: 6to7AuditProvideConfiguration


#### Using XML

snippet: 6to7configureAuditUsingXml


snippet: 6to7configureAuditUsingXmlAppSettings


snippet: 6to7configureAuditUsingXmlReadAppSettings


### Logging


#### ProvideConfiguration

snippet: 6to7LoggingThresholdFromIProvideConfiguration


#### Using XML

snippet: 6to7OverrideLoggingDefaultsInAppConfig



snippet: 6to7OverrideLoggingDefaultsInAppSettings

snippet: 6to7OverrideLoggingDefaultsReadAppSettings


### Error Queue


#### ProvideConfiguration

snippet: 6to7ErrorQueueConfigurationProvider



#### Using XML

snippet: 6to7configureErrorQueueViaXml

snippet: 6to7configureErrorQueueViaAppSettings
snippet: 6to7configureErrorQueueReadAppSettings


#### IConfigurationSource

snippet: 6to7ErrorQueueConfigurationSource

snippet: 6to7UseCustomConfigurationSourceForErrorQueueConfig

snippet: 6to7UseCustomConfigurationSourceForErrorQueueConfigNew




include: dependencies
