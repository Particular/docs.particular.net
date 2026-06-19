---
title: AWS Loan Broker Showcase
summary: A sample for a fictional cinema demonstrating a saga that aggregates ticket sales.
reviewed: 2026-06-19
component: Core
---

## Scenario

The AWS Loan Broker Showcase is a basic loan broker implementation following the [structure presented](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ComposedMessagingExample.html) by [Gregor Hohpe](https://www.enterpriseintegrationpatterns.com/gregor.html) in his book - [Enterprise Integration Patterns](https://www.enterpriseintegrationpatterns.com/).

![Logical architecture of the loan broker](ConsumerLoanBroker.gif)

The AWS Loan Broker Showcase simplifies serverless messaging on AWS. It demonstrates how to build distributed systems with NServiceBus and the Particular Service Platform, using AWS services - eliminating the complexity of managing dozens of Lambda functions and Step Functions with a streamlined, enterprise-grade messaging pattern.

The showcase, by default, runs locally using LocalStack, and no AWS account is needed. The how to run the example section details how to configure the solution to connect to AWS services.

![Architecture of the AWS loan broker sample](architecture-view.png)

The example is composed by:

- A client application, sending loan requests.
- A credit bureau providing the customers' credit score.
- A loan broker service that receives loan requests enriches them with credit scores and orchestrates communication with downstream banks.
- Three bank adapters, acting like Anti-Corruption layers (ACL), simulate communication with downstream banks offering loans.
- An email sender simulating email communication with customers.

The example also ships the following monitoring services:

- The Particular platform to monitor endpoints, capture and visualize audit messages, and manage failed messages.
- A Prometheus instance to collect, store, and query raw metrics data.
- A Grafana instance with three different metrics dashboards using Prometheus as the data source.
- A Jaeger instance to visualize OpenTelemetry traces.
- AWS Distro for OpenTelemetry collector (ADOT) to collect and export metrics and traces to various destinations.

The example also exports metrics and traces to AWS CloudWatch and XRay.

## Prerequsities

1. .NET 10 SDK
2. Docker
3. Docker Compose

## Running the sample

The simplest way to run the example is using Docker for both the endpoints and the infrastructure.
The client application, the loan broker service, the e-mail sender, and the bank adapters can be deployed as Docker containers alongside the Particular platform to monitor the system, LocalStack to mock the AWS services, and the additional containers needed for enabling OpenTelemetry observability.

To run the complete example in Docker, publish the endpoint container images and start the Compose environment from the root folder:

```
dotnet publish src/AwsLoanBrokerSample.sln -c Release --os linux /t:PublishContainer
docker compose up -d
```

> [!TIP]
> Once the project is running, check out the [Things to try](#things-to-try) section.

The `dotnet publish` commands build the projects and publish Linux container images to the local Docker registry using the .NET SDK's built-in container support. The Docker Compose command starts those endpoint images and configures all the additional infrastructural containers.

To stop the running solution and remove all deployed containers. Using a command prompt, execute the following command:

```
docker compose down
```

To run the solution without rebuilding container images, execute the following command:

```
docker compose up -d
```

> [!Note]
> To run transport and persistence using AWS services instead of LocalStack:
> - remove the `AWS_ENDPOINT_URL` variable from the [aws.env](env/aws.env) file
> - ensure the following environment variables are defined with appropriate values:
>   - `AWS_ACCESS_KEY_ID`
>   - `AWS_SECRET_ACCESS_KEY`
>   - `AWS_REGION`

### Running endpoints from the IDE

If you prefer to start the endpoints from your IDE to debug the code, execute the following command from a command prompt in the root directory to start the required infrastructure:

```
docker compose --profile infrastructure up -d
```
