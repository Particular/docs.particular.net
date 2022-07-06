In order to provision or de-provision the resources required by an endpoint, the `rabbitmq-transport` command line (CLI) tool can be used.

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Transport.RabbitMQ.CommandLine
```

Once installed, the `rabbitmq-transport` command line tool will be available for use.

`rabbitmq-transport <command> [options]`

### Available commands

- [`delays verify`](#delays-verify)
- [`delays create`](#delays-create)
- [`delays migrate`](#delays-migrate)
- [`endpoint create`](#endpoint-create)
- [`queue migrate-to-quorum`](#queue-migrate-to-quorum)

### `delays verify`

Verify broker pre-requisites for using the delay infrastructure v2.

NOTE: This command requires the [RabbitMQ management plugin](https://www.rabbitmq.com/management.html) to be installed on the broker.

```
rabbitmq-transport delays verify --url http://localhost:15672/ --username guest --password guest
```

#### Options

`--url` | `-c` : Specifies the url to the RabbitMQ management api<br />
`--username` : Specifies the username for accessing the RabbitMQ management api<br />
`--password`: Specifies the password for accessing the RabbitMQ management api<br />

### `delays create`

Create the infrastructure needed for delayed messages using:

```
rabbitmq-transport delays create --connectionString host=localhost
```

#### Options

`--connectionString` | `-c` : The connection string to use<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. --connectionString, if specified, will take precedence over this option<br />
`--certPath`: Set the path to the client certificate file for connecting to the broker<br />
`--certPassphrase`: The passphrase for client certificate file for when using a client certificate<br />
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate<br />
`--useExternalAuth`: Use the external authorization option when connecting to the broker<br />

### `delays migrate`

Migrates existing timeout infrastructure from v1 to v2:

```
rabbitmq-transport delays migrate --connectionString host=localhost
```

#### Options

`--connectionString` | `-c` : The connection string to use<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. --connectionString, if specified, will take precedence over this option<br />
`--certPath`: Set the path to the client certificate file for connecting to the broker<br />
`--certPassphrase`: The passphrase for client certificate file for when using a client certificate<br />
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate<br />
`--useExternalAuth`: Use the external authorization option when connecting to the broker<br />

### `endpoint create`

Create a new endpoint using:

```
rabbitmq-transport endpoint create MyEndPoint --connectionString host=localhost
```

#### Options

`--connectionString` | `-c` : The connection string to use<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. --connectionString, if specified, will take precedence over this option<br />
`--certPath`: Set the path to the client certificate file for connecting to the broker<br />
`--certPassphrase`: The passphrase for client certificate file for when using a client certificate<br />
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate<br />
`--routingTopology` | `-r`: Specifies which routing toplogy to use, valid options: Conventional|Direct<br />
`--useDurableEntities` | `-d`: Specifies if entities should be created as durable<br />
`--queueType` | `-c`: Specifies queue type will be used for queue creation, valid options: Quorum|Classic<br />
`--useExternalAuth`: Use the external authorization option when connecting to the broker<br />
`--errorQueueName`: Specifies that an error queue with the specified name should be created<br />
`--auditQueueName`: Specifies that an audit queue with the specified name should be created<br />
`--instanceDiscriminators`: Specifies a list of instance discriminators to use when the endpoint needs uniquely addressable instances<br />

### `queue migrate-to-quorum`

Migrate an existing classic queue to quorum queues.

```
rabbitmq-transport queue migrate-to-quorum MyQueue --connectionString host=localhost
```

NOTE: The migration command does not work with queues created by endpoints using the direct routing topology.

#### Options

`--connectionString` | `-c` : The connection string to use<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. --connectionString, if specified, will take precedence over this option<br />
`--certPath`: Set the path to the client certificate file for connecting to the broker<br />
`--certPassphrase`: The passphrase for client certificate file for when using a client certificate<br />
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate<br />
`--routingTopology` | `-r`: Specifies which routing toplogy to use, valid options: Conventional|Direct<br />
`--useDurableEntities` | `-d`: Specifies if entities should be created as durable<br />
`--queueType` | `-c`: Specifies queue type will be used for queue creation, valid options: Quorum|Classic<br />
`--useExternalAuth`: Use the external authorization option when connecting to the broker<br />