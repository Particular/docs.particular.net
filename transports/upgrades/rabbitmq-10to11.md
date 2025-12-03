---
title: RabbitMQ Transport Upgrade Version 10 to 11
summary: Instructions on how to upgrade RabbitMQ Transport from version 10 to 11.
reviewed: 2025-09-09
component: Rabbit
related:
- transports/rabbitmq
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

## SetClientCertificate supports fewer certificate formats

Due to Microsoft making the `X509Certificate2` constructor that accepts content as a `string` file path [obsolete](https://learn.microsoft.com/en-us/dotnet/fundamentals/syslib-diagnostics/syslib0057), the `SetClientCertificate` overload that takes a `path` and a `password` now only supports loading X.509 or PKCS12 certificates.

If a different certificate format is required, the `SetClientCertificate` overload that directly takes an `X509Certificate2` instance can be used instead.