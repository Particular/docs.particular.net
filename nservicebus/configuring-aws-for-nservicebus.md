---
title: Configuring AWS For NServiceBus
summary: Endpoint naming issues that occur when you restart the server from the AWS console can be prevented using a special tool.
tags: []
---

Stale endpoint naming issues may occur when you shut down and restart the server instance from the AWS console. It happens as AWS is changing the host name.

It is possible to turn off this server name change feature, using a tool installed on your server instance called EC2ConfigService Settings:

![EC2 Config Settings](ec2-config-settings.png)

In this tool, uncheck "Set Computer Name" in the General tab and click OK:

![](ec2-service-properties.png)

Read more information on [troubleshooting this issue](http://christer.dk/post/NServiceBus-on-Amazon-EC2-voodoo.aspx).

