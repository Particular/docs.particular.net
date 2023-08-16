// Reading
// https://azureossd.github.io/2023/01/03/Using-Managed-Identity-and-Bicep-to-pull-images-with-Azure-Container-Apps/
// https://github.com/Azure-Samples/container-apps-jobs/
// https://techcommunity.microsoft.com/t5/fasttrack-for-azure/how-to-create-azure-container-apps-jobs-with-bicep-and-azure-cli/ba-p/3863968
// https://github.com/Azure-Samples/container-apps-jobs
// https://www.thorsten-hans.com/background-workers-in-azure-container-apps-with-keda/
// https://www.thorsten-hans.com/az-containerapp-aka-amazing-loop-performance/
// https://www.thorsten-hans.com/managed-identities-with-azure-container-apps/
// https://azure.github.io/aca-dotnet-workshop/aca/09-aca-autoscale-keda/
// https://learn.microsoft.com/en-us/azure/container-apps/scale-app
// https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/install#azure-cli

// Parameters
@description('Specifies the name of the user-defined managed identity.')
param name string

@description('Specifies the name of the Service Bus namespace.')
param serviceBusNamespace string

@description('Specifies the name of the Azure Container Registry')
param acrName string

@description('Specifies the location.')
param location string = resourceGroup().location

@description('Specifies the resource tags.')
param tags object


module managedIdentity 'identity.bicep' = {
  name: 'nservicebus-containerapp-sample'
  params: {
      name: 'env-${name}'
      location: location
  }
}
