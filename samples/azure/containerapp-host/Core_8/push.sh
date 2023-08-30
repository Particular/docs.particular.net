#!/bin/bash

# Variables
source ./variables.sh

# Login to ACR
az acr login --name $acrName 

# Retrieve ACR login server. Each container image needs to be tagged with the loginServer name of the registry. 
loginServer=$(az acr show --name $acrName --query loginServer --output tsv)

# Use a for loop to tag and push the local docker images to the Azure Container Registry
for index in ${!images[@]}; do
  #dotnet publish -c Release --os linux --arch x64 //t:PublishContainer #-p ContainerRegistry=$acrName
  dotnet publish -c Release --os linux --arch x64 //t:PublishContainer -p ContainerRegistry=$acrEndpoint
done