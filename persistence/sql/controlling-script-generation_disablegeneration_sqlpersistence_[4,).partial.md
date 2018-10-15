## Disabling script generation

In version 4.2 and below, script generation can be disabled by removing the [NServiceBus.Persistence.Sql.MsBuild package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild) from the project.

In version 4.3 and above, the script generation capability isn't contained in a separate package. Execution of the script generation task can be completely disabled by including the following in the project file:

snippet: DisableScriptGeneration