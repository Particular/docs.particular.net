> [!NOTE]
> Environment variables take precedence over App.config settings.

| Instance                  | Environment Variable Prefix | App.Config Prefix      | Default Port |
|---------------------------|-----------------------------|------------------------|--------------|
| ServiceControl (Primary)  | `SERVICECONTROL`            | `ServiceControl`       | 33333        |
| ServiceControl.Audit      | `SERVICECONTROL_AUDIT`      | `ServiceControl.Audit` | 44444        |
| ServiceControl.Monitoring | `MONITORING`                | `Monitoring`           | 33633        |
