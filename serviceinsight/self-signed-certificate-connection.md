---
title: SSL Validation
reviewed: 2016-09-28
summary: ServiceInsight refuses to connect to a ServiceControl instance running on SSL with self-signed certificate.
component: ServiceInsight
tags:
- SSL
- ServiceControl
- Certificate
---

ServiceControl can be configured to run on a SSL, but if a self-signed certificate is chosen, ServiceInsight would not allow connections to that instance. While running a self-sign certificate is not recommended, ServiceInsight provides an override configuration entry to allow you to connect to such instances. 

![ServiceInsight refuses to connect](./images/ssl-validation.png)

Open `ServiceInsight.exe.config` from the installation directory and change the value for the following entry to `True` (the dafault value is False):

```XML
<appSettings>
    <add key="SkipCertificateValidation" value="False" />
</appSettings>
``` 

NOTE: Since the application by default installs in the `Program Files` folder, admninistrative priviledge might be required to edit the config file above. 