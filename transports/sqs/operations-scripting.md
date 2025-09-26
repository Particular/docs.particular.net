---
title: SQS Transport Scripting
summary: Example code and scripts to facilitate deployment and operational actions against the SQS Transport.
component: SQS
reviewed: 2024-03-27
related:
 - nservicebus/operations
---

The following are example code and scripts in C# and PowerShell to facilitate deployment and operations against the SQS Transport.

## Requirements

If using PowerShell, the [AWS Tools for PowerShell](https://docs.aws.amazon.com/powershell/latest/userguide/pstools-getting-set-up.html) must be installed and properly configured.

For all operations that create resources in AWS the corresponding rights must be granted. For more information refer to the [IAM policies guide](https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html).

## QueueNameHelper

#### In C&#35;

snippet: sqs-queue-name-helper

The above `QueueNameHelper` makes sure that queues follow the proper naming guidelines for SQS.

#### In PowerShell

snippet: sqs-powershell-queue-name-helper

## Native Send

### The native send helper methods

A send involves the following actions:

 * Create and serialize the payload including headers.
 * Write a message body directly to SQS Transport.


#### In C&#35;

snippet: sqs-nativesend


#### In PowerShell

snippet: sqs-powershell-nativesend


### Using the native send helper methods

snippet: sqs-nativesend-usage

The headers must contain the message type that is sent as a fully qualified assembly name as well as the message id header. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` and `MessageId` headers.

## Native Send Large Messages

### The native send helper methods

A send involves the following actions:

 * Create an unique S3 key containing the `S3Prefix` and the `MessageId`
 * Upload the body of the message to the S3 bucket
 * Create and serialize the message with an empty payload including headers and the `S3BodyKey`.
 * Write a message directly to SQS Transport.

#### In C&#35;

snippet: sqs-nativesend-large

#### In PowerShell

snippet: sqs-powershell-nativesend-large

### Using the native send helper methods

snippet: sqs-nativesend-large-usage

The headers must contain the message type that is sent as a fully qualified assembly name as well as the message id header. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` and `MessageId` headers.

The S3 bucket name and the S3 prefix must be provided as defined in the transport configuration of the endpoint that will be receiving the message.

## Create resources

In order to provision the resources required by an endpoint, use the `sqs-transport` command line (CLI) tool.

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.AmazonSQS.CommandLine
```

Once installed, the `sqs-transport` command line tool will be available for use.

`sqs-transport <command> [arguments] [options]`

### Available commands

- `endpoint create`
- `endpoint add delay-delivery-support`
- `endpoint add large-message-support`
- `endpoint subscribe`
- `endpoint set-policy events`
- `endpoint set-policy wildcard`
- `endpoint list-policy`

### sqs-transport endpoint create

Create a new endpoint using:

```
sqs-transport endpoint create [name]
                              [--access-key-id]
                              [--secret]
                              [--region]
                              [--retention]
```

#### arguments

`name` : Name of the endpoint (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-p` | `--prefix`: Prefix to prepend to the endpoint queue

`-t` | `--retention`: Retention Period in seconds (defaults to 345600)

### sqs-transport endpoint add delay-delivery-support

Add delay delivery support to an endpoint using:

```
sqs-transport endpoint add [name] delay-delivery-support
                                        [--access-key-id]
                                        [--secret]
                                        [--region]
                                        [--prefix]
                                        [--retention]
```

#### arguments

`name` : Name of the endpoint (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-p` | `--prefix`: Prefix to prepend to the delay delivery queue

`-t` | `--retention`: Retention period in seconds (defaults to 345600)

### sqs-transport endpoint add large-message-support

Add large message support to an endpoint using:

```
sqs-transport endpoint add [name] large-message-support [bucket-name]
                                                        [--access-key-id]
                                                        [--secret]
                                                        [--region]
                                                        [--retention]
```

#### arguments

`name` : Name of the endpoint (required)

`bucket-name` : Name of the s3 bucket (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-p` | `--key-prefix`: S3 Key prefix to prepend to all bucket keys

`-e` | `--expiration`: Expiration time in days (defaults to 4)

### sqs-transport endpoint subscribe event-type

Subscribe an endpoint to an event type using:

```
sqs-transport endpoint subscribe [name] [event-type]
                                        [--access-key-id]
                                        [--secret]
                                        [--region]
                                        [--prefix]
```

> [!NOTE]
> This command will only set up the subscription from the topic representing the event-type to the input queue of the endpoint. It will **not** set up the IAM policy which allows messages to flow from the topic to the input queue. To set up the IAM policy refer to the `sqs-transport endpoint set-policy events` or `sqs-transport endpoint set-policy wildcard` command.

#### arguments

`name` : Name of the endpoint (required)

`event-type` : Full name of the event to subscribe to (e.g. MyNamespace.MyMessage) (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-p` | `--prefix`: Prefix to prepend to the topic provisioned for the event type and the subscribing queue

### sqs-transport endpoint set-policy events

Set the IAM policy on the input queue of an endpoint based on the event types the endpoint subscribed to using:

```
sqs-transport endpoint set-policy [name] events
                                         [--event-type]
                                         [--access-key-id]
                                         [--secret]
                                         [--region]
                                         [--prefix]
```

#### arguments

`name` : Name of the endpoint (required)

#### options

`-evt` | `--event-type` : Full name of the event allowed through the IAM policy (e.g. MyNamespace.MyMessage); can be repeated multiple times to allow multiple event types to pass.

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-r` | `--region`: Overrides the environment variable 'AWS_REGION'

`-p` | `--prefix`: Prefix to prepend to the topic provisioned for the event type and the subscribing queue

### sqs-transport endpoint set-policy wildcard

Set the IAM policy on the input queue of an endpoint based on wildcard conditions using:

```
sqs-transport endpoint set-policy [name] wildcard
                                         [--account-condition]
                                         [--namespace-condition]
                                         [--prefix-condition]
                                         [--remove-event-type]
                                         [--access-key-id]
                                         [--secret]
                                         [--region]
                                         [--prefix]
```

#### arguments

`name` : Name of the endpoint (required)

#### options

`-ac` | `--account-condition` : Allow all messages from any topic in the account to pass. If no value is provided, the account name will be derived from the endpoint input queue.

`-pc` | `--prefix-condition` : Allow all messages from any topic with prefix to pass. If no value is provided, the prefix from the `-p | --prefix` option will be used.

`-nc` | `--namespace-condition` : Allow all messages from any message in the specified namespaces to pass

`-revt` | `--remove-event-type` : Since existing event type conditions on the policy will not be removed by default, specify a value for this option to remove an existing event type condition in case they are covered by the wildcard policy implicitly. Can be repeated multiple times to remove multiple event types.

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-r` | `--region`: Overrides the environment variable 'AWS_REGION'

`-p` | `--prefix`: Prefix to prepend to the topic provisioned for the event type and the subscribing queue

### sqs-transport endpoint list-policy

List the existing IAM policy on the input queue of an endpoint using:

```
sqs-transport endpoint subscribe [name] list-policy
                                        [--access-key-id]
                                        [--secret]
                                        [--region]
                                        [--prefix]
```

#### arguments

`name` : Name of the endpoint (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-r` | `--region`: Overrides the environment variable 'AWS_REGION'

`-p` | `--prefix`: Prefix to prepend to the topic provisioned for the event type and the subscribing queue


## Delete resources

In order to de-provision the resources required by an endpoint, the `sqs-transport` command line (CLI) tool can be used.

### Available commands

- `endpoint unsubscribe`
- `endpoint remove delay-delivery-support`
- `endpoint remove large-message-support`
- `endpoint delete`

### sqs-transport endpoint unsubscribe

Unsubscribe an endpoint from an event type using:

```
sqs-transport endpoint unsubscribe [name] [event-type]
                              [--access-key-id]
                              [--secret]
                              [--region]
                              [--prefix]
                              [--remove-shared-resources]
```

#### arguments

`name` : Name of the endpoint (required)

`event-type` : Full name of the event to unsubscribe from (e.g. MyNamespace.MyMessage) (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-p` | `--prefix`: Prefix prepended to the topic provisioned for the event type and the subscribing queue

`-f` | `--remove-shared-resources`: Remove shared resources (the topic provisioned for the event type)

### sqs-transport endpoint remove delay-delivery-support

Remove delay delivery support from an endpoint using:

```
sqs-transport endpoint remove [name] delay-delivery-support
                              [--access-key-id]
                              [--secret]
                              [--region]
                              [--prefix]
```

#### arguments

`name` : Name of the endpoint (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-p` | `--prefix`: Prefix to prepend to the delay delivery queue

### sqs-transport endpoint remove large-message-support

Remove large message support from an endpoint using:

```
sqs-transport endpoint remove [name] large-message-support [bucket-name]
                              [--access-key-id]
                              [--secret]
                              [--region]
                              [--remove-shared-resources]
```

#### arguments

`name` : Name of the endpoint (required)

`bucket-name` : Name of the s3 bucket (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-e` | `--remove-shared-resources`: Remove shared resources (S3 Bucket)


### sqs-transport endpoint delete

Delete an existing endpoint using:

```
sqs-transport endpoint delete name
                              [--access-key-id]
                              [--secret]
                              [--region]
```

#### arguments

`name` : Name of the endpoint (required)

#### options

`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-p` | `--prefix`: Prefix to prepend to the endpoint queue

## Return message to source queue

### The retry helper methods

A retry involves the following actions:

 * Read a message from the error queue.
 * Forward that message to another queue to be retried.

snippet: sqs-return-to-source-queue

> [!WARNING]
> This example code will receive other messages from the error queue while it finds the desired message. All messages received by this code will be marked as invisible until the [visibility timeout](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-visibility-timeout.html) expires.

### Using the retry helper methods

snippet: sqs-return-to-source-queue-usage
