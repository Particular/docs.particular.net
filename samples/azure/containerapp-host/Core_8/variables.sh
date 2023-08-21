# Variables
location='northeurope'
deploymentName='main'
prefix='NServiceBus'
acrName="${prefix}Acr"
acrResourceGrougName="${prefix}RG"
senderImageName="sender"
receiverImageName="receiver"

images=($senderImageName $receiverImageName)
tag="v1"

# Azure Subscription and Tenant
subscriptionId=$(az account show --query id --output tsv)
subscriptionName=$(az account show --query name --output tsv)
tenantId=$(az account show --query tenantId --output tsv)