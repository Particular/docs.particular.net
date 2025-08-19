---
title: HTTPS Certificate Validation
reviewed: 2025-03-31
summary: ServiceInsight refuses to connect to a ServiceControl instance running over HTTPS with a self-signed certificate.
component: ServiceInsight
---

ServiceControl can be configured to run over a secured connection, but if a self-signed certificate is used for this purpose, ServiceInsight will not connect to that instance by default. While using a self-signed certificate is generally not recommended, ServiceInsight provides an override configuration entry to allow connecting to instances secured with one of these certificates.

![ServiceInsight refuses to connect](./images/ssl-validation.png)

Open `ServiceInsight.exe.config` from the installation directory and change the value for the following entry to `True` (the default value is False):

```XML
<appSettings>
    <add key="SkipCertificateValidation" value="False" />
</appSettings>
```

> [!NOTE]
> Since the application by default installs in the `Program Files` folder, admninistrative privilege might be required to edit the config file above.
