## Critical error handling

A bridge can encounter a [critical error](/nservicebus/hosting/critical-errors.md) when a transport cannot recover from an infrastructure failure. Starting with version 5.1, `DefineCriticalErrorAction` can be used to stop the affected bridge endpoint and terminate the process:

snippet: bridge-critical-error-action

Configure the hosting environment to [restart the terminated process](/nservicebus/hosting/critical-errors.md#how-do-i-deal-with-persistent-critical-errors-terminate-and-restart-the-process). Calling `context.Stop` without terminating the process stops only the bridge endpoint that raised the critical error.
