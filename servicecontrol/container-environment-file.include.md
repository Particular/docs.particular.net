These parameters can also be added by using a standard Docker environment file. Each parameter has its line and does not need to be enclosed by quotes. It can then be used by Docker as follows:

```cmd
docker run --env-file servicecontrol.env [dockerimage]
```

An example of the `servicecontrol.env` file:

```env
# License text
ServiceControl/LicenseText=<?xml version="1.0" encoding="utf-8"?><license id="..."></license>

# Connection string
ServiceControl/ConnectionString=data source=[server],1433; user id=username; password=[password]; Initial Catalog=servicecontrol

# Remote audit instances
ServiceControl/RemoteInstances=[{'api_uri':'http://[hostname]:44444/api'}]
```
