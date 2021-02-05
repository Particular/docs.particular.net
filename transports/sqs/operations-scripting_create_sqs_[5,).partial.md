## Create resources

In order to provision the resources required by an endpoint, use the `sqs-transport` command line (CLI) tool.

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.NServiceBus.AmazonSQS.CommandLine
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

NOTE: This command will only set up the subscription from the topic representing the event-type to the input queue of the endpoint. It will **not** set up the IAM policy which allows messages to flow from the topic to the input queue. To set up the IAM policy refer to the `sqs-transport endpoint set-policy events` or `sqs-transport endpoint set-policy wildcard` command.

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
