---
title: Configuring Amazon EC2 Instances for NServiceBus
summary: Endpoint naming issues that occur when restarting the server from the AWS Mangement Console can be prevented using a special tool
reviewed: 2018-05-17
related:
 - nservicebus/operations
redirects:
 - nservicebus/configuring-aws-for-nservicebus
---

Stale endpoint naming issues may occur when shutting down and restarting the server instance from the [AWS Management Console](https://aws.amazon.com/console/) due to AWS changing the host name of the instance.

It is possible to turn off this server name change feature using the [EC2Config tool](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/ec2config-service.html) which is installed on the server instance:

![EC2 Config Settings](ec2-config-settings.png)

In this tool, uncheck "Set Computer Name" in the General tab and click OK:

![](ec2-service-properties.png)

For Windows 2016 Server, EC2Config has been replaced by EC2Launch. Review the [EC2Launch documentation](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/ec2launch.html) for instructions on how to change the computer name for EC2 instances on Windows 2016 Server.