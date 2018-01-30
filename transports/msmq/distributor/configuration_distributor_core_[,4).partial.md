To start the endpoint as a Distributor, run the host from the command line as follows:

```dos
NServiceBus.Host.exe NServiceBus.Distributor
```

The NServiceBus.Distributor profile instructs the NServiceBus framework to start a distributor on this endpoint, waiting for workers to enlist to it. Unlike the NServiceBus.Master profile, the NServiceBus.Distributor profile does not execute a worker process.
