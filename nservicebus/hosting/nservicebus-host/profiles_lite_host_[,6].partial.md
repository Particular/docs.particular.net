 * `InMemory` persistence is used for storing information like sagas, subscriptions, timeouts, etc. That is a convenient option for development, but should not be used in production.
 * The Lite profile by default turns on the `TimeoutManager`.
 * [Installers](/nservicebus/operations/installers.md) are always invoked when running the Lite profile.
 * Logging is output to the console.