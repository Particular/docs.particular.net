#!/bin/bash

# Variables
source ./variables.sh

# Login to ACR
az acr login --name $acrName

# Login to ACR
token=$(az acr login --name $acrName --expose-token --output tsv --query accessToken)

# Login docker
#docker login $loginServer -u $acrName -p $token

# Expose the token to the container task
export SDK_CONTAINER_REGISTRY_PWORD=$token
export SDK_CONTAINER_REGISTRY_UNAME=$acrName

# Retrieve ACR login server
loginServer=$(az acr show --name $acrName --query loginServer --output tsv)

# Use a for loop to tag and push the local docker images to the Azure Container Registry
for index in ${!images[@]}; do
  dotnet publish -c Release /t:PublishContainer -p ContainerRegistry=$loginServer
done