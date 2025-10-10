## Troubleshooting via PowerShell

The ServiceControl Management PowerShell module offers some cmdlets to assist with troubleshooting the installation of ServiceControl instances.

### Check if a port is already in use

Before adding an instance of ServiceControl test if the port to use is currently in use.

snippet: ps-testport

This example shows the available ports out of a range of ports

snippet: ps-testportrange

If the port is already in use, then choose a different port.