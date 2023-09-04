// Parameters
@description('Specifies the name prefix.')
param prefix string = '$uniqueString(resourceGroup().id)'

@description('Specifies the location.')
param location string = resourceGroup().location

@description('Specifies the resource tags.')
param tags object = {
  IaC: 'Bicep'
  Demo: 'Azure Container Apps Jobs'
}

@description('Specifies the name of the Azure Container Apps Environment.')
param containerAppEnvironmentName string = '${prefix}-environment'

@description('Specifies the Azure Container Registry name.')
param acrName string = '${prefix}-acr'

@description('Specifies the name of the Service Bus namespace.')
param serviceBusNamespace string = '${prefix}-servicebus'

@description('Specifies the name of the user-defined managed identity.')
param managedIdentityName string = '${prefix}-job-managed-identity'

@description('Specifies the name of the parameters Azure Service Bus queue.')
param parametersServiceBusQueueName string = 'parameters'

@description('Specifies the name of the results Azure Service Bus queue.')
param resultsServiceBusQueueName string = 'results'

@description('Specifies the name of the sender job.')
param senderJobName string = '${prefix}-sender'

@description('Specifies the name of the processor job.')
param processorJobName string = '${prefix}-processor'

@description('Specifies the name of the receiver job.')
param receiverJobName string = '${prefix}-receiver'

@description('Specifies the name (e.g., sbsender) of the container image of the sender job.')
param senderImageName string = 'sender'

@description('Specifies the name (e.g., sbreceiver) of the container image of the receiver job.')
param receiverImageName string = 'receiver'

@description('Specifies the tag (e.g., v1) of the container image of the sender job.')
param senderImageTag string = 'v1'

@description('Specifies the tag (e.g., v1) of the container image of the receiver job.')
param receiverImageTag string = 'v1'

@description('Maximum number of replicas of the sender job to run per execution.')
param senderParallelism int = 1

@description('Maximum number of replicas of the sender job to run per execution.')
param receiverParallelism int = 5

@description('Specifies the minimum number of job executions to run per polling interval.')
param minExecutions int = 1

@description('Specifies the maximum number of job executions to run per polling interval.')
param maxExecutions int = 10

@description('Specifies the polling interval in seconds.')
param pollingInterval int = 30

@description('Specifies the maximum number of retries before the replica fails..')
param replicaRetryLimit int = 1

@description('Specifies the maximum number of seconds a replica can execute.')
param replicaTimeout int = 300

// Existing Resources
resource environment 'Microsoft.App/managedEnvironments@2023-04-01-preview' existing = {
  name: containerAppEnvironmentName
}

resource acr 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' existing = {
  name: acrName
}

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: managedIdentityName
}

resource namespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' existing = {
  name: serviceBusNamespace
}

// Variables
var serviceBusEndpoint = '${namespace.id}/AuthorizationRules/RootManageSharedAccessKey'
var serviceBusConnectionString = listKeys(serviceBusEndpoint, namespace.apiVersion).primaryConnectionString

// Modules
module senderJob 'container-apps-job.bicep' = {
  name: 'senderJob'
  params: {
    name: toLower(senderJobName)
    location: location
    managedIdentityId: managedIdentity.id
    containerImage: '${acr.properties.loginServer}/${senderImageName}:${senderImageTag}'
    triggerType: 'Manual'
    parallelism: senderParallelism
    replicaCompletionCount: senderParallelism
    replicaRetryLimit: replicaRetryLimit
    replicaTimeout: replicaTimeout
    environmentId: environment.id
    tags: tags
    registries: [
      {
        server: acr.properties.loginServer
        identity: managedIdentity.id
      }
    ]
    env: [
      {
        name: 'AZURE_CLIENT_ID'
        value: managedIdentity.properties.clientId
      }
      {
        name: 'FULLY_QUALIFIED_NAMESPACE'
        value: toLower('${serviceBusNamespace}.servicebus.windows.net')
      }
      {
        name: 'INPUT_QUEUE_NAME'
        value: parametersServiceBusQueueName
      }
      {
        name: 'MIN_NUMBER'
        value: '1'
      }
      {
        name: 'MAX_NUMBER'
        value: '10'
      }
      {
        name: 'MESSAGE_COUNT'
        value: '100'
      }
    ]
  }
}

module receiverJob 'container-apps-job.bicep' = {
  name: 'receiverJob'
  params: {
    name: receiverJobName
    location: location
    managedIdentityId: managedIdentity.id
    containerImage: '${acr.properties.loginServer}/${receiverImageName}:${receiverImageTag}'
    triggerType: 'Event'
    maxExecutions: maxExecutions
    minExecutions: minExecutions
    pollingInterval: pollingInterval
    parallelism: receiverParallelism
    replicaCompletionCount: receiverParallelism
    replicaRetryLimit: replicaRetryLimit
    replicaTimeout: replicaTimeout
    environmentId: environment.id
    tags: tags
    registries: [
      {
        server: acr.properties.loginServer
        identity: managedIdentity.id
      }
    ]
    env: [
      {
        name: 'AZURE_CLIENT_ID'
        value: managedIdentity.properties.clientId
      }
      {
        name: 'FULLY_QUALIFIED_NAMESPACE'
        value: toLower('${serviceBusNamespace}.servicebus.windows.net')
      }
      {
        name: 'OUTPUT_QUEUE_NAME'
        value: resultsServiceBusQueueName
      }
      {
        name: 'MAX_MESSAGE_COUNT'
        value: '20'
      }
      {
        name: 'MAX_WAIT_TIME'
        value: '5'
      }
    ]
    secrets: [
      {
        name: 'service-bus-connection-string'
        value: serviceBusConnectionString
      }
    ]
    rules: [
      {
        name: 'azure-servicebus-queue-rule'
        type: 'azure-servicebus'
        metadata: {
          messageCount: '5'
          namespace: serviceBusNamespace
          queueName: resultsServiceBusQueueName
        }
        auth: [
          {
            secretRef: 'service-bus-connection-string'
            triggerParameter: 'connection'
          }
        ]
      }
    ]
  }
}
