

## Verifying that the sample works correctly

In this sample the information at the `Info` level is logged to the console window. 

There will be a few standard logging entries in the console window that are automatically created by NServiceBus when logging level is set to `Info`, for example `Queue [private$\error] is running with [Everyone] and [NT AUTHORITY\ANONYMOUS LOGON] permissions. Consider setting appropriate permissions, if required by the organization. For more information, consult the documentation.`. 

There will also be a custom entry logged from inside the handler `Hello from MyHandler`.