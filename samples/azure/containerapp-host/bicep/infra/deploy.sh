#!/bin/bash

# Variables
location='northeurope'
deploymentName='main'
prefix='NServiceBus'
resourceGroupName="${prefix}RG"

# Commands
validateTemplate=1
useWhatIf=1
deploy=1

# Template
template="main.bicep"
parameters="main.parameters.json"

# Subscription id, subscription name, and tenant id of the current subscription
subscriptionId=$(az account show --query id --output tsv)
subscriptionName=$(az account show --query name --output tsv)
tenantId=$(az account show --query tenantId --output tsv)

# Check if the resource group already exists
echo "Checking if [$resourceGroupName] resource group actually exists in the [$subscriptionName] subscription..."

az group show --name $resourceGroupName &>/dev/null

if [[ $? != 0 ]]; then
  echo "No [$resourceGroupName] resource group actually exists in the [$subscriptionName] subscription"
  echo "Creating [$resourceGroupName] resource group in the [$subscriptionName] subscription..."
  
  # Create the resource group
  az group create --name $resourceGroupName --location $location 1>/dev/null
  
  if [[ $? == 0 ]]; then
    echo "[$resourceGroupName] resource group successfully created in the [$subscriptionName] subscription"
  else
    echo "Failed to create [$resourceGroupName] resource group in the [$subscriptionName] subscription"
    exit
  fi
else
  echo "[$resourceGroupName] resource group already exists in the [$subscriptionName] subscription"
fi

# Validate the Bicep template
if [[ $validateTemplate == 1 ]]; then
  if [[ $useWhatIf == 1 ]]; then
      # Execute a deployment What-If operation at resource group scope.
      echo "Previewing changes deployed by [$template] Bicep template..."
      az deployment group what-if \
      --resource-group $resourceGroupName \
      --template-file $template \
      --no-pretty-print \
      --output jsonc \
      --parameters $parameters \
      --parameters location=$location \
      prefix=$prefix
      
      if [[ $? == 0 ]]; then
        echo "[$template] Bicep template validation succeeded"
      else
        echo "Failed to validate [$template] Bicep template"
        exit
      fi
  else
    # Validate the Bicep template
    echo "Validating [$template] Bicep template..."
    output=$(az deployment group validate \
      --resource-group $resourceGroupName \
      --template-file $template \
      --no-pretty-print \
      --output jsonc \
      --parameters $parameters \
      --parameters location=$location \
      prefix=$prefix)
    
    if [[ $? == 0 ]]; then
      echo "[$template] Bicep template validation succeeded"
    else
      echo "Failed to validate [$template] Bicep template"
      echo $output
      exit
    fi
  fi
fi

# Deploy the Bicep template
if [[ $deploy == 1 ]]; then
  az deployment group create \
  --name $deploymentName \
  --resource-group $resourceGroupName \
  --template-file $template \
  --parameters $parameters \
  --parameters location=$location \
    prefix=$prefix 1>/dev/null

  if [[ $? == 0 ]]; then
      echo "[$deploymentName] deployment successfully created in the [$resourceGroupName] resource group"
  else
      echo "Failed to create [$deploymentName] deployment in the [$resourceGroupName] resource group"
      exit -1
  fi
fi