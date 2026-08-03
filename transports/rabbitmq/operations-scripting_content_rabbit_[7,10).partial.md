To provision or deprovision the resources required by an endpoint, use the `rabbitmq-transport` command-line (CLI) tool. The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Transport.RabbitMQ.CommandLine
```

Once installed, the `rabbitmq-transport` command line tool will be available for use.

`rabbitmq-transport <command> [options]`

### Available commands

- [`delays create`](#delays-create)
- [`delays migrate`](#delays-migrate)
- [`delays verify`](#delays-verify)
- [`endpoint create`](#endpoint-create)
- [`queue migrate-to-quorum`](#queue-migrate-to-quorum)

### `delays create`

Use this command to create v2 delay infrastructure queues and exchanges:

```
rabbitmq-transport delays create [options]
```

#### Options

- `--connectionString` | `-c`: Force this command to use the specified connection string
- `--connectionStringEnv`: Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option
- `--certPath`: The path to the client certificate file for connecting to the broker
- `--certPassphrase`: The passphrase for the client certificate file specified by the `certPath` option
- `--disableCertValidation`: Disable remote certificate validation when connecting to the broker
- `--useExternalAuth`: Use the external authorization option when connecting to the broker

### `delays migrate`

Use this command to migrate in-flight delayed messages from the v1 delay infrastructure to the v2 delay infrastructure:

```
rabbitmq-transport delays migrate [options]
```

#### Options

- `--connectionString` | `-c`: Force this command to use the specified connection string
- `--connectionStringEnv`: Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option
- `--certPath`: The path to the client certificate file for connecting to the broker
- `--certPassphrase`: The passphrase for the client certificate file specified by the `certPath` option
- `--disableCertValidation`: Disable remote certificate validation when connecting to the broker
- `--useExternalAuth`: Use the external authorization option when connecting to the broker
- `--routingTopology` | `-r`: The routing topology to use

### `delays verify`

Use this command to verify broker requirements for using the v2 delay infrastructure:

```
rabbitmq-transport delays verify [options]
```

> [!NOTE]
> This command requires the [RabbitMQ management plugin](https://www.rabbitmq.com/management.html) to be installed on the broker.

#### Options

- `--url`: The URL of the RabbitMQ management API
- `--username`: The username for accessing the RabbitMQ management API
- `--password`: The password for accessing the RabbitMQ management API

### `endpoint create`

Use this command to create queues and exchanges for an endpoint:

```
rabbitmq-transport endpoint create <endpointName> [options]
```

#### Arguments

- `endpointName`: The name of the endpoint to create

#### Options

- `--connectionString` | `-c`: Force this command to use the specified connection string
- `--connectionStringEnv`: Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option
- `--certPath`: Set the path to the client certificate file for connecting to the broker
- `--certPassphrase`: The passphrase for the client certificate file when using a client certificate
- `--disableCertValidation`: Disable remote certificate validation when connecting to the broker
- `--useExternalAuth`: Use the external authorization option when connecting to the broker
- `--routingTopology` | `-r`: Specifies which routing topology to use
- `--useDurableEntities` | `-d`: Specifies if entities should be created as durable
- `--queueType` | `-t`: Specifies the queue type to be used for queue creation
- `--errorQueueName`: Also create an error queue with the specified name
- `--auditQueueName`: Also create an audit queue with the specified name
- `--instanceDiscriminators`: An optional list of instance discriminators to use when the endpoint needs uniquely addressable instances

### `queue migrate-to-quorum`

Use this command to migrate an existing classic queue to a quorum queue.

```
rabbitmq-transport queue migrate-to-quorum <queueName> [options]
```

> [!NOTE]
> The migration command does not work with queues created by endpoints using the direct routing topology.

#### Arguments

- `queueName`: The name of the classic queue to migrate to a quorum queue

#### Options

- `--connectionString` | `-c`: Force this command to use the specified connection string
- `--connectionStringEnv`: Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option
- `--certPath`: Set the path to the client certificate file for connecting to the broker
- `--certPassphrase`: The passphrase for the client certificate file when using a client certificate
- `--disableCertValidation`: The passphrase for client certificate validation when using a client certificate
- `--useExternalAuth`: Use the external authorization option when connecting to the broker
