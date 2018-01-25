To start the endpoint as a Distributor, install the [NServiceBus.Distributor.MSMQ NuGet](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ) and then run the host from the command line as follows:

```dos
NServiceBus.Host.exe NServiceBus.MSMQDistributor
```

The NServiceBus.MSMQDistributor profile instructs the NServiceBus framework to start a distributor process on this endpoint, waiting for workers to enlist to it. Unlike the NServiceBus.MSMQMaster profile, the NServiceBus.MSMQDistributor profile does not execute a worker process.
