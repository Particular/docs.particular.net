#!/bin/bash

# Variables
source ./variables.sh

# Login to ACR
token=$(az acr login --name $acrName --expose-token --output tsv --query accessToken)

# Expose the token to the container task
export SDK_CONTAINER_REGISTRY_PWORD=$token
export SDK_CONTAINER_REGISTRY_UNAME="00000000-0000-0000-0000-000000000000"

# Retrieve ACR login server
loginServer=$(az acr show --name $acrName --query loginServer --output tsv)

# Use a for loop to tag and push the local docker images to the Azure Container Registry
dotnet publish -c Release //t:PublishContainer -p ContainerRegistry=$loginServer