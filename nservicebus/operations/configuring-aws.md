---
title: Configuring Amazon EC2 Instances for NServiceBus
summary: Endpoint naming issues that occur when restarting the server from the AWS Management Console can be prevented using a tool
reviewed: 2025-02-19
related:
 - nservicebus/operations
redirects:
 - nservicebus/configuring-aws-for-nservicebus
---

Stale endpoint naming issues may occur when shutting down and restarting the server instance from the [AWS Management Console](https://aws.amazon.com/console/) due to AWS changing the hostname of the instance.

It is possible to turn off this server name change feature using the [EC2Launch v2 agent](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/ec2launch-v2.html) which is installed on the server instance:

![EC2 Config Settings](ec2-config-settings.png)

In this tool, uncheck "Set Computer Name" in the General tab and click Save:

![](ec2-service-properties.png)
