

Running an **embedded** distributor process alongside the worker (also known as *Master mode*) is not supported in versions 6 and higher. Instead, a stand alone distributor endpoint running NServiceBus 5 should be used.

NOTE: This endpoints only acts like a load balancer, it does NOT require *any* handlers, sagas or message mappings. Once deployed, it does not require any version updates.

## Upgrading to NServiceBus 6

Worker configuration sections like `MasterNodeConfig` are obsolete in favor of the code API `endpointConfiguration.EnlistWithLegacyMSMQDistributor`. For more information refer to the [Upgrading a distributor-based scaled out endpoint to version 6](/samples/scaleout/distributor-upgrade/) article.
