// Parameters
@description('Specifies the name of the Azure Container Apps Environment.')
param name string

@description('Specifies the address prefixes of the virtual network.')
param addressPrefixes string = '10.0.0.0/8'

@description('Specifies the name of a subnet for the Azure Container Apps environment infrastructure components. This subnet must be in the same VNET as the subnet defined in runtimeSubnetId. Must not overlap with any other provided IP ranges.')
param infrastructureSubnetName string = 'InfrastructureSubnet'

@description('Specifies the address prefix of a subnet for the Azure Container Apps environment infrastructure components. This subnet must be in the same VNET as the subnet defined in runtimeSubnetId. Must not overlap with any other provided IP ranges.')
param infrastructureSubnetAddressPrefix string = '10.0.0.0/21'

@description('Specifies the name of the subnet hosting the worker nodes of the user agent pool of the AKS cluster.')
param privateLinkServiceSubnetName string = 'PlsSubnet'

@description('Specifies the address prefix of the subnet hosting the worker nodes of the user agent pool of the AKS cluster.')
param privateLinkServiceSubnetAddressPrefix string = '10.0.16.0/21'

@description('Specifies whether enabling the private link to the Azure Service Bus namespace.')
param serviceBusNamespacePrivateEndpointEnabled bool = false

@description('Specifies the name of the private link to the Azure Service Bus namespace.')
param serviceBusNamespacePrivateEndpointName string = 'ServiceBusNamespacePrivateEndpoint'

@description('Specifies the resource id of the Azure Service Bus namespace.')
param serviceBusNamespaceId string

@description('Specifies whether enabling the private link to the Azure Container Registry.')
param acrPrivateEndpointEnabled bool = false

@description('Specifies the name of the private link to the Azure Container Registry.')
param acrPrivateEndpointName string = 'AcrPrivateEndpoint'

@description('Specifies the resource id of the Azure Container Registry.')
param acrId string

@description('Specifies the resource id of the Log Analytics workspace.')
param workspaceId string

@description('Specifies the workspace data retention in days.')
param retentionInDays int = 60

@description('Specifies the location.')
param location string = resourceGroup().location

@description('Specifies the resource tags.')
param tags object

// Variables
var diagnosticSettingsName = 'diagnosticSettings'
var vnetLogCategories = [
  'VMProtectionAlerts'
]
var vnetMetricCategories = [
  'AllMetrics'
]
var vnetLogs = [for category in vnetLogCategories: {
  category: category
  enabled: true
  retentionPolicy: {
    enabled: true
    days: retentionInDays
  }
}]
var vnetMetrics = [for category in vnetMetricCategories: {
  category: category
  enabled: true
  retentionPolicy: {
    enabled: true
    days: retentionInDays
  }
}]

// Resources
resource vnet 'Microsoft.Network/virtualNetworks@2022-07-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressPrefixes
      ]
    }
    subnets: [
      {
        name: infrastructureSubnetName
        properties: {
          addressPrefix: infrastructureSubnetAddressPrefix
          delegations: []
          privateEndpointNetworkPolicies: 'Disabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
        }
      }
      {
        name: privateLinkServiceSubnetName
        properties: {
          addressPrefix: privateLinkServiceSubnetAddressPrefix
          delegations: []
          privateEndpointNetworkPolicies: 'Disabled'
          privateLinkServiceNetworkPolicies: 'Disabled'
        }
      }
    ]
    virtualNetworkPeerings: []
    enableDdosProtection: false
  }
}

// Private DNS Zones
resource serviceBusPrivateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = if (serviceBusNamespacePrivateEndpointEnabled) {
  name: 'privatelink.${toLower(environment().name) == 'azureusgovernment' ? 'servicebus.usgovcloudapi.net' : 'servicebus.windows.net'}'
  location: 'global'
  tags: tags
}

resource acrPrivateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = if (acrPrivateEndpointEnabled) {
  name: 'privatelink.${toLower(environment().name) == 'azureusgovernment' ? 'azurecr.us' : 'azurecr.io'}'
  location: 'global'
  tags: tags
}

// Virtual Network Links
resource serviceBusPrivateDnsZoneVirtualNetworkLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = if (serviceBusNamespacePrivateEndpointEnabled) {
  parent:serviceBusPrivateDnsZone
  name: 'link_to_${toLower(name)}'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnet.id
    }
  }
}

resource acrPrivateDnsZoneVirtualNetworkLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = if (acrPrivateEndpointEnabled) {
  parent: acrPrivateDnsZone
  name: 'link_to_${toLower(name)}'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnet.id
    }
  }
}

// Private Endpoints and Private DNS Zone Groups
resource serviceBusNamespacePrivateEndpoint 'Microsoft.Network/privateEndpoints@2021-08-01' = if (serviceBusNamespacePrivateEndpointEnabled) {
  name: serviceBusNamespacePrivateEndpointName
  location: location
  tags: tags
  properties: {
    privateLinkServiceConnections: [
      {
        name: serviceBusNamespacePrivateEndpointName
        properties: {
          privateLinkServiceId: serviceBusNamespaceId
          groupIds: [
            'namespace'
          ]
        }
      }
    ]
    subnet: {
      id: '${vnet.id}/subnets/${privateLinkServiceSubnetName}'
    }
  }
}

resource serviceBusNamespacePrivateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2021-08-01' = if (serviceBusNamespacePrivateEndpointEnabled) {
  parent: serviceBusNamespacePrivateEndpoint
  name: 'PrivateDnsZoneGroupName'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: 'dnsConfig'
        properties: {
          privateDnsZoneId: serviceBusPrivateDnsZone.id
        }
      }
    ]
  }
}

resource acrPrivateEndpoint 'Microsoft.Network/privateEndpoints@2022-09-01' = if (acrPrivateEndpointEnabled) {
  name: acrPrivateEndpointName
  location: location
  tags: tags
  properties: {
    privateLinkServiceConnections: [
      {
        name: acrPrivateEndpointName
        properties: {
          privateLinkServiceId: acrId
          groupIds: [
            'registry'
          ]
        }
      }
    ]
    subnet: {
      id: '${vnet.id}/subnets/${privateLinkServiceSubnetName}'
    }
  }
}

resource acrPrivateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2022-09-01' = if (acrPrivateEndpointEnabled) {
  parent: acrPrivateEndpoint
  name: 'acrPrivateDnsZoneGroup'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: 'dnsConfig'
        properties: {
          privateDnsZoneId: acrPrivateDnsZone.id
        }
      }
    ]
  }
}

// Diagnostic Settings
resource vnetDiagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: diagnosticSettingsName
  scope: vnet
  properties: {
    workspaceId: workspaceId
    logs: vnetLogs
    metrics: vnetMetrics
  }
}

// Outputs
// Outputs
output id string = vnet.id
output name string = vnet.name
output infrastructureSubnetId string = resourceId('Microsoft.Network/virtualNetworks/subnets', vnet.name, infrastructureSubnetName)
output privateLinkServiceSubnetId string = resourceId('Microsoft.Network/virtualNetworks/subnets', vnet.name, privateLinkServiceSubnetName)
