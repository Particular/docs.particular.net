---
title: Configuring AWS For NServiceBus
summary: Endpoint naming issues that occur when restarting the server from the AWS console can be prevented using a special tool.
reviewed: 2016-03-17
related:
 - nservicebus/operations
redirects:
 - nservicebus/configuring-aws-for-nservicebus
---

Stale endpoint naming issues may occur when shutting down and restart the server instance from the AWS console. It happens as AWS is changing the host name.

It is possible to turn off this server name change feature, using a tool installed on the server instance called EC2ConfigService Settings:

![EC2 Config Settings](ec2-config-settings.png)

In this tool, uncheck "Set Computer Name" in the General tab and click OK:

![](ec2-service-properties.png)
