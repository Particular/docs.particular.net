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

#### arguments

`name` : Name of the endpoint (required)

`event-type` : Full name of the event to subscribe to (e.g. MyNamespace.MyMessage) (required)

#### options
 
`-i` | `--access-key-id` : Overrides the environment variable 'AWS_ACCESS_KEY_ID'

`-s` | `--secret` : Overrides the environment variable 'AWS_REGION'

`-r` | `--region`: Overrides the environment variable 'AWS_SECRET_ACCESS_KEY'

`-p` | `--prefix`: Prefix to prepend to the topic provisioned for the event type and the subscribing queue