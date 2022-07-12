NServiceBus can be configured to use [Unity](https://github.com/unitycontainer/unity) for dependency injection.


## Default usage

snippet: Unity


## Using an existing container

snippet: Unity_Existing


## Breaking changes in Unity

Breaking changes in Unity will be handled as follows:

- Changes that require a change to the `NServiceBus.Unity` API (e.g. changes to `IUnityContainer`) will be released in a new major version of `NServiceBus.Unity`.
- Changes that do not require a change to the `NServiceBus.Unity` API will be released as a patch release to the latest minor version of `NServiceBus.Unity`.
- If Unity releases a new major version, it will be supported in a new minor release of `NServiceBus.Unity`.
