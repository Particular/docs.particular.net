Set the default container using the following configuration API:

snippet: CosmosDBDefaultContainer

The container that by default is used for all incoming messages is specified via `DefaultContainer(..)`. When installers are enabled this (default) container will be created if it doesn't exist.

To opt-out from creating the default container either disable the installers or use

snippet: CosmosDBDisableContainerCreation

Any other containers that are resolved by extracing partition information from incoming messages need to be [manually created in Azure](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/how-to-create-container)
