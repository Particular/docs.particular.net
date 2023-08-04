---
title: Using NServiceBus in AWS Lambda with SQS
reviewed: 2020-08-24
component: SQSLambda
related:
 - samples/sqs
redirects:
- samples/previews/aws-lambda/sqs
---

This sample shows how to host NServiceBus within an AWS Lambda, in this case, a function triggered by incoming SQS messages. This enables hosting message handlers in AWS Lambda, gaining the abstraction of message handlers implemented using `IHandleMessages<T>` and also taking advantage of NServiceBus's extensible message-processing pipeline. This sample also shows a function triggered by an HTTP call and how to use NServiceBus to dispatch messages from within this context.

When hosting NServiceBus within AWS Lambda, the function handler class hosts an NServiceBus endpoint that is capable of processing multiple message types.

downloadbutton

## Prerequisites

The sample includes a [`CloudFormation`](https://aws.amazon.com/cloudformation/aws-cloudformation-templates/) template, which will deploy the Lambda and create the necessary queues to run the sample.

The [`Amazon.Lambda.Tools` CLI](https://github.com/aws/aws-lambda-dotnet) can be used to deploy the template to an AWS account.

1. Install the [`Amazon.Lambda.Tools CLI`](https://github.com/aws/aws-lambda-dotnet#amazonlambdatools)
1. Make sure an S3 bucket is available in the AWS region of choice
2. Update the values of `stack-name` and `s3-bucket` settings in aws-lambda-tools-defaults.json file found in the **ServerlessEndpoint** project

## Running the sample

INFO: It is not possible at this stage to use the AWS .NET Mock Lambda Test Tool to run the sample locally.

Run the following command from the `ServerlessEndpoint` directory to deploy the Lambda project:

`dotnet lambda deploy-serverless`

The deployment will ask for a stack name and an S3 bucket name to deploy the serverless stack.

After that, running the sample will launch a single console window:

* **RegularEndpoint** is a console application that will send a `TriggerMessage` to the `ServerlessEndpoint` queue, which is monitored by the AWS Lambda.
* The deployed **ServerlessEndpoint** project will receive messages from the `ServerlessEndpoint` queue and process them using the AWS Lambda runtime.

To try the AWS Lambda

1. From the **RegularEndpoint** window, press <kbd>Enter</kbd> to send a `TriggerMessage` to the ServerLessEndpoint queue.
1. The AWS Lambda will receive the `TriggerMessage` and hand off its procesing to NServiceBus.
1. The NServiceBus message handler for `TriggerMessage` on **ServerlessEndpoint** sends a `ResponseMessage` that will be handled by the **RegularEndpoint**

## Code walk-through

The static NServiceBus endpoint must be configured using details that come from the AWS Lambda `ILambdaContext`. Since that is not available until a message is handled by the function, the NServiceBus endpoint instance is deferred until the first message is processed, using a lambda expression such as:

snippet: EndpointSetup

The same class defines the AWS Lambda, which makes up the hosting for the NServiceBus endpoint. The `SqsHandler` method hands off processing of messages to NServiceBus:

snippet: SqsFunctionHandler

Meanwhile, the message handler for `TriggerMessage`, also hosted within the AWS Lambda project, is a regular NServiceBus message handler that is capable of sending messages.

snippet: TriggerMessageHandler

## Dispatching a message outside of a message handler

There could be the need to dispatch a message after reacting to events other than messages being pushed to a queue. For example, responding to an S3 bucket file upload or to an HTTP request. This sample also demonstrates this use case.

1. Open a browser and visit the URL produced during the execution of  `dotnet lambda deploy-serverless`. Running the command produced a list of outputs, use the value produced for `ApiURL` output.
1. The AWS Lambda will receive the http call and send a `TriggerMessage` to the ServerLessEndpoint queue.
1. As in the previous example, the AWS Lambda will receive the `TriggerMessage` and hand off its procesing to NServiceBus.
1. The NServiceBus message handler for `TriggerMessage` on **ServerlessEndpoint** sends a `ResponseMessage` that will be handled by the **RegularEndpoint**

snippet: HttpFunctionHandler

## Removing the sample stack

To remove the deployed stack enter:

`dotnet lambda delete-serverless`

and provide the previously chosen stack name.