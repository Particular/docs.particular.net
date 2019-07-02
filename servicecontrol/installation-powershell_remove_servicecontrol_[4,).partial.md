The following commands show how to remove a ServiceControl instance(s). To list existing instances of the ServiceControl service use `Get-ServiceControlInstances`. To list existing instances of the ServiceControl Audit service use `Get-MonitoringInstances`. To list remote instances for a ServiceControl service use `Get-ServiceControlRemotes`.

Remove the instance that was created in the Add sample and delete the database and logs:

```ps
Remove-ServiceControlInstance -Name Test.ServiceControl -RemoveDB -RemoveLogs
```

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string, or host name.

If there is a ServiceControl Audit instance for the installation it must be removed first:

```ps
Remove-ServiceControlRemote -Name Test.ServiceControl -RemoteInstanceAddress http://localhost:44444/api

Remove-AuditInstance -Name Test.ServiceControl.Audit -RemoveDB -RemoveLogs
```