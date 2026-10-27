### ServiceControl/Https.CertificatePath

_Added in version 6.11.0_

The path to the PFX or PEM certificate file. If hosting as a container, the certificate file can also be volume-mounted to the container:

```text
-v certificate.pfx:/usr/share/ParticularSoftware/certificate.pfx
```

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_HTTPS_CERTIFICATEPATH` |
| **App config key** | `ServiceControl/Https.CertificatePath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | None |
