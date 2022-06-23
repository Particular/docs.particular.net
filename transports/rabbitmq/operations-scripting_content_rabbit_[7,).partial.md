In order to provision or de-provision the resources required by an endpoint, the `rabbitmq-transport` command line (CLI) tool can be used.

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Transport.RabbitMQ.CommandLine
```

Once installed, the `rabbitmq-transport` command line tool will be available for use.

`rabbitmq-transport <command> [options]`

### Available commands

- `delays verify`
- `delays create`
- `delays migrate`
- `endpoint create`
- `queue migrate-to-quorum`

### rabbitmq-transport delays verify

Verify broker pre-requisites for using the delay infrastructure v2.

NOTE: This command requires the RabbitMQ management api plugin to be installed on the broker.

```
rabbitmq-transport delays verify --url http://localhost:15672/ --username guest --password guest
```

#### options

`--url` | `-c` : Specifies the url to the RabbitMQ management api
`--username` : Specifies the username for accessing the RabbitMQ management api
`--password`: Specifies the password for acessing the RabbitMQ management api

### rabbitmq-transport delays create

Create the infrastructure needed for delayed messages using:

```
rabbitmq-transport delays create --connectionString host=localhost
```

#### options

`--connectionString` | `-c` : The connection string to use
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. --connectionString, if specified, will take precedence over this option
`--certPath`: Set the path to the client certificate file for connecting to the broker
`--certPassphrase`: The passphrase for client certificate file for when using a client certificate
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate
`--useExternalAuth`: Use the external authorization option when connecting to the broker

### rabbitmq-transport delays migrate

Migrates existing timeout infrastructure from v1 to v2:

```
rabbitmq-transport delays migrate --connectionString host=localhost
```

#### options

`--connectionString` | `-c` : The connection string to use
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. --connectionString, if specified, will take precedence over this option
`--certPath`: Set the path to the client certificate file for connecting to the broker
`--certPassphrase`: The passphrase for client certificate file for when using a client certificate
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate
`--useExternalAuth`: Use the external authorization option when connecting to the broker
### rabbitmq-transport endpoint create

Create a new endpoint using:

```
rabbitmq-transport endpoint create MyEndPoint --connectionString host=localhost
```

#### options

`--connectionString` | `-c` : The connection string to use
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. --connectionString, if specified, will take precedence over this option
`--certPath`: Set the path to the client certificate file for connecting to the broker
`--certPassphrase`: The passphrase for client certificate file for when using a client certificate
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate
`--routingTopology` | `-r`: Specifies which routing toplogy to use, valid options: Conventional|Direct
`--useDurableEntities` | `-d`: Specifies if entities should be created as durable
`--queueType` | `-c`: Specifies queue type will be used for queue creation, valid options: Quorum|Classic
`--useExternalAuth`: Use the external authorization option when connecting to the broker
`--errorQueueName`: Specifies that an error queue with the specified name should be created
`--auditQueueName`: Specifies that an audit queue with the specified name should be created
`--instanceDiscriminators`: Specifies a list of instance discriminators to use when the endpoint needs uniquely addressable instances

### rabbitmq-transport  queue migrate-to-quorum

Migrate an existing classic queue to quorum queues.

```
rabbitmq-transport queue migrate-to-quorum MyQueue --connectionString host=localhost
```

NOTE: The command only support the Direct routing topology currently

#### options

`--connectionString` | `-c` : The connection string to use
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. --connectionString, if specified, will take precedence over this option
`--certPath`: Set the path to the client certificate file for connecting to the broker
`--certPassphrase`: The passphrase for client certificate file for when using a client certificate
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate
`--routingTopology` | `-r`: Specifies which routing toplogy to use, valid options: Conventional|Direct
`--useDurableEntities` | `-d`: Specifies if entities should be created as durable
`--queueType` | `-c`: Specifies queue type will be used for queue creation, valid options: Quorum|Classic
`--useExternalAuth`: Use the external authorization option when connecting to the broker

include: return-to-source-queue
