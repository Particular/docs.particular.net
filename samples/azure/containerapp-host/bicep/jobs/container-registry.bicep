// Parameters
@description('Specifies the name of the Azure Container Registry.')
param name string

@description('Specifies the principal id of the user-defined managed identity.')
param managedIdentityPrincipalId string

// Variables
var acrPullRoleDefinitionId = resourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')

// Resources
resource acr 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' existing = {
  name: name
}

resource acrPullRoleDefinitionAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name:  guid(managedIdentityPrincipalId, acr.id, acrPullRoleDefinitionId)
  scope: acr
  properties: {
    roleDefinitionId: acrPullRoleDefinitionId
    principalId: managedIdentityPrincipalId
    principalType: 'ServicePrincipal'
  }
}

//Outputs
output id string = acr.id
output name string = acr.name
output loginServer string = acr.properties.loginServer
