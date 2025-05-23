### RavenDB connection string

_Environment variable:_ `RAVENDB_CONNECTIONSTRING`

Provides the URL to connect to the database container that stores the instance's data. The database container should be exclusive to the instance, and not shared by any other ServiceControl instances.

If the [storage requirements for the RavenDB container](/servicecontrol/ravendb/containers.md#required-settings) cannot be met by the container hosting infrastructure, especially in cloud-hosted environments, an externally-hosted and separately-licensed RavenDB instance can also be used starting with ServiceControl version 6.0.

In this case, the RavenDB Major.Minor version must match the version expected by ServiceControl as shown in this table:

include: servicecontrol-ravendb-versions

#### RavenDB client certificate

_Added in ServiceControl version 6.3.0_

When connecting to an external RavenDB instance, the RavenDB client certificate can be specified using a combination of settings, which are attempted in this order:

1. An environment variable `RAVENDB_CLIENTCERTIFICATEBASE64` can be used to supply the client certificate as a Base64-encoded string.
2. An environment variable `RAVENDB_CLIENTCERTIFICATEPATH` can be used to identify the local path to a certificate that has added to the container via a mounted volume.
3. The app will attempt to load the certificate from `/app/raven-client-certificate.pfx`.
4. The app will attempt to access the database without a client certificate.

In any of the above cases, the certificate can be password-protected, in which case the password can be supplied using the `RAVENDB_CLIENTCERTIFICATEPASSWORD` environment variable.