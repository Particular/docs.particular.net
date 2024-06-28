## Troubleshooting via PowerShell

The ServiceControl Management PowerShell module offers some cmdlets to assist with troubleshooting the installation of ServiceControl instances.

### Check if a port is already in use

Before adding an instance of ServiceControl test if the port to use is currently in use.

snippet: ps-testport

This example shows the available ports out of a range of ports

snippet: ps-testportrange

If the port is already in use, then choose a different port.

### Checking and manipulating UrlAcls

The Window HTTPServer API is used by underlying components in ServiceControl. This API uses a permissions system to limit what accounts can add a HTTP listener to a specific URI. The standard mechanism for viewing and manipulating these ports is with the [netsh.exe](https://technet.microsoft.com/en-us/library/Cc725882.aspx) command line tool.

For example `netsh.exe http show urlacl` will list all of the available UrlAcls. This output is detailed but not easy to query. The ServiceControl Management PowerShell provides simplified PowerShell equivalents for listing, adding, and removing UrlAcls and makes the output easier to query.

For example the following command lists all of the UrlAcls assigned to any URI for port 33333.

snippet: ps-urlacls

In this example any UrlAcl on port 33335 is removed

snippet: ps-removeurlacl

The following example shows how to add a UrlAcl for a ServiceControl service that should only respond to a specific DNS name. This would require an update of the ServiceControl configuration file as well. Refer to [setting a custom host name and port number](/servicecontrol/setting-custom-hostname.md)

snippet: ps-addurlacl
