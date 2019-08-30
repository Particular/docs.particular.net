---
title: Using NServiceBus in AWS Lambda with SQS
reviewed: 2019-08-27
component: SQSLambdas
related:
 - samples/sqs
---

include: aws-lambda-experimental

This sample shows how to host NServiceBus within an AWS Lambda, in this case, a function triggered by incoming SQS messages. This enables hosting message handlers in AWS Lambda, gaining the abstraction of message handlers implemented using `IHandleMessages<T>` and also taking advantage of NServiceBus's extensible message processing pipeline.

When hosting NServiceBus within AWS Lambda, the function handler (as identified by the `function-handler` property in the `aws-lambda-tools-defaults.json`) hosts an NServiceBus endpoint that is capable of processing multiple different message types.

downloadbutton

## Prerequisites

The sample includes a [`CloudFormation`](https://aws.amazon.com/cloudformation/aws-cloudformation-templates/) template which will deploy the Lambda and create the necessary queues to run the sample.

The [`Amazon.Lambda.Tools` cli](https://github.com/aws/aws-lambda-dotnet) can be used to deploy the template to your AWS account.

1. Install the [`Amazon.Lambda.Tools cli`](https://github.com/aws/aws-lambda-dotnet#amazonlambdatools)
1. Make sure an S3 bucket is available in your preferred AWS region

## Running the sample

INFO: It is not possible at this stage to use the AWS Mock Test tool to locally run the sample.

 Run the following command from the `AwsLambda.SQSTrigger` directory to deploy the Lambda project:

`dotnet lambda deploy-serverless`

The deployment will ask for a stack name and an S3 bucket name in order to deploy the serverless stack.

After which, running the sample should launch a single console window:

* **AWSLambda.Sender** is a console application that will send a `TriggerMessage` to the `AwsLambdaSQSTrigger` queue, which is monitored by the AWS Lambda.
* The deployed **AWSLambda.SQSTrigger** project will receive messages from the `AwsLambdaSQSTrigger` queue and process them using the AWS Lambda runtime.

To try the AWS Lambda:

1. From the **AwsLambda.Sender** window, press <kbd>Enter</kbd> to send a `TriggerMessage` to the trigger queue.
1. The AWS Lambda will receive the `TriggerMessage` and process it with NServiceBus.
1. The NServiceBus message handler for `TriggerMessage` sends a `FollowUpMessage`.
1. The AWS Lambda will receive the `FollowUpMessage` and process it with NServiceBus.
1. The NServiceBus message handler for `FollowUpMessage` sends a `BackToSenderMessage` that will be handled by the **AwsLambda.Sender**

## Code walk-through

The static NServiceBus endpoint must be configured using details that come from the AWS Lambda `ILambdaContext`. Since that is not available until a message is handled by the function, the NServiceBus endpoint instance is deferred until the first message is processed, using a lambda expression like this:

snippet: EndpointSetup

The same class defines the AWS Lambda which makes up the hosting for the NServiceBus endpoint. The `FunctionHandler` method hands off processing of the message to NServiceBus:

snippet: FunctionHandler

Meanwhile, the message handlers for `TriggerMessage` and `FollowUpMessage`, also hosted within the Azure Functions project, are normal NServiceBus message handlers, which are also capable of sending messages themselves.

snippet: TriggerMessageHandler

snippet: FollowupMessageHandler

## Removing the sample stack

In order to remove the deployed stack enter:

`dotnet lambda delete-serverless`

and provide the previously chosen stack name.