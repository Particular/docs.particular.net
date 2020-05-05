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