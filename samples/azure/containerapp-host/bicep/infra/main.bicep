// Parameters
@description('Specifies the name prefix.')
param prefix string = '$uniqueString(resourceGroup().id)'

@description('Specifies the location.')
param location string = resourceGroup().location

@description('Specifies the resource tags.')
param tags object = {
  IaC: 'Bicep'
  Sample: 'NServiceBus Container App'
}

@description('Specifies the name of the Azure Container Apps Environment.')
param containerAppEnvironmentName string = '${prefix}-environment'

@description('Specifies whether the environment only has an internal load balancer. These environments do not have a public static IP resource. They must provide infrastructureSubnetId if enabling this property')
param internal bool = false

@description('Specifies the IP range in CIDR notation assigned to the Docker bridge, network. Must not overlap with any other provided IP ranges.')
param dockerBridgeCidr string = '10.2.0.1/16'

@description('Specifies the IP range in CIDR notation that can be reserved for environment infrastructure IP addresses. Must not overlap with any other provided IP ranges.')
param platformReservedCidr string = '10.1.0.0/16'

@description('Specifies an IP address from the IP range defined by platformReservedCidr that will be reserved for the internal DNS server.')
param platformReservedDnsIP string = '10.1.0.2'

@description('Specifies whether the Azure Container Apps environment should be zone-redundant.')
param zoneRedundant bool = true

@description('Specifies the name of the Log Analytics workspace.')
param logAnalyticsWorkspaceName string = '${prefix}-workspace'

@description('Specifies the workspace data retention in days. -1 means Unlimited retention for the Unlimited Sku. 730 days is the maximum allowed for all other Skus.')
param logAnalyticsRetentionInDays int = 30

@description('Specifies the service tier of the workspace: Free, Standalone, PerNode, Per-GB.')
@allowed([
  'Free'
  'Standalone'
  'PerNode'
  'PerGB2018'
])
param logAnalyticsSku string = 'PerNode'

@description('Specifies the name of the virtual network.')
param virtualNetworkName string = '${prefix}-vnet'

@description('Specifies the address prefixes of the virtual network.')
param virtualNetworkAddressPrefixes string = '10.0.0.0/8'

@description('Specifies the name of a subnet for the Azure Container Apps environment infrastructure components. This subnet must be in the same VNET as the subnet defined in runtimeSubnetId. Must not overlap with any other provided IP ranges.')
param infrastructureSubnetName string = 'InfrastructureSubnet'

@description('Specifies the address prefix of a subnet for the Azure Container Apps environment infrastructure components. This subnet must be in the same VNET as the subnet defined in runtimeSubnetId. Must not overlap with any other provided IP ranges.')
param infrastructureSubnetAddressPrefix string = '10.0.0.0/21'

@description('Specifies the name of the subnet hosting the worker nodes of the user agent pool of the AKS cluster.')
param privateLinkServiceSubnetName string = 'PrivateSubnet'

@description('Specifies the address prefix of the subnet hosting the worker nodes of the user agent pool of the AKS cluster.')
param privateLinkServiceSubnetAddressPrefix string = '10.0.16.0/21'

@description('Specifies the name of the Service Bus namespace.')
param serviceBusNamespace string = '${prefix}-servicebus'

@description('Enabling this property creates a Premium Service Bus Namespace in regions supported availability zones.')
param serviceBusZoneRedundant bool = false

@description('Specifies the name of Service Bus namespace SKU.')
@allowed([
  'Basic'
  'Premium'
  'Standard'
])
param serviceBusSkuName string = 'Standard'

@description('Specifies the messaging units for the Service Bus namespace. For Premium tier, capacity are 1,2 and 4.')
param serviceBusCapacity int = 1

@description('Specifies the name of the Service Bus queue.')
param serviceBusQueueNames array = [ 'Sender', 'Receiver' ]

@description('Specifies the lock duration of the queue.')
param serviceBusQueueLockDuration string = 'PT5M'

@description('Specifies the maximum delivery count of the queue.')
param serviceBusQueueMaxDeliveryCount int = 10

@description('Specifies whether duplication is enabled on the queue.')
param serviceBusQueueRequiresDuplicateDetection bool = false

@description('Specifies whether session is enabled on the queue.')
param serviceBusQueueRequiresSession bool = false

@description('Specifies whether dead lettering is enabled on the queue.')
param serviceBusQueueDeadLetteringOnMessageExpiration bool = false

@description('Specifies the duplicate detection history time of the queue.')
param serviceBusQueueDuplicateDetectionHistoryTimeWindow string = 'PT10M'

@description('Specifies the name of the private link to the storage account.')
param serviceBusNamespacePrivateEndpointName string = '${prefix}-service-bus-namespace-private-endpoint'

@description('Specifies the name of the user-defined managed identity.')
param managedIdentityName string = '${prefix}-job-managed-identity'

@description('Specifies the name of the Azure Container Registry')
@minLength(5)
@maxLength(50)
param acrName string = '${prefix}-acr'

@description('Enable admin user that have push / pull permission to the registry.')
param acrAdminUserEnabled bool = false

@description('Specifies the name of the private link to the Azure Container Registry.')
param acrPrivateEndpointName string = '${prefix}-acr-private-endpoint'

@description('Specifies the SKU name of the Azure Container Registry.')
@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param acrSkuName string = 'Standard'

// Resources
module workspace 'log-analytics-workspace.bicep' = {
  name: 'workspace'
  scope: resourceGroup()
  params: {
    name: logAnalyticsWorkspaceName
    sku: logAnalyticsSku
    retentionInDays: logAnalyticsRetentionInDays
    location: location
    tags: tags
  }
}

module namespace 'service-bus.bicep' = {
  name: 'namespace'
  scope: resourceGroup()
  params: {
    name: serviceBusNamespace
    location: location
    tags: tags
    zoneRedundant: serviceBusZoneRedundant
    skuName: serviceBusSkuName
    capacity: serviceBusCapacity
    queueNames: serviceBusQueueNames
    lockDuration: serviceBusQueueLockDuration
    maxDeliveryCount: serviceBusQueueMaxDeliveryCount
    requiresDuplicateDetection: serviceBusQueueRequiresDuplicateDetection
    requiresSession: serviceBusQueueRequiresSession
    deadLetteringOnMessageExpiration: serviceBusQueueDeadLetteringOnMessageExpiration
    duplicateDetectionHistoryTimeWindow: serviceBusQueueDuplicateDetectionHistoryTimeWindow
    workspaceId: workspace.outputs.id
  }
}

module network 'virtual-network.bicep' = {
  name: 'network'
  scope: resourceGroup()
  params: {
    name: virtualNetworkName
    location: location
    tags: tags
    addressPrefixes: virtualNetworkAddressPrefixes
    infrastructureSubnetName: infrastructureSubnetName
    infrastructureSubnetAddressPrefix: infrastructureSubnetAddressPrefix
    privateLinkServiceSubnetName: privateLinkServiceSubnetName
    privateLinkServiceSubnetAddressPrefix: privateLinkServiceSubnetAddressPrefix
    workspaceId: workspace.outputs.id
    serviceBusNamespacePrivateEndpointEnabled: serviceBusSkuName == 'Premium'
    serviceBusNamespacePrivateEndpointName: serviceBusNamespacePrivateEndpointName
    serviceBusNamespaceId: namespace.outputs.id
    acrPrivateEndpointEnabled: acrSkuName == 'Premium'
    acrPrivateEndpointName: acrPrivateEndpointName
    acrId: acr.outputs.id
  }
}

module environment 'container-apps-environment.bicep' = {
  name: 'environment'
  scope: resourceGroup()
  params: {
    name: containerAppEnvironmentName
    location: location
    tags: tags
    internal: internal
    dockerBridgeCidr: dockerBridgeCidr
    platformReservedCidr: platformReservedCidr
    platformReservedDnsIP: platformReservedDnsIP
    zoneRedundant: zoneRedundant
    workspaceName: logAnalyticsWorkspaceName
    infrastructureSubnetId: network.outputs.infrastructureSubnetId
  }
}

module managedIdentity 'managed-identity.bicep' = {
  name: 'managedIdentity'
  scope: resourceGroup()
  params: {
    name: managedIdentityName
    location: location
    serviceBusNamespace: namespace.outputs.name
    acrName: acr.outputs.name
    tags: tags
  }
}

module acr 'container-registry.bicep' = {
  name: 'containerRegistry'
  params: {
    name: acrName
    sku: acrSkuName
    adminUserEnabled: acrAdminUserEnabled
    workspaceId: workspace.outputs.id
    retentionInDays: logAnalyticsRetentionInDays
    location: location
    tags: tags
  }
}
