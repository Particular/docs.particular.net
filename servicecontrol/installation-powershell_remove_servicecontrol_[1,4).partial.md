The following commands show how to remove a ServiceControl instance(s). To list existing instances of the ServiceControl service use `Get-ServiceControlInstances`.

Remove the instance that was created in the Add sample and delete the database and logs:

```ps
Remove-ServiceControlInstance -Name Test.ServiceControl -RemoveDB -RemoveLogs
```

Remove all ServiceControl instances created in the Add sample and delete the database and logs for each one:

```ps
Get-ServiceControlInstances | Remove-ServiceControlInstance -RemoveDB -RemoveLogs
```

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string, or host name.