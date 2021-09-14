## Assembly scanning

[Assembly scanning](/nservicebus/hosting/assembly-scanning.md) loads assemblies from two locations:

- The `bin` directory of the Azure Functions application
- The Azure Functions runtime directory

If the same assembly is present in both locations, an exception is thrown which prevents the endpoint from running. Contact [Particular Software support](https://particular.net/support) for assistance.
