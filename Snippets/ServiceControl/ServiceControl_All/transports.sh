#!/bin/bash

# startcode RabbitMQDockerTransport
docker run -e TRANSPORTTYPE=QuorumConventionalRouting particular/servicecontrol:latest
docker run -e TRANSPORTTYPE=QuorumConventionalRouting particular/servicecontrol-audit:latest
docker run -e TRANSPORTTYPE=QuorumConventionalRouting particular/servicecontrol-monitoring:latest
# endcode

# startcode AzureServiceBusDockerTransport
docker run -e TRANSPORTTYPE=NetStandardAzureServiceBus particular/servicecontrol:latest
docker run -e TRANSPORTTYPE=NetStandardAzureServiceBus particular/servicecontrol-audit:latest
docker run -e TRANSPORTTYPE=NetStandardAzureServiceBus particular/servicecontrol-monitoring:latest
# endcode

# startcode SQLServerDockerTransport
docker run -e TRANSPORTTYPE=SQLServer particular/servicecontrol:latest
docker run -e TRANSPORTTYPE=SQLServer particular/servicecontrol-audit:latest
docker run -e TRANSPORTTYPE=SQLServer particular/servicecontrol-monitoring:latest
# endcode

# startcode AmazonSQSDockerTransport
docker run -e TRANSPORTTYPE=AmazonSQS particular/servicecontrol:latest
docker run -e TRANSPORTTYPE=AmazonSQS particular/servicecontrol-audit:latest
docker run -e TRANSPORTTYPE=AmazonSQS particular/servicecontrol-monitoring:latest
# endcode