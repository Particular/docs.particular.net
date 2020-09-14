Running the project will result in 3 console windows:

1. **Samples.CustomChecks.3rdPartySystem**: Represents the third-party system by simulating an HTTP service running on `http://localhost:57789`. At startup, the custom check is returning success, but the state can be toggled between success and failure by pressing <kbd>Enter</kbd>.
1. **Samples.CustomChecks.Monitor3rdParty**: The endpoint containing the custom check. The success or failure of the third-party system is continuously written to the console.
1. **PlatformLauncher**: Runs an in-process version of ServiceControl and ServicePulse. When the ServiceControl instance is ready, a browser window will be launched displaying the Custom Checks view in ServicePulse.
