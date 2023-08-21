// Parameters
@description('Specifies the name of the Azure Container Apps Environment.')
param name string

@description('Specifies the location.')
param location string = resourceGroup().location

@description('Specifies the resource tags.')
param tags object

@description('Specifies whether the environment only has an internal load balancer. These environments do not have a public static IP resource. They must provide infrastructureSubnetId if enabling this property')
param internal bool = false

@description('Specifies the IP range in CIDR notation assigned to the Docker bridge, network. Must not overlap with any other provided IP ranges.')
param dockerBridgeCidr string

@description('Specifies the IP range in CIDR notation that can be reserved for environment infrastructure IP addresses. Must not overlap with any other provided IP ranges.')
param platformReservedCidr string

@description('Specifies an IP address from the IP range defined by platformReservedCidr that will be reserved for the internal DNS server.')
param platformReservedDnsIP string

@description('Specifies whether the Azure Container Apps environment should be zone-redundant.')
param zoneRedundant bool = true

@description('Specifies the resource id of the infrastructure subnet.')
param infrastructureSubnetId string

@description('Specifies the name of the Log Analytics workspace.')
param workspaceName string

@description('Specifies the Azure Monitor instrumentation key used by Dapr to export Service to Service communication telemetry.')
param daprAIInstrumentationKey string = ''

@description('Specifies the configuration of Dapr component.')
param daprAIConnectionString string = ''

@description('Specifies the certificate password.')
@secure()
param certificatePassword	string = ''

@description('Specifies the PFX or PEM certificate value.')
param certificateValue string = ''

@description('Specifies the DNS suffix for the environment domain.')
param dnsSuffix	string = ''

@description('Specifies workload profiles configured for the Managed Environment.')
param workloadProfiles array = []

// Resources
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' existing = {
  name: workspaceName
}

resource environment 'Microsoft.App/managedEnvironments@2023-04-01-preview' = {
  name: name
  location: location
  tags: tags
  properties: {
    customDomainConfiguration: empty(certificatePassword) && empty(certificateValue) && empty(dnsSuffix)? null : {
      certificatePassword: certificatePassword
      certificateValue: certificateValue
      dnsSuffix: dnsSuffix
    }
    daprAIInstrumentationKey: daprAIInstrumentationKey
    daprAIConnectionString: daprAIConnectionString
    vnetConfiguration: {
      internal: internal
      infrastructureSubnetId: infrastructureSubnetId
      dockerBridgeCidr: dockerBridgeCidr
      platformReservedCidr: platformReservedCidr
      platformReservedDnsIP: platformReservedDnsIP
    }
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
    zoneRedundant: zoneRedundant
    workloadProfiles: workloadProfiles
  }
}

// Outputs
output id string = environment.id
output name string = environment.name
output daprConfiguration object = environment.properties.daprConfiguration
output kedaConfiguration object = environment.properties.kedaConfiguration
output appLogsConfiguration object = environment.properties.appLogsConfiguration
