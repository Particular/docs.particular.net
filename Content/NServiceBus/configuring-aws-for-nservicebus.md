---
title: Configuring AWS For NServiceBus
summary: Endpoint naming issues that occur when you restart the server from the AWS console can be prevented using a special tool.
originalUrl: http://www.particular.net/articles/configuring-aws-for-nservicebus
tags: []
createdDate: 2013-05-22T08:40:08Z
modifiedDate: 2013-11-25T06:08:10Z
authors: []
reviewers: []
contributors: []
---

Stale endpoint naming issues may occur when you shut down and restart the server instance from the AWS console. It happens as AWS is changing the host name.

It is possible to turn off this server name change feature, using a tool installed on your server instance called EC2ConfigService Settings:

![EC2 Config Settings](EC2ConfigSettings.png)

In this tool, uncheck "Set Computer Name" in the General tab and click OK:

![](EC2ServiceProperties.png)

Read more information on [troubleshooting this issue](http://christer.dk/post/NServiceBus-on-Amazon-EC2-voodoo.aspx).

