Command to create Azure Functions trigger queue using Azure CLI


ASB

```
az servicebus queue create --name <function-name> --namespace-name <asb-namespace-to-use> --resource-group <resource-group-containing-namespace>
```

ASQ

Command to create an ASB queue using Azure CLI:
```
 az storage queue create --name <function-name> --connection-string "<storage-account-connection-string>"
 ```