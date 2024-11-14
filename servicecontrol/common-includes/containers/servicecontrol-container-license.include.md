### License

_Environment variable:_ `PARTICULARSOFTWARE_LICENSE`

The Particular Software license, which is most easily provided to a container [as an environment variable](/nservicebus/licensing/#license-management-environment-variable). The environment variable should contain the full multi-line contents of the license file.

A license file can also be volume-mounted to the container in the [machine-wide license location for Linux](/nservicebus/licensing/#license-management-machine-wide-license-location):

```shell
-v license.xml:/usr/share/ParticularSoftware/license.xml
```