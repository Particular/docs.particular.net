---
title: AWS compute services
summary:
reviewed: 2024-03-14
callsToAction: ['solution-architect', 'poc-help']
---

AWS provides a range of hosting options, spanning the range from Serverless all the way through to EC2 instances.

The Particular Service Platform can be hosted using:

![alt_text](images/image3.png "image_tooltip")

#### Serverless

[AWS Lambda](https://aws.amazon.com/lambda/) is a serverless compute service offered by AWS that allows developers to run code in response to events without the need to manage servers. NServiceBus supports AWS Lambda so that new or existing applications can directly consume messages from [SQS queues](https://aws.amazon.com/sqs/).

[Host NServiceBus applications on AWS Lambda](https://docs.particular.net/nservicebus/hosting/aws-lambda-simple-queue-service/).

#### PaaS

[Platform as a Service (PaaS)](https://en.wikipedia.org/wiki/Platform_as_a_service) models provide managed hosting environments where applications can be deployed without having to manage the underlying infrastructure, operating system, or runtime environments.

[AWS Elastic Beanstalk](https://aws.amazon.com/elasticbeanstalk/) allows you to deploy your .NET web applications to either IIS or Nginx and have Beanstalk provision and manage your infrastructure. NServiceBus applications can be integrated into these .NET Core web applications.

##### Containers

[Containers](https://en.wikipedia.org/wiki/Containerization_(computing)) are a popular mechanism to deploy and host applications in PaaS services. NServiceBus can be used by containerized applications and deployed to services like:

- [Amazon Elastic Container Service](https://aws.amazon.com/ecs/)
- [Amazon Elastic Kubernetes Service](https://aws.amazon.com/eks/)

#### IaaS

Infrastructure as a Service (IaaS) provides virtualized computing resources like virtual machines, storage, and networking that can be used to build and manage the required infrastructure.

NServiceBus applications can easily be hosted on virtual machines. Popular techniques include:

- [Integrating NServiceBus with the Microsoft Generic Host](https://docs.particular.net/nservicebus/hosting/extensions-hosting)
- [Custom hosted web applications](https://docs.particular.net/nservicebus/hosting/web-application)
- [Installing NServiceBus endpoints as Windows Services](https://docs.particular.net/nservicebus/hosting/windows-service)
- [Manually controlling NServiceBus lifecycle in an executable (e.g. Console or GUI applications)](https://docs.particular.net/nservicebus/hosting/#self-hosting)
- [Custom-managed Kubernetes clusters hosting container applications](https://docs.particular.net/nservicebus/hosting/docker-host/)

#### Choosing a hosting model

The best choice of hosting model depends on the desired characteristics, such as:

- **Scalability**: Different hosting options offer different approaches to scaling. Managed solutions are typically easier to scale on demand and can scale on more granular levels. In addition to the scalability, elasticity (the time required to scale up or down) may also factor into the choice.
- **Pricing:** Managed services typically offer more dynamic pricing models that adjust with the demands of an application, in comparison with more rigid pricing models for infrastructure services. However, managed services typically charge more for their pricing units, so infrastructure services may be more economical for consistent demand. AWS offers a[ pricing calculator](https://calculator.aws/) to help understand a given service's pricing model.
- **Portability:** Serverless models are primarily built on proprietary programming models, heavily tied to the cloud service vendor. Hosting models built on open standards make it easier to run components in other hosting environments. Additionally, it may also be desirable to run components using on-premises servers or workstations.
- **Flexibility:** Lower-level infrastructure provides more control over the configuration and management of applications. Serverless offerings offer less flexibility due to the higher level of abstractions exposed to the code.
- **Manageability:** Serverless and PaaS models remove the concerns about the underlying infrastructure challenges (e.g. automatic scaling, OS updates, load balancing, etc.), typically at the cost of flexibility. Managing and maintaining infrastructure using other models may require significant resources and knowledge.