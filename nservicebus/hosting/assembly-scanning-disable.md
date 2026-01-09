---
title: Disable assembly scanning
summary: Completely disable assembly scanning and manually register all required types
reviewed: 2026-01-09
component: core
versions: '[10,]'
---

Assembly scanning can be completely disabled. When disabled, no assemblies are scanned, and the endpoint will not automatically discover message types, handlers, features, or installers.

snippet: DisableAssemblyScanning

> [!WARNING]
> When assembly scanning is disabled, all required types (message handlers, sagas, features, etc.) must be explicitly registered. Failure to register required types will cause the endpoint to fail at startup or behave incorrectly.

## Registering message handlers

Use `AddHandler<THandler>()` to register message handlers:

snippet: RegisterHandlerManually

## Registering sagas

Use `AddSaga<TSaga>()` to register sagas:

snippet: RegisterSagaManually

## Enabling features

Use `EnableFeature<TFeature>()` to enable features:

snippet: EnableFeatureManually

## Registering installers

Use `AddInstaller<TInstaller>()` to register installers:

snippet: RegisterInstallerManually

## Migration guidance

For detailed information about migrating from assembly scanning to manual registration, see [Non-assembly scanning mode](/nservicebus/hosting/non-assembly-scanning-mode.md).