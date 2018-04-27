---
title: Configuring Amazon EC2 instances For NServiceBus
summary: Endpoint naming issues that occur when restarting the server from the AWS Mangement Console can be prevented using a special tool.
reviewed: 2016-10-09
related:
 - nservicebus/operations
redirects:
 - nservicebus/configuring-aws-for-nservicebus
---

Stale endpoint naming issues may occur when shutting down and restarting the server instance from the [AWS Management Console](https://aws.amazon.com/console/) due to AWS changing the host name of the instance.

It is possible to turn off this server name change feature using a tool called EC2ConfigService Settings which is installed on the server instance:

![EC2 Config Settings](ec2-config-settings.png)

In this tool, uncheck "Set Computer Name" in the General tab and click OK:

![](ec2-service-properties.png)
