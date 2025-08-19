---
title: Azure Service Bus KeyVault Sample
summary: Demonstrates how to use KeyVault to store connection string to the Azure Service Bus
reviewed: 2025-08-04
component: ASBS
related:
- transports/azure-service-bus
---

## Prerequisites

include: asb-connectionstringkeyvault-xplat

## Code walk-through

This sample shows a basic two-endpoint scenario exchanging messages with each other so that:

* Endpoints extract the connection string from Azure KeyVault.
* `Endpoint1` sends a `Message1` message to `Endpoint2`.
* `Endpoint2` replies to `Endpoint1` with a `Message2` instance.

### KeyVault client

snippet: config

### Running the sample

> [!NOTE]
> As a general practice security credentials should not be stored in environment variables, .env files, or hardcode in the source code. One option to achieve this is to rely on the `DefaultAzureCredential` which will try to access security information based on the runtime environment e.g. via Azure CLI locally, or via environment variables when running on Azure.

If running this sample on a machine in Azure (e.g., Virtual Machine, Azure Function, etc.) and authenticating with a Service Principal:

- [Assign KeyVault permissions to the Service Principal (SPN)](https://learn.microsoft.com/en-us/azure/key-vault/general/rbac-guide?tabs=azure-cli) of the host.
- `DefaultAzureCredential` will use either `EnvironmentCredential` or `ManagedIdentityCredential` to authenticate automatically.

If running this sample on a developer computer and authenticating with your domain account:

- Install Azure CLI as in [the documentation](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest).
- [Grant proper permissions to the account you use](https://learn.microsoft.com/en-us/azure/key-vault/general/rbac-guide?tabs=azure-cli) to authenticate against Azure.
- Authenticate in the Azure CLI using `az cli` command.
- If you have access to multiple tenants, you may need to specify the correct one explicitly: `az login --tenant <TENANT_ID>`.
- `DefaultAzureCredential` will use `AzureCliCredential` to authenticate.

Otherwise, configure your environment accordingly, or provide a properly configured `TokenCredential`:

- See the supported mechanisms in [the documentation](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential?view=azure-dotnet).

