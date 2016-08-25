
Because MSMQ does not allow performant remote receives in most cases scaling out requires sender-side round-robin distribution. When using MSMQ different instances are usually deployed to different (virtual) machines. The following instance mapping file (see [scaling out with sender-side distribution](sender-side-distribution.md)) shows scaling out of the `Sales` endpoint. System administrators are able to spin-up new instances of the endpoint should the load increase and the only requirement is adding an entry to the instance mapping file. No changes in the source code are required.

snippet:InstanceMappingFile-ScaleOut

The corresponding logical routing is

snippet:Routing-MapMessagesToLogicalEndpoints


WARNING: When using this scaling out technique in a mixed version environment make sure to deploy a distributor in front of the scaled out version 6 endpoint if that endpoint needs to communicate with endpoints using versions lower than 6 (refer to [the distributor sample](/samples/scaleout/distributor/) for details). Otherwise event messages will get duplicated and commands won't be properly distributed.