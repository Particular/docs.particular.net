---
title: Using NServiceBus in AWS Lambda with annotations and SQS
reviewed: 2026-01-05
component: SQSLambda
related:
 - samples/aws/sqs-simple
 - samples/aws/sqs-native-integration
 - samples/aws/lambda-sqs
---

This sample shows how to host NServiceBus within an AWS Lambda when using the [annotations programming model](https://github.com/aws/aws-lambda-dotnet/tree/master/Libraries/src/Amazon.Lambda.Annotations), in this case, a function triggered by incoming SQS messages. This enables hosting message handlers in AWS Lambda, gaining the abstraction of message handlers implemented using `IHandleMessages<T>` and also taking advantage of NServiceBus's extensible message-processing pipeline. This sample also shows a function triggered by an HTTP call and how to use NServiceBus to dispatch messages from within this context.

When hosting NServiceBus within AWS Lambda, the function handler class hosts an NServiceBus endpoint that is capable of processing multiple message types.

downloadbutton

## Prerequisites

The sample includes a [`CloudFormation` template](https://aws.amazon.com/cloudformation/aws-cloudformation-templates/), which will deploy the Lambda and create the necessary queues.

The [`Amazon.Lambda.Tools` CLI](https://github.com/aws/aws-lambda-dotnet) can be used to deploy the template to an AWS account.

1. Install the [`Amazon.Lambda.Tools CLI`](https://github.com/aws/aws-lambda-dotnet#amazonlambdatools)
2. Make sure an S3 bucket is available in the AWS region of choice
3. Update the `s3-bucket` settings in aws-lambda-tools-defaults.json file found in the **ServerlessEndpoint** project with the name of the bucket
4. Optionally change the `stack-name` setting

> [!NOTE]
> The AWS Lambda annotation model requires specifying resources ARN. Before deploying and running the Lambda, open the `SqsLambda.cs` and update the `FunctionHandler` method `SQSEvent` attribute, replacing the `region` and `account-id` values with valid ones.

## Running the sample

> [!NOTE]
> It is not possible at this stage to use the AWS .NET Mock Lambda Test Tool to run the sample locally.

Run the following command from the `ServerlessEndpoint` directory to deploy the Lambda project:

`dotnet lambda deploy-serverless`

The deployment will ask for a stack name and an S3 bucket name to deploy the serverless stack.

After that, running the sample will launch a single console window:

* **RegularEndpoint** is a console application that will send a `TriggerMessage` to the `ServerlessEndpoint` queue, which is monitored by the AWS Lambda.
* The deployed **ServerlessEndpoint** project will receive messages from the `ServerlessEndpoint` queue and process them using the AWS Lambda runtime.

To try the AWS Lambda

1. From the **RegularEndpoint** window, press <kbd>Enter</kbd> to send a `TriggerMessage` to the ServerLessEndpoint queue.
1. The AWS Lambda will receive the `TriggerMessage` and hand off its processing to NServiceBus.
1. The NServiceBus message handler for `TriggerMessage` on **ServerlessEndpoint** sends a `ResponseMessage` that will be handled by the **RegularEndpoint**

## Code walk-through

The NServiceBus endpoint is configured at Lambda startup time, and registered in the service collection as follows:

snippet: EndpointSetup

The `FunctionHandler` method hands-off processing of messages to NServiceBus:

snippet: SqsFunctionHandler

Meanwhile, the message handler for `TriggerMessage`, also hosted within the AWS Lambda project, is a regular NServiceBus message handler that is capable of sending messages.

snippet: TriggerMessageHandler

## Dispatching a message outside of a message handler

There could be the need to dispatch a message after reacting to events other than messages being pushed to a queue. For example, responding to an S3 bucket file upload or to an HTTP request. This sample also demonstrates this use case.

1. Open a browser and visit the URL produced during the execution of `dotnet lambda deploy-serverless`. The command produces a list of outputs; note the value for `ApiURL`.
1. The AWS Lambda will receive the HTTP call and send a `TriggerMessage` to the ServerlessEndpoint queue.
1. As in the previous example, the AWS Lambda will receive the `TriggerMessage` and hand off its processing to NServiceBus.
1. The NServiceBus message handler for `TriggerMessage` on **ServerlessEndpoint** sends a `ResponseMessage` that will be handled by the **RegularEndpoint**.

snippet: HttpFunctionHandler

## Removing the sample stack

To remove the deployed stack enter:

`dotnet lambda delete-serverless`

and provide the previously chosen stack name.
