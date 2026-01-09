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
> When assembly scanning is disabled, message handlers, sagas, features, and installers must be explicitly registered. Messages received without a registered handler will fail and be moved to the error queue.

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