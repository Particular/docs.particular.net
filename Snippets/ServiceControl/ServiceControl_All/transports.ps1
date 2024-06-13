# startcode MSMQPowerShellTransport
New-ServiceControlInstance -Transport MSMQ
New-ServiceControlAuditInstance -Transport MSMQ
New-MonitoringInstance -Transport MSMQ
# endcode

# startcode RabbitMQPowerShellTransport
New-ServiceControlInstance -Transport QuorumConventionalRouting
New-ServiceControlAuditInstance -Transport QuorumConventionalRouting
New-MonitoringInstance -Transport QuorumConventionalRouting
# endcode

# startcode AzureServiceBusPowerShellTransport
New-ServiceControlInstance -Transport NetStandardAzureServiceBus
New-ServiceControlAuditInstance -Transport NetStandardAzureServiceBus
New-MonitoringInstance -Transport NetStandardAzureServiceBus
# endcode

# startcode SQLServerPowerShellTransport
New-ServiceControlInstance -Transport SQLServer
New-ServiceControlAuditInstance -Transport SQLServer
New-MonitoringInstance -Transport SQLServer
# endcode

# startcode AmazonSQSPowerShellTransport
New-ServiceControlInstance -Transport AmazonSQS
New-ServiceControlAuditInstance -Transport AmazonSQS
New-MonitoringInstance -Transport AmazonSQS
# endcode
