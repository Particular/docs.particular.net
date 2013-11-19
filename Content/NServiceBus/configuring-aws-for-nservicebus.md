---
title: "Configuring AWS For NServiceBus"
tags: ""
summary: "Stale endpoint naming issues may occur when you shut down and restart the server instance down from the AWS console. It happens as AWS is changing the host name."
---

Stale endpoint naming issues may occur when you shut down and restart the server instance down from the AWS console. It happens as AWS is changing the host name.

It is possible to turn off this server name change feature, using a tool installed on your server instance called EC2ConfigService Settings:

![EC2 Config Settings](EC2ConfigSettings.png)

In this tool, uncheck "Set Computer Name" in the General tab and click OK:

![](EC2ServiceProperties.png)

Read more information on [troubleshooting this issue](http://christer.dk/post/NServiceBus-on-Amazon-EC2-voodoo.aspx).

