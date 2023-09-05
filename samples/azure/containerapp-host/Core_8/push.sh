#!/bin/bash

# Variables
source ./variables.sh

# Login to ACR and grab the one-time token
token=$(az acr login --name $acrName --expose-token --output tsv --query accessToken)

# Based on https://learn.microsoft.com/en-us/azure/container-registry/container-registry-authentication?tabs=azure-cli#az-acr-login-with---expose-token
# we should be using:
# docker login $loginServer -u "00000000-0000-0000-0000-000000000000" --password-stdin <<< $token
# but it seems to not pass the credentials correctly to dotnet publish
# resulting in 401 for one of the projects build and a success for another

# Accorind to https://github.com/dotnet/sdk-container-builds/blob/main/docs/RegistryAuthentication.md#authentication-via-environment-variables we could use an empty uname and pass token in 
# an environment variable
# export SDK_CONTAINER_REGISTRY_PWORD=$token
# export SDK_CONTAINER_REGISTRY_UNAME=""
# but it fails to log in

# What appears to work is a combination of these two, passing 00000000-0000-0000-0000-000000000000 as uname in the env variable:
export SDK_CONTAINER_REGISTRY_PWORD=$token
export SDK_CONTAINER_REGISTRY_UNAME="00000000-0000-0000-0000-000000000000"

# Running dotnet publish on the solution level -- no need to iterate over projects
dotnet publish -c Release //t:PublishContainer -p ContainerRegistry=$loginServer
