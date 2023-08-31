# Variables
location='northeurope'
deploymentName='main'
prefix='nservicebus'
acrName="${prefix}acr"
senderImageName="sender"
receiverImageName="receiver"

images=($senderImageName $receiverImageName)

# Azure Subscription and Tenant
subscriptionId=$(az account show --query id --output tsv)
subscriptionName=$(az account show --query name --output tsv)
tenantId=$(az account show --query tenantId --output tsv)