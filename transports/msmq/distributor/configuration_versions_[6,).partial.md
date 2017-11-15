

Running an **embedded** distributor process alongside the worker (also known as *Master mode*) is not supported in Versions 6 and higher. Instead, a stand alone Distributor endpoint running NServiceBus 5 should be used.

NOTE: This endpoints only acts like a load balancer, it does NOT require *any* handlers, sagas or message mappings. Once deployed, it does not require any version updates.

## Upgrading to NServiceBus 6

Worker configuration sections like `MasterNodeConfig` are obsoleted in favor or our code API `endpointConfiguraiton.EnlistWithLegacyMSMQDistributor`. For more information refer to [Upgrading a Distributor-based scaled out endpoint to Version 6](/samples/scaleout/distributor-upgrade/) documentation.
