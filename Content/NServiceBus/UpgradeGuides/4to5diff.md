---
title: API differences between V4 and V5
summary: API differences between V4 and V5 
tags:
- upgrade
- migration
- diff
---

### The following public types have been removed.

- `NServiceBus.Audit.MessageAuditer` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Audit/MessageAuditer.cs) ]
- `NServiceBus.AutomaticSubscriptions.AutoSubscriber` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscriber.cs) ]
- `NServiceBus.AutomaticSubscriptions.DefaultAutoSubscriptionStrategy` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/DefaultAutoSubscriptionStrategy.cs) ]
- `NServiceBus.Config.AddressInitializer` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/AddressInitializer.cs) ]
- `NServiceBus.Config.ChannelCollection` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/GatewayConfig.cs) ]
- `NServiceBus.Config.ChannelConfig` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/GatewayConfig.cs) ]
- `NServiceBus.Config.DisplayInfrastructureServicesStatus` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/InfrastructureServices.cs) ]
- `NServiceBus.Config.GatewayConfig` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/GatewayConfig.cs) ]
- `NServiceBus.Config.InfrastructureServices` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/InfrastructureServices.cs) ]
- `NServiceBus.Config.SiteCollection` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/GatewayConfig.cs) ]
- `NServiceBus.Config.SiteConfig` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/GatewayConfig.cs) ]
- `NServiceBus.Config.WindowsInstallerRunner` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/WindowsInstallerRunner.cs) ]
- `NServiceBus.ConfigureGateway` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/ConfigureGateway.cs) ]
- `NServiceBus.ConfigureSagas` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/ConfigureSagas.cs) ]
- `NServiceBus.ConfigureSecondLevelRetriesExtensions` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/Config/ConfigureSecondLevelRetriesExtensions.cs) ]
- `NServiceBus.DataBus.Config.Bootstrapper` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/DataBus/Config/Bootstrapper.cs) ]
- `NServiceBus.DataBus.DefaultDataBusSerializer` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/DataBus/DefaultDatabusSerializer.cs) ]
- `NServiceBus.DataBus.FileShare.FileShareDataBus` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/DataBus/FileShare/FileShareDataBus.cs) ]
- `NServiceBus.DataBus.InMemory.InMemoryDataBus` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/DataBus/InMemory/InMemoryDataBus.cs) ]
- `NServiceBus.Encryption.EncryptionMessageMutator` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Encryption/EncryptionMessageMutator.cs) ]
- `NServiceBus.Features.Categories.Serializers` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Serializers.cs) ]
- `NServiceBus.Features.EnableDefaultFeatures` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Support/EnableDefaultFeatures.cs) ]
- `NServiceBus.Features.Feature<T>` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs) ]
- `NServiceBus.Features.FeatureCategory` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs) ]
- `NServiceBus.Features.FeatureInitializer` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Support/FeatureInitializer.cs) ]
- `NServiceBus.Features.Gateway` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Gateway.cs) ]
- `NServiceBus.Features.MsmqTransport` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/Config/MsmqTransport.cs) ]
- `NServiceBus.FeatureSettingsExtensions` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/Config/FeatureSettingsExtensions.cs) ]
- `NServiceBus.Gateway.Channels.Channel` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Channels/Channel.cs) ]
- `NServiceBus.Gateway.Channels.ChannelFactory` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Channels/ChannelFactory.cs) ]
- `NServiceBus.Gateway.Channels.ChannelTypeAttribute` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Channels/ChannelTypeAttribute.cs) ]
- `NServiceBus.Gateway.Channels.DataReceivedOnChannelArgs` 
- `NServiceBus.Gateway.Channels.Http.DefaultResponder` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Channels/Http/DefaultResponder.cs) ]
- `NServiceBus.Gateway.Channels.Http.HttpChannelReceiver` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Channels/Http/HttpChannelReceiver.cs) ]
- `NServiceBus.Gateway.Channels.Http.HttpChannelSender` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Channels/Http/HttpChannelSender.cs) ]
- `NServiceBus.Gateway.Channels.Http.HttpHeaders` 
- `NServiceBus.Gateway.Channels.Http.IHttpResponder` 
- `NServiceBus.Gateway.Channels.Http.SetDefaultResponder` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Channels/Http/SetDefaultResponder.cs) ]
- `NServiceBus.Gateway.Channels.IChannelFactory` 
- `NServiceBus.Gateway.Channels.IChannelReceiver` 
- `NServiceBus.Gateway.Channels.IChannelSender` 
- `NServiceBus.Gateway.Channels.ReceiveChannel` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Channels/Channel.cs) ]
- `NServiceBus.Gateway.Deduplication.GatewayMessage` 
- `NServiceBus.Gateway.Deduplication.InMemoryDeduplication` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/InMemory/Gateway/InMemoryDeduplication.cs) ]
- `NServiceBus.Gateway.Deduplication.RavenDBDeduplication` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/Gateway/RavenDBDeduplication.cs) ]
- `NServiceBus.Gateway.DefaultInputAddress` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/DefaultInputAddress.cs) ]
- `NServiceBus.Gateway.HeaderManagement.DataBusHeaderManager` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/HeaderManagement/DataBusHeaderManager.cs) ]
- `NServiceBus.Gateway.HeaderManagement.GatewayHeaderManager` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/HeaderManagement/GatewayHeaderManager.cs) ]
- `NServiceBus.Gateway.HeaderManagement.GatewayHeaders` 
- `NServiceBus.Gateway.HeaderManagement.HeaderMapper` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/HeaderManagement/HeaderMapper.cs) ]
- `NServiceBus.Gateway.Notifications.IMessageNotifier` 
- `NServiceBus.Gateway.Notifications.INotifyAboutMessages` 
- `NServiceBus.Gateway.Notifications.MessageNotifier` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Notifications/MessageNotifier.cs) ]
- `NServiceBus.Gateway.Notifications.MessageReceivedOnChannelArgs` 
- `NServiceBus.Gateway.Persistence.InMemoryPersistence` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/InMemory/Gateway/InMemoryPersistence.cs) ]
- `NServiceBus.Gateway.Persistence.MessageInfo` 
- `NServiceBus.Gateway.Persistence.Raven.GatewayMessage` 
- `NServiceBus.Gateway.Persistence.Raven.RavenDbPersistence` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/Gateway/RavenDBPersistence.cs) ]
- `NServiceBus.Gateway.Receiving.ChannelException` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Receiving/ChannelException.cs) ]
- `NServiceBus.Gateway.Receiving.ConfigurationBasedChannelManager` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Receiving/ConfigurationBasedChannelManager.cs) ]
- `NServiceBus.Gateway.Receiving.ConventionBasedChannelManager` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Receiving/ConventionBasedChannelManager.cs) ]
- `NServiceBus.Gateway.Receiving.GatewayReceiver` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Receiving/GatewayReceiver.cs) ]
- `NServiceBus.Gateway.Receiving.IManageReceiveChannels` 
- `NServiceBus.Gateway.Receiving.IReceiveMessagesFromSites` 
- `NServiceBus.Gateway.Receiving.SingleCallChannelReceiver` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Receiving/SingleCallChannelReceiver.cs) ]
- `NServiceBus.Gateway.Routing.Endpoints.DefaultEndpointRouter` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Routing/Endpoints/DefaultEndpointRouter.cs) ]
- `NServiceBus.Gateway.Routing.IRouteMessagesToEndpoints` 
- `NServiceBus.Gateway.Routing.IRouteMessagesToSites` 
- `NServiceBus.Gateway.Routing.Site` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Routing/Site.cs) ]
- `NServiceBus.Gateway.Routing.Sites.ConfigurationBasedSiteRouter` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Routing/Sites/ConfigurationBasedSiteRouter.cs) ]
- `NServiceBus.Gateway.Routing.Sites.KeyPrefixConventionSiteRouter` 
- `NServiceBus.Gateway.Routing.Sites.OriginatingSiteHeaderRouter` 
- `NServiceBus.Gateway.Sending.CallInfo` 
- `NServiceBus.Gateway.Sending.CallType` 
- `NServiceBus.Gateway.Sending.GatewaySender` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Sending/GatewaySender.cs) ]
- `NServiceBus.Gateway.Sending.IForwardMessagesToSites` 
- `NServiceBus.Gateway.Sending.SingleCallChannelForwarder` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Sending/SingleCallChannelForwarder.cs) ]
- `NServiceBus.Gateway.Utils.Hasher` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Gateway/Utils/Hasher.cs) ]
- `NServiceBus.Hosting.Configuration.ConfigManager` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Configuration/ConfigurationManager.cs) ]
- `NServiceBus.Hosting.Profiles.ProfileActivator` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Profiles/ProfileActivator.cs) ]
- `NServiceBus.Hosting.Roles.Handlers.TransportRoleHandler` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Roles/Handlers/TransportRoleHandler.cs) ]
- `NServiceBus.Hosting.Roles.IConfigureRole` 
- `NServiceBus.Hosting.Roles.IConfigureRole<T>` 
- `NServiceBus.Hosting.Roles.IRole` 
- `NServiceBus.Hosting.Roles.RoleManager` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Roles/RoleManager.cs) ]
- `NServiceBus.Hosting.Windows.Roles.Handlers.ClientRoleHandler` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Roles/Handlers/ClientRoleHandler.cs) ]
- `NServiceBus.Hosting.Windows.Roles.Handlers.PublisherRoleHandler` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Roles/Handlers/PublisherRoleHandler.cs) ]
- `NServiceBus.Hosting.Windows.Roles.Handlers.ServerRoleHandler` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Roles/Handlers/ServerRoleHandler.cs) ]
- `NServiceBus.Installation.GatewayHttpListenerInstaller` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Installation/GatewayHttpListenerInstaller.cs) ]
- `NServiceBus.Installation.PerformanceMonitorUsersInstaller` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Installation/PerformanceMonitorUsersInstaller.cs) ]
- `NServiceBus.Management.Retries.SecondLevelRetries` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs) ]
- `NServiceBus.MessageConventionExtensions` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/MessageConventionExtensions.cs) ]
- `NServiceBus.Msmq` 
- `NServiceBus.ObjectBuilder.Common.Config.ConfigureCommon` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/ObjectBuilder/Common/Config/ConfigureCommon.cs) ]
- `NServiceBus.ObjectBuilder.Common.SynchronizedInvoker` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/ObjectBuilder/Common/SynchronizedInvoker.cs) ]
- `NServiceBus.Persistence.InMemory.InMemoryPersistence` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/InMemory/InMemoryPersistence.cs) ]
- `NServiceBus.Persistence.InMemory.SagaPersister.InMemorySagaPersister` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/InMemory/SagaPersister/InMemorySagaPersister.cs) ]
- `NServiceBus.Persistence.InMemory.SubscriptionStorage.InMemorySubscriptionStorage` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/InMemory/SubscriptionStorage/InMemorySubscriptionStorage.cs) ]
- `NServiceBus.Persistence.InMemory.TimeoutPersister.InMemoryTimeoutPersistence` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/InMemory/TimeoutPersister/InMemoryTimeoutPersistence.cs) ]
- `NServiceBus.Persistence.Msmq.SubscriptionStorage.Config.SubscriptionsQueueCreator` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Msmq/SubscriptionStorage/Config/SubscriptionsQueueCreator.cs) ]
- `NServiceBus.Persistence.Msmq.SubscriptionStorage.MsmqSubscriptionStorage` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Msmq/SubscriptionStorage/MsmqSubscriptionStorage.cs) ]
- `NServiceBus.Persistence.Raven.RavenConventions` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/RavenConventions.cs) ]
- `NServiceBus.Persistence.Raven.RavenPersistenceConstants` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/RavenPersistenceConstants.cs) ]
- `NServiceBus.Persistence.Raven.RavenSessionFactory` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/RavenSessionFactory.cs) ]
- `NServiceBus.Persistence.Raven.RavenUnitOfWork` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/RavenUnitofWork.cs) ]
- `NServiceBus.Persistence.Raven.RavenUserInstaller` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/RavenUserInstaller.cs) ]
- `NServiceBus.Persistence.Raven.SagaPersister.RavenSagaPersister` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/SagaPersister/RavenSagaPersister.cs) ]
- `NServiceBus.Persistence.Raven.SagaPersister.SagaUniqueIdentity` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/SagaPersister/RavenSagaPersister.cs) ]
- `NServiceBus.Persistence.Raven.StoreAccessor` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/StoreAccessor.cs) ]
- `NServiceBus.Persistence.Raven.SubscriptionStorage.MessageTypeConverter` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/SubscriptionStorage/Subscription.cs) ]
- `NServiceBus.Persistence.Raven.SubscriptionStorage.RavenSubscriptionStorage` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/SubscriptionStorage/RavenSubscriptionStorage.cs) ]
- `NServiceBus.Persistence.Raven.SubscriptionStorage.Subscription` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/SubscriptionStorage/Subscription.cs) ]
- `NServiceBus.Persistence.Raven.TimeoutPersister.RavenTimeoutPersistence` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/Raven/TimeoutPersister/RavenTimeoutPersistence.cs) ]
- `NServiceBus.Persistence.SetupDefaultPersistence` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Persistence/SetupDefaultPersistence.cs) ]
- `NServiceBus.Sagas.ConfigureTimeoutAsSystemMessages` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/ConfigureTimeoutAsSystemMessages.cs) ]
- `NServiceBus.Scheduling.Configuration.ConfigureScheduledTaskAsSystemMessages` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Scheduling/Configuration/ConfigureScheduledTaskAsSystemMessages.cs) ]
- `NServiceBus.Scheduling.Configuration.SchedulerConfiguration` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Scheduling/Configuration/SchedulerConfiguration.cs) ]
- `NServiceBus.Scheduling.InMemoryScheduledTaskStorage` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Scheduling/InMemoryScheduledTaskStorage.cs) ]
- `NServiceBus.Scheduling.IScheduledTaskStorage` 
- `NServiceBus.Scheduling.IScheduler` 
- `NServiceBus.Settings.Endpoint` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/Endpoint.cs) ]
- `NServiceBus.Settings.ISetDefaultSettings` 
- `NServiceBus.Settings.TransportSettings` 
- `NServiceBus.SyncConfig` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SyncConfig.cs) ]
- `NServiceBus.Timeout.Core.TimeoutManagerDefaults` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/Core/TimeoutManagerDefaults.cs) ]
- `NServiceBus.Transports.ConfigureTransport<T>` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/ConfigureTransport.cs) ]
- `NServiceBus.Transports.IConfigureTransport` 
- `NServiceBus.Transports.IConfigureTransport<T>` 
- `NServiceBus.Transports.Msmq.Config.CheckMachineNameForComplianceWithDtcLimitation` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/Config/CheckMachineNameForComplianceWithDTCLimitation.cs) ]
- `NServiceBus.Unicast.BusAsyncResultEventArgs` 
- `NServiceBus.Unicast.Config.DefaultToTimeoutManagerBasedDeferal` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Config/DefaultToTimeoutManagerBasedDeferal.cs) ]
- `NServiceBus.Unicast.Config.DefaultTransportForHost` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Config/DefaultTransportForHost.cs) ]
- `NServiceBus.Unicast.DefaultDispatcherFactory` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/DefaultDispatcherFactory.cs) ]
- `NServiceBus.Unicast.HandlerInvocationCache` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/HandlerInvocationCache.cs) ]
- `NServiceBus.Unicast.Monitoring.CriticalTimeCalculator` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Monitoring/CriticalTimeCalculator.cs) ]
- `NServiceBus.Unicast.Monitoring.EstimatedTimeToSLABreachCalculator` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Monitoring/EstimatedTimeToSLABreachCalculator.cs) ]
- `NServiceBus.Unicast.Monitoring.PerformanceCounterInitializer` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Monitoring/PerformanceCounterInitializer.cs) ]
- `NServiceBus.Unicast.Monitoring.ProcessingStatistics` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Monitoring/ProcessingStatistics.cs) ]
- `NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.EnableMessageDrivenPublisherIfStorageIsFound` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/EnableMessageDrivenPublisherIfStorageIsFound.cs) ]
- `NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.MessageDrivenSubscriptionManager` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/MessageDrivenSubscriptionManager.cs) ]
- `NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.SubcriberSideFiltering.FilteringMutator` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/SubcriberSideFiltering/FilteringMutator.cs) ]
- `NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.SubcriberSideFiltering.SubscriptionPredicatesEvaluator` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/SubcriberSideFiltering/SubscriptionPredicatesEvaluator.cs) ]
- `NServiceBus.Unicast.Transport.Config.Bootstrapper` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/Config/Bootstrapper.cs) ]
- `NServiceBus.Unicast.Transport.Transactional.Config.AdvancedTransactionalConfig` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/Config/AdvancedTransactionalConfig.cs) ]


### The following public types have been made internal.

- `NDesk.Options.Option` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Options.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Options.cs) ]
- `NDesk.Options.OptionAction<TKey, TValue>` 
- `NDesk.Options.OptionContext` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Options.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Options.cs) ]
- `NDesk.Options.OptionException` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Options.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Options.cs) ]
- `NDesk.Options.OptionSet` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Options.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Options.cs) ]
- `NDesk.Options.OptionValueCollection` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Options.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Options.cs) ]
- `NDesk.Options.OptionValueType` 
- `NServiceBus.CircuitBreakers.CircuitBreaker` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/CircuitBreakers/CircuitBreaker.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/CircuitBreakers/CircuitBreaker.cs) ]
- `NServiceBus.Config.Conventions.EndpointHelper` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/Conventions/EndpointHelper.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Config/Conventions/EndpointHelper.cs) ]
- `NServiceBus.Config.SatelliteConfigurer` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Config/SatelliteConfigurer.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Config/SatelliteConfigurer.cs) ]
- `NServiceBus.Faults.Forwarder.Config.FaultsQueueCreator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Faults/Forwarder/Config/FaultsQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Faults/Forwarder/Config/FaultsQueueCreator.cs) ]
- `NServiceBus.Faults.Forwarder.FaultManager` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Faults/Forwarder/FaultManager.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Faults/Forwarder/FaultManager.cs) ]
- `NServiceBus.Faults.InMemory.FaultManager` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Faults/InMemory/FaultManager.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Faults/InMemory/FaultManager.cs) ]
- `NServiceBus.Hosting.Profiles.ProfileManager` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Profiles/ProfileManager.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Profiles/ProfileManager.cs) ]
- `NServiceBus.Hosting.Wcf.WcfManager` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Wcf/WcfManager.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Wcf/WcfManager.cs) ]
- `NServiceBus.Hosting.Wcf.WcfServiceHost` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Wcf/WcfServiceHost.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Wcf/WcfServiceHost.cs) ]
- `NServiceBus.Hosting.Windows.Arguments.HostArguments` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Arguments/HostArguments.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Arguments/HostArguments.cs) ]
- `NServiceBus.Hosting.Windows.EndpointType` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/EndpointType.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/EndpointType.cs) ]
- `NServiceBus.Hosting.Windows.EndpointTypeDeterminer` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/EndpointTypeDeterminer.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/EndpointTypeDeterminer.cs) ]
- `NServiceBus.Hosting.Windows.HostServiceLocator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/HostServiceLocator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/HostServiceLocator.cs) ]
- `NServiceBus.Hosting.Windows.Installers.WindowsInstaller` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Installers/WindowsInstaller.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Installers/WindowsInstaller.cs) ]
- `NServiceBus.Hosting.Windows.LoggingHandlers.IntegrationLoggingHandler` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/LoggingHandlers/IntegrationLoggingHandler.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/LoggingHandlers/IntegrationLoggingHandler.cs) ]
- `NServiceBus.Hosting.Windows.LoggingHandlers.LiteLoggingHandler` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/LoggingHandlers/LiteLoggingHandler.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/LoggingHandlers/LiteLoggingHandler.cs) ]
- `NServiceBus.Hosting.Windows.LoggingHandlers.ProductionLoggingHandler` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/LoggingHandlers/ProductionLoggingHandler.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/LoggingHandlers/ProductionLoggingHandler.cs) ]
- `NServiceBus.Hosting.Windows.Profiles.Handlers.PerformanceCountersProfileHandler` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/Profiles/Handlers/PerformanceCountersProfileHandler.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Profiles/Handlers/PerformanceCountersProfileHandler.cs) ]
- `NServiceBus.Impersonation.Windows.WindowsIdentityEnricher` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Impersonation/Windows/WindowsIdentityEnricher.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Impersonation/Windows/WindowsIdentityEnricher.cs) ]
- `NServiceBus.ObjectBuilder.Common.CommonObjectBuilder` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/ObjectBuilder/Common/CommonObjectBuilder.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ObjectBuilder/Common/CommonObjectBuilder.cs) ]
- `NServiceBus.Persistence.Msmq.SubscriptionStorage.Entry` 
- `NServiceBus.Sagas.ConfigureHowToFindSagaWithMessageDispatcher` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/ConfigureHowToFindSagaWithMessageDispatcher.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Sagas/ConfigureHowToFindSagaWithMessageDispatcher.cs) ]
- `NServiceBus.Sagas.Finders.HeaderSagaIdFinder<T>` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Finders/HeaderSagaIdFinder.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Sagas/Finders/HeaderSagaIdFinder.cs) ]
- `NServiceBus.Sagas.Finders.PropertySagaFinder<TSagaData, TMessage>` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Finders/PropertySagaFinder.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Sagas/Finders/PropertySagaFinder.cs) ]
- `NServiceBus.Satellites.Config.SatelliteContext` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Satellites/Config/SatelliteContext.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Satellites/Config/SatelliteContext.cs) ]
- `NServiceBus.Satellites.SatelliteLauncher` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Satellites/SatelliteLauncher.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Satellites/SatelliteLauncher.cs) ]
- `NServiceBus.Satellites.SatellitesQueuesCreator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Satellites/SatellitesQueuesCreator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Satellites/SatellitesQueuesCreator.cs) ]
- `NServiceBus.Scheduling.DefaultScheduler` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Scheduling/DefaultScheduler.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Scheduling/DefaultScheduler.cs) ]
- `NServiceBus.Scheduling.Messages.ScheduledTask` 
- `NServiceBus.Scheduling.ScheduledTaskMessageHandler` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Scheduling/ScheduledTaskMessageHandler.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Scheduling/ScheduledTaskMessageHandler.cs) ]
- `NServiceBus.SecondLevelRetries.Helpers.SecondLevelRetriesHeaders` 
- `NServiceBus.SecondLevelRetries.SecondLevelRetriesProcessor` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetriesProcessor.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetriesProcessor.cs) ]
- `NServiceBus.Serializers.Json.Internal.MessageContractResolver` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/Internal/MessageContractResolver.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Internal/MessageContractResolver.cs) ]
- `NServiceBus.Serializers.Json.Internal.MessageSerializationBinder` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/Internal/MessageSerializationBinder.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Internal/MessageSerializationBinder.cs) ]
- `NServiceBus.Serializers.Json.Internal.XContainerConverter` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/Internal/XContainerConverter.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Internal/XContainerConverter.cs) ]
- `NServiceBus.Serializers.XML.Config.MessageTypesInitializer` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/Config/MessageTypesInitializer.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/MessageTypesInitializer.cs) ]
- `NServiceBus.Serializers.XML.XmlSanitizingStream` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/XmlSanitizingStream.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/XmlSanitizingStream.cs) ]
- `NServiceBus.ServiceAsyncResult` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/ServiceAsyncResult.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/Wcf/ServiceAsyncResult.cs) ]
- `NServiceBus.Timeout.Core.DefaultTimeoutManager` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/Core/DefaultTimeoutManager.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Timeout/Core/DefaultTimeoutManager.cs) ]
- `NServiceBus.Timeout.Hosting.Windows.TimeoutDispatcherProcessor` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutDispatcherProcessor.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutDispatcherProcessor.cs) ]
- `NServiceBus.Timeout.Hosting.Windows.TimeoutMessageProcessor` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutMessageProcessor.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutMessageProcessor.cs) ]
- `NServiceBus.Timeout.Hosting.Windows.TimeoutPersisterReceiver` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutPersisterReceiver.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutPersisterReceiver.cs) ]
- `NServiceBus.Timeout.TimeoutManagerDeferrer` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/Core/TimeoutManagerDeferrer.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Timeout/Core/TimeoutManagerDeferrer.cs) ]
- `NServiceBus.Timeout.TimeoutManagerHeaders` 
- `NServiceBus.Transports.Msmq.MsmqQueueCreator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/MsmqQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/Msmq/MsmqQueueCreator.cs) ]
- `NServiceBus.Unicast.MessagingBestPractices` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/MessagingBestPractices.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/MessagingBestPractices.cs) ]
- `NServiceBus.Unicast.Monitoring.CausationMutator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Monitoring/CausationMutator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Audit/CausationMutator.cs) ]
- `NServiceBus.Unicast.Publishing.StorageDrivenPublisher` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Publishing/StorageDrivenPublisher.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Publishing/StorageDrivenPublisher.cs) ]
- `NServiceBus.Unicast.Queuing.Installers.AuditQueueCreator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Queuing/Installers/AuditQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Queuing/Installers/AuditQueueCreator.cs) ]
- `NServiceBus.Unicast.Queuing.Installers.EndpointInputQueueCreator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Queuing/Installers/EndpointInputQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Queuing/Installers/EndpointInputQueueCreator.cs) ]
- `NServiceBus.Unicast.Queuing.Installers.ForwardReceivedMessagesToQueueCreator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Queuing/Installers/ForwardReceivedMessagesToQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Queuing/Installers/ForwardReceivedMessagesToQueueCreator.cs) ]
- `NServiceBus.Unicast.Queuing.QueuesCreator` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Queuing/QueuesCreator.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Queuing/QueuesCreator.cs) ]
- `NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.NoopSubscriptionAuthorizer` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/NoopSubscriptionAuthorizer.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/NoopSubscriptionAuthorizer.cs) ]
- `NServiceBus.Unicast.Transport.TransportConnectionString` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportConnectionString.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/TransportConnectionString.cs) ]
- `NServiceBus.Unicast.Transport.TransportMessageExtensions` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/ControlMessage.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/TransportMessageExtensions.cs) ]


### The following types have differences.


#### NServiceBus.Address  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Address.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Address.cs) ]

##### Methods Removed

  - `NServiceBus.Address get_PublicReturnAddress()` 
  - `void PreventChanges()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Address.cs#L191) ]


#### NServiceBus.AutomaticSubscriptions.Config.AutoSubscribeSettings  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs) ]

##### Methods Removed

  - `void .ctor()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs#L8) ]
  - `NServiceBus.AutomaticSubscriptions.Config.AutoSubscribeSettings AutoSubscribePlainMessages()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs#L36) ]
  - `NServiceBus.AutomaticSubscriptions.Config.AutoSubscribeSettings CustomAutoSubscriptionStrategy<T>()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs#L47) ]
  - `NServiceBus.AutomaticSubscriptions.Config.AutoSubscribeSettings DoNotAutoSubscribeSagas()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs#L18) ]
  - `NServiceBus.AutomaticSubscriptions.Config.AutoSubscribeSettings DoNotRequireExplicitRouting()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs#L27) ]


#### NServiceBus.AutoSubscribeSettingsExtensions  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettingsExtensions.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettingsExtensions.cs) ]

##### Methods Removed

  - `NServiceBus.Features.FeatureSettings AutoSubscribe(NServiceBus.Features.FeatureSettings, Action<NServiceBus.AutomaticSubscriptions.Config.AutoSubscribeSettings>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettingsExtensions.cs#L11) ]


#### NServiceBus.Config.IWantToRunWhenConfigurationIsComplete  

##### Methods Removed

  - `void Run()` 


#### NServiceBus.Configure  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure.cs) ]

##### Fields Removed

  - `Func<string> DefineEndpointVersionRetriever`

##### Methods changed to non-public

  - `void Initialize()` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs#L337) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure.cs#L102) ]
  - `void set_Builder(NServiceBus.ObjectBuilder.IBuilder)` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs#L75) ]

##### Methods Removed

  - `void add_ConfigurationComplete(Action)` 
  - `void ForAllTypes<T>(Action<Type>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs#L379) ]
  - `NServiceBus.Config.ConfigurationSource.IConfigurationSource get_ConfigurationSource()` 
  - `NServiceBus.Settings.Endpoint get_Endpoint()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs#L161) ]
  - `bool get_SendOnlyMode()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs#L191) ]
  - `NServiceBus.Settings.TransportSettings get_Transports()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs#L167) ]
  - `IEnumerable<Assembly> GetAssembliesInDirectory(string, String[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs#L417) ]
  - `void remove_ConfigurationComplete(Action)` 
  - `void ScaleOut(Action<NServiceBus.Settings.ScaleOutSettings>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Configure.cs#L184) ]
  - `void set_ConfigurationSource(NServiceBus.Config.ConfigurationSource.IConfigurationSource)` 


#### NServiceBus.ConfigureCriticalErrorAction  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/ConfigureCriticalErrorAction.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/CriticalError/ConfigureCriticalErrorAction_Obsolete.cs) ]

##### Methods Removed

  - `void RaiseCriticalError(NServiceBus.Configure, string, Exception)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/ConfigureCriticalErrorAction.cs#L44) ]


#### NServiceBus.EndpointSLAAttribute  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Configuration/EndpointSLAAttribute.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Monitoring/SLA/EndpointSLAAttribute.cs) ]

##### Methods Removed

  - `string get_SLA()` 
  - `void set_SLA(string)` 


#### NServiceBus.ExtensionMethods  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/ExtensionMethods.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ExtensionMethods.cs) ]

##### Fields Removed

  - `Func<object, string, string> GetHeaderAction`
  - `Action<object, string, string> SetHeaderAction`

##### Methods Removed

  - `NServiceBus.IBus get_Bus()` 
  - `Func<IDictionary<string, string>> get_GetStaticOutgoingHeadersAction()` 
  - `NServiceBus.IMessageCreator get_MessageCreator()` 
  - `void set_Bus(NServiceBus.IBus)` 
  - `void set_GetStaticOutgoingHeadersAction(Func<IDictionary<string, string>>)` 
  - `void set_MessageCreator(NServiceBus.IMessageCreator)` 
  - `void SetMessageHeader(NServiceBus.IBus, object, string, string)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/ExtensionMethods.cs#L65) ]


#### NServiceBus.Features.Audit  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Audit/Audit.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Audit/Audit.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Audit/Audit.cs#L15) ]

##### Methods Removed

  - `bool get_IsEnabledByDefault()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Audit/Audit.cs#L41) ]
  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Audit/Audit.cs#L16) ]


#### NServiceBus.Features.AutoSubscribe  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscribe.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscribe.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscribe.cs#L13) ]

##### Methods Removed

  - `bool get_IsEnabledByDefault()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscribe.cs#L34) ]
  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscribe.cs#L12) ]


#### NServiceBus.Features.BinarySerialization  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Binary/Config/BinarySerialization.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Binary/Config/BinarySerialization.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Binary/Config/BinarySerialization.cs#L11) ]

##### Methods Removed

  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Binary/Config/BinarySerialization.cs#L9) ]


#### NServiceBus.Features.BsonSerialization  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/Config/BsonSerialization.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/BsonSerialization.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/BsonSerialization.cs#L12) ]

##### Methods Removed

  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/Config/BsonSerialization.cs#L10) ]


#### NServiceBus.Features.Feature  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Features/Feature.cs) ]

##### Methods changed to non-public

  - `void EnableByDefault<T>()` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L75) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Features/Feature.cs#L97) ]

##### Methods Removed

  - `IEnumerable<NServiceBus.Features.Feature> ByCategory(NServiceBus.Features.FeatureCategory)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L142) ]
  - `void Disable<T>()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L93) ]
  - `void Disable(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L101) ]
  - `void DisableByDefault(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L109) ]
  - `void Enable<T>()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L59) ]
  - `void Enable(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L67) ]
  - `void EnableByDefault(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L83) ]
  - `bool Equals(object)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L179) ]
  - `NServiceBus.Features.FeatureCategory get_Category()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L134) ]
  - `bool get_Enabled()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L50) ]
  - `int GetHashCode()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L196) ]
  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L19) ]
  - `bool IsEnabled<T>()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L117) ]
  - `bool IsEnabled(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L126) ]
  - `bool op_Equality(NServiceBus.Features.Feature, NServiceBus.Features.Feature)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L202) ]
  - `bool op_Inequality(NServiceBus.Features.Feature, NServiceBus.Features.Feature)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L212) ]
  - `bool ShouldBeEnabled()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Features/Feature.cs#L26) ]


#### NServiceBus.Features.JsonSerialization  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/Config/JsonSerialization.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/JsonSerialization.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/JsonSerialization.cs#L13) ]

##### Methods Removed

  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/Config/JsonSerialization.cs#L11) ]


#### NServiceBus.Features.MessageDrivenSubscriptions  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/MessageDrivenSubscriptions.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/MessageDrivenSubscriptions.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/MessageDrivenSubscriptions.cs#L11) ]

##### Methods Removed

  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/MessageDrivenSubscriptions.cs#L10) ]


#### NServiceBus.Features.Sagas  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Sagas/Sagas.cs) ]

##### Fields Removed

  - `IDictionary<Type, IDictionary<Type, KeyValuePair<PropertyInfo, PropertyInfo>>> SagaEntityToMessageToPropertyLookup`

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Sagas/Sagas.cs#L281) ]
  - `bool IsSagaType(Type)` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L282) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Sagas/Sagas.cs#L221) ]

##### Methods Removed

  - `void Clear()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L460) ]
  - `void ConfigureFinder(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L331) ]
  - `void ConfigureSaga(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L302) ]
  - `bool FindAndConfigureSagasIn(IEnumerable<Type>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L37) ]
  - `MethodInfo GetFindByMethodForFinder(NServiceBus.Saga.IFinder, object)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L194) ]
  - `IEnumerable<Type> GetFindersForMessageAndEntity(Type, Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L244) ]
  - `IEnumerable<Type> GetSagaDataTypes()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L277) ]
  - `Type GetSagaEntityTypeForSagaType(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L183) ]
  - `Type GetSagaTypeForSagaEntityType(Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L172) ]
  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L17) ]
  - `bool ShouldMessageStartSaga(Type, Type)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Sagas/Sagas.cs#L120) ]


#### NServiceBus.Features.SecondLevelRetries  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs#L13) ]

##### Methods Removed

  - `bool get_IsEnabledByDefault()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs#L50) ]
  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs#L56) ]
  - `bool ShouldBeEnabled()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs#L31) ]


#### NServiceBus.Features.TimeoutManager  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/TimeoutManager.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Timeout/TimeoutManager.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Timeout/TimeoutManager.cs#L13) ]

##### Methods Removed

  - `NServiceBus.Address get_DispatcherAddress()` 
  - `NServiceBus.Address get_InputAddress()` 
  - `bool get_IsEnabledByDefault()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/TimeoutManager.cs#L17) ]
  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/TimeoutManager.cs#L42) ]
  - `bool ShouldBeEnabled()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/TimeoutManager.cs#L24) ]


#### NServiceBus.Features.XmlSerialization  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/Config/XmlSerialization.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerialization.cs) ]

##### Methods changed to non-public

  - `void .ctor()` [ [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerialization.cs#L12) ]

##### Methods Removed

  - `void Initialize()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/Config/XmlSerialization.cs#L11) ]


#### NServiceBus.Hosting.Helpers.SkippedFile  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Helpers/SkippedFile.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Hosting/Helpers/SkippedFile.cs) ]

##### Methods changed to non-public

  - `void .ctor(string, string)` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Hosting/Helpers/SkippedFile.cs#L9) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Hosting/Helpers/SkippedFile.cs#L9) ]


#### NServiceBus.Hosting.Profiles.IHandleProfile  

##### Methods Removed

  - `void ProfileActivated()` 


#### NServiceBus.Hosting.Windows.WindowsHost  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/WindowsHost.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Hosting.Windows/WindowsHost.cs) ]

##### Methods Removed

  - `void .ctor(Type, String[], string, bool, IEnumerable<string>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/WindowsHost.cs#L26) ]
  - `void Install(string)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Hosting.Windows/WindowsHost.cs#L75) ]


#### NServiceBus.IBus  

##### Methods Removed

  - `IDictionary<string, string> get_OutgoingHeaders()` 
  - `void Publish<T>(T)` 
  - `void Publish<T>()` 
  - `void Publish<T>(Action<T>)` 
  - `NServiceBus.ICallback Send(object)` 
  - `NServiceBus.ICallback Send<T>(Action<T>)` 
  - `NServiceBus.ICallback Send(string, object)` 
  - `NServiceBus.ICallback Send(NServiceBus.Address, object)` 
  - `NServiceBus.ICallback Send<T>(string, Action<T>)` 
  - `NServiceBus.ICallback Send<T>(NServiceBus.Address, Action<T>)` 
  - `NServiceBus.ICallback Send(string, string, object)` 
  - `NServiceBus.ICallback Send(NServiceBus.Address, string, object)` 
  - `NServiceBus.ICallback Send<T>(string, string, Action<T>)` 
  - `NServiceBus.ICallback Send<T>(NServiceBus.Address, string, Action<T>)` 
  - `NServiceBus.ICallback SendToSites(IEnumerable<string>, object)` 
  - `void Subscribe(Type, Predicate<object>)` 


#### NServiceBus.IMessageContext  

##### Methods Removed

  - `DateTime get_TimeSent()` 


#### NServiceBus.INeedInitialization  

##### Methods Removed

  - `void Init()` 


#### NServiceBus.Installation.INeedToInstallSomething  

##### Methods Removed

  - `void Install(string)` 


#### NServiceBus.IStartableBus  

##### Methods Removed

  - `void add_Started(EventHandler)` 
  - `void remove_Started(EventHandler)` 
  - `void Shutdown()` 
  - `NServiceBus.IBus Start(Action)` 


#### NServiceBus.IWantToRunBeforeConfigurationIsFinalized  

##### Methods Removed

  - `void Run()` 


#### NServiceBus.Logging.LogManager  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Logging/LogManager.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/LogManager.cs) ]

##### Methods Removed

  - `void .ctor()` 
  - `NServiceBus.Logging.ILoggerFactory get_LoggerFactory()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Logging/LogManager.cs#L12) ]
  - `void set_LoggerFactory(NServiceBus.Logging.ILoggerFactory)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Logging/LogManager.cs#L15) ]


#### NServiceBus.MessageIntentEnum  

##### Fields Removed

  - `NServiceBus.MessageIntentEnum Init`


#### NServiceBus.MessageInterfaces.MessageMapper.Reflection.MessageMapper  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs) ]

##### Methods changed to non-public

  - `Type CreateTypeFrom(Type, ModuleBuilder)` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L150) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L156) ]
  - `string GetNewTypeName(Type)` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L141) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L147) ]
  - `void InitType(Type, ModuleBuilder)` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L51) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L51) ]


#### NServiceBus.MessageMutator.IMutateOutgoingTransportMessages  

##### Methods Removed

  - `void MutateOutgoing(Object[], NServiceBus.TransportMessage)` 


#### NServiceBus.ObjectBuilder.IConfigureComponents  

##### Methods Removed

  - `NServiceBus.ObjectBuilder.IConfigureComponents RegisterSingleton<T>(object)` 


#### NServiceBus.Saga.ISagaPersister  

##### Methods Removed

  - `T Get<T>(Guid)` 
  - `T Get<T>(string, object)` 


#### NServiceBus.Saga.Saga&lt;TSagaData&gt;  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Saga/Saga.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Saga/SagaT.cs) ]

##### Methods Removed

  - `void ConfigureHowToFindSaga()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Saga/Saga.cs#L46) ]
  - `NServiceBus.IBus get_Bus()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Saga/Saga.cs#L91) ]
  - `bool get_Completed()` 
  - `T get_Data()` 
  - `NServiceBus.Saga.IContainSagaData get_Entity()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Saga/Saga.cs#L27) ]
  - `NServiceBus.Saga.IHandleReplyingToNullOriginator get_HandleReplyingToNullOriginator()` 
  - `NServiceBus.Saga.IConfigureHowToFindSagaWithMessage get_SagaMessageFindingConfiguration()` 
  - `void set_Bus(NServiceBus.IBus)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Saga/Saga.cs#L97) ]
  - `void set_Data(T)` 
  - `void set_Entity(NServiceBus.Saga.IContainSagaData)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Saga/Saga.cs#L28) ]
  - `void set_HandleReplyingToNullOriginator(NServiceBus.Saga.IHandleReplyingToNullOriginator)` 
  - `void set_SagaMessageFindingConfiguration(NServiceBus.Saga.IConfigureHowToFindSagaWithMessage)` 


#### NServiceBus.Saga.ToSagaExpression&lt;TSagaData, TMessage&gt;  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Saga/ToSagaExpression.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Saga/ToSagaExpression.cs) ]

##### Methods Removed

  - `void ToSaga(Expression<Func<TSaga, object>>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus/Saga/ToSagaExpression.cs#L30) ]


#### NServiceBus.SecondLevelRetries.Config.SecondLevelRetriesSettings  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/Config/SecondLevelRetriesSettings.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Config/SecondLevelRetriesSettings.cs) ]

##### Methods Removed

  - `void .ctor()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/SecondLevelRetries/Config/SecondLevelRetriesSettings.cs#L8) ]


#### NServiceBus.Serialization.IMessageSerializer  

##### Methods Removed

  - `void Serialize(Object[], Stream)` 


#### NServiceBus.Serializers.Binary.BinaryMessageSerializer  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Binary/BinaryMessageSerializer.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Binary/BinaryMessageSerializer.cs) ]

##### Methods Removed

  - `void Serialize(Object[], Stream)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Binary/BinaryMessageSerializer.cs#L32) ]


#### NServiceBus.Serializers.Json.JsonMessageSerializer  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/JsonMessageSerializer.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/JsonMessageSerializer.cs) ]

##### Methods Removed

  - `T DeserializeObject<T>(string)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/JsonMessageSerializer.cs#L36) ]


#### NServiceBus.Serializers.Json.JsonMessageSerializerBase  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/JsonMessageSerializerBase.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/JsonMessageSerializerBase.cs) ]

##### Methods Removed

  - `void Serialize(Object[], Stream)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/Json/JsonMessageSerializerBase.cs#L46) ]


#### NServiceBus.Serializers.XML.XmlMessageSerializer  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/XmlMessageSerializer.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/XmlMessageSerializer.cs) ]

##### Methods Removed

  - `void .ctor(NServiceBus.MessageInterfaces.IMessageMapper)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/XmlMessageSerializer.cs#L27) ]
  - `bool get_SkipWrappingElementForSingleMessages()` 
  - `void Serialize(Object[], Stream)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/XmlMessageSerializer.cs#L836) ]
  - `void set_SkipWrappingElementForSingleMessages(bool)` 


#### NServiceBus.Settings.ScaleOutSettings  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/ScaleOutSettings.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Settings/ScaleOutSettings.cs) ]

##### Methods Removed

  - `void .ctor()` 
  - `NServiceBus.Settings.ScaleOutSettings UseSingleBrokerQueue()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/ScaleOutSettings.cs#L16) ]
  - `NServiceBus.Settings.ScaleOutSettings UseUniqueBrokerQueuePerMachine()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/ScaleOutSettings.cs#L27) ]


#### NServiceBus.Settings.SettingsHolder  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/SettingsHolder.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Settings/SettingsHolder.cs) ]

##### Methods changed to non-public

  - `void PreventChanges()` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/SettingsHolder.cs#L165) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Settings/SettingsHolder.cs#L253) ]

##### Methods Removed

  - `void ApplyTo<T>()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/SettingsHolder.cs#L180) ]
  - `void Reset()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/SettingsHolder.cs#L115) ]


#### NServiceBus.Settings.TransactionSettings  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/TransactionSettings.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Settings/TransactionSettings.cs) ]

##### Methods Removed

  - `void .ctor()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/TransactionSettings.cs#L33) ]
  - `NServiceBus.Settings.TransactionSettings Advanced(Action<NServiceBus.Settings.TransactionSettings/TransactionAdvancedSettings>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Settings/TransactionSettings.cs#L65) ]


#### NServiceBus.Timeout.Core.IPersistTimeouts  

##### Methods Removed

  - `List<Tuple<string, DateTime>> GetNextChunk(DateTime, DateTime&)` 


#### NServiceBus.Timeout.Core.TimeoutData  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Timeout/Core/TimeoutData.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Timeout/Core/TimeoutData.cs) ]

##### Methods Removed

  - `string get_CorrelationId()` 
  - `void set_CorrelationId(string)` 


#### NServiceBus.TransportMessage  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/TransportMessage.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/TransportMessage.cs) ]

##### Methods Removed

  - `string get_IdForCorrelation()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/TransportMessage.cs#L85) ]
  - `void set_ReplyToAddress(NServiceBus.Address)` 


#### NServiceBus.Transports.IDeferMessages  

##### Methods Removed

  - `void Defer(NServiceBus.TransportMessage, DateTime, NServiceBus.Address)` 


#### NServiceBus.Transports.IPublishMessages  

##### Methods Removed

  - `bool Publish(NServiceBus.TransportMessage, IEnumerable<Type>)` 


#### NServiceBus.Transports.ISendMessages  

##### Methods Removed

  - `void Send(NServiceBus.TransportMessage, NServiceBus.Address)` 


#### NServiceBus.Transports.Msmq.MsmqDequeueStrategy  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/MsmqDequeueStrategy.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/Msmq/MsmqDequeueStrategy.cs) ]

##### Methods Removed

  - `void .ctor()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/MsmqDequeueStrategy.cs#L363) ]
  - `bool get_PurgeOnStartup()` 
  - `NServiceBus.Transports.Msmq.MsmqUnitOfWork get_UnitOfWork()` 
  - `void set_PurgeOnStartup(bool)` 
  - `void set_UnitOfWork(NServiceBus.Transports.Msmq.MsmqUnitOfWork)` 


#### NServiceBus.Transports.Msmq.MsmqMessageSender  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/MsmqMessageSender.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/Msmq/MsmqMessageSender.cs) ]

##### Methods Removed

  - `void Send(NServiceBus.TransportMessage, NServiceBus.Address)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/MsmqMessageSender.cs#L38) ]


#### NServiceBus.Transports.Msmq.MsmqUnitOfWork  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs) ]

##### Methods changed to non-public

  - `void ClearTransaction()` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs#L31) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs#L45) ]
  - `void SetTransaction(System.Messaging.MessageQueueTransaction)` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs#L21) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs#L31) ]


#### NServiceBus.Unicast.MessageHandlerRegistry  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/MessageHandlerRegistry.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/MessageHandlerRegistry.cs) ]

##### Methods Removed

  - `void .ctor()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/MessageHandlerRegistry.cs#L81) ]


#### NServiceBus.Unicast.Messages.MessageMetadata  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Messages/MessageMetadata.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Messages/MessageMetadata.cs) ]

##### Methods Removed

  - `void .ctor()` 
  - `void set_MessageHierarchy(IEnumerable<Type>)` 
  - `void set_MessageType(Type)` 
  - `void set_Recoverable(bool)` 
  - `void set_TimeToBeReceived(TimeSpan)` 


#### NServiceBus.Unicast.Queuing.IWantQueueCreated  

##### Methods Removed

  - `bool get_IsDisabled()` 


#### NServiceBus.Unicast.Transport.ControlMessage  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/ControlMessage.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/ControlMessage.cs) ]

##### Methods Removed

  - `NServiceBus.TransportMessage Create(NServiceBus.Address)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/ControlMessage.cs#L14) ]


#### NServiceBus.Unicast.Transport.ITransport  

##### Methods Removed

  - `int get_MaxThroughputPerSecond()` 
  - `int get_NumberOfWorkerThreads()` 
  - `void set_MaxThroughputPerSecond(int)` 


#### NServiceBus.Unicast.Transport.TransactionSettings  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransactionSettings.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/TransactionSettings.cs) ]

##### Methods Removed

  - `void .ctor()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransactionSettings.cs#L9) ]
  - `NServiceBus.Unicast.Transport.TransactionSettings get_Default()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransactionSettings.cs#L33) ]
  - `bool get_DontUseDistributedTransactions()` 
  - `void set_DontUseDistributedTransactions(bool)` 


#### NServiceBus.Unicast.Transport.TransportReceiver  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs) ]

##### Methods changed to non-public

  - `void DisposeManaged()` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L481) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L421) ]
  - `void set_MaximumConcurrencyLevel(int)` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L75) ]
  - `void set_TransactionSettings(NServiceBus.Unicast.Transport.TransactionSettings)` 

##### Methods Removed

  - `void .ctor()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L493) ]
  - `void ChangeNumberOfWorkerThreads(int)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L171) ]
  - `int get_MaxThroughputPerSecond()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L120) ]
  - `int get_NumberOfWorkerThreads()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L64) ]
  - `void set_MaxThroughputPerSecond(int)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L123) ]
  - `void Start(string)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L176) ]


#### NServiceBus.Unicast.UnicastBus  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ]

##### Methods changed to non-public

  - `void DisposeManaged()` [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L949) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/UnicastBus.cs#L746) ]

##### Methods Removed

  - `void add_MessageReceived(NServiceBus.Unicast.UnicastBus/MessageReceivedDelegate)` 
  - `void add_MessagesSent(EventHandler<NServiceBus.Unicast.MessagesEventArgs>)` 
  - `void add_NoSubscribersForMessage(EventHandler<NServiceBus.Unicast.MessageEventArgs>)` 
  - `void add_Started(EventHandler)` 
  - `NServiceBus.ICallback Defer(TimeSpan, Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L721) ]
  - `NServiceBus.ICallback Defer(DateTime, Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L745) ]
  - `NServiceBus.Address get_ForwardReceivedMessagesTo()` 
  - `NServiceBus.Address get_MasterNodeAddress()` 
  - `NServiceBus.Audit.MessageAuditer get_MessageAuditer()` 
  - `NServiceBus.Transports.IDeferMessages get_MessageDeferrer()` 
  - `IDictionary<Type, Type> get_MessageDispatcherMappings()` 
  - `NServiceBus.Unicast.Messages.MessageMetadataRegistry get_MessageMetadataRegistry()` 
  - `NServiceBus.Transports.IPublishMessages get_MessagePublisher()` 
  - `NServiceBus.Serialization.IMessageSerializer get_MessageSerializer()` 
  - `bool get_SkipDeserialization()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L1028) ]
  - `NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.SubcriberSideFiltering.SubscriptionPredicatesEvaluator get_SubscriptionPredicatesEvaluator()` 
  - `TimeSpan get_TimeToBeReceivedOnForwardedMessages()` 
  - `void Publish<T>(T[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L309) ]
  - `void remove_MessageReceived(NServiceBus.Unicast.UnicastBus/MessageReceivedDelegate)` 
  - `void remove_MessagesSent(EventHandler<NServiceBus.Unicast.MessagesEventArgs>)` 
  - `void remove_NoSubscribersForMessage(EventHandler<NServiceBus.Unicast.MessageEventArgs>)` 
  - `void remove_Started(EventHandler)` 
  - `void Reply(Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L472) ]
  - `NServiceBus.ICallback Send(Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L569) ]
  - `NServiceBus.ICallback Send(string, Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L625) ]
  - `NServiceBus.ICallback Send(NServiceBus.Address, Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L630) ]
  - `NServiceBus.ICallback Send(string, string, Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L670) ]
  - `NServiceBus.ICallback Send(NServiceBus.Address, string, Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L680) ]
  - `NServiceBus.ICallback SendLocal(Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L550) ]
  - `NServiceBus.ICallback SendToSites(IEnumerable<string>, object)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L700) ]
  - `NServiceBus.ICallback SendToSites(IEnumerable<string>, Object[])` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L707) ]
  - `void set_DisableMessageHandling(bool)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L68) ]
  - `void set_ForwardReceivedMessagesTo(NServiceBus.Address)` 
  - `void set_MasterNodeAddress(NServiceBus.Address)` 
  - `void set_MessageAuditer(NServiceBus.Audit.MessageAuditer)` 
  - `void set_MessageDeferrer(NServiceBus.Transports.IDeferMessages)` 
  - `void set_MessageDispatcherMappings(IDictionary<Type, Type>)` 
  - `void set_MessageMetadataRegistry(NServiceBus.Unicast.Messages.MessageMetadataRegistry)` 
  - `void set_MessagePublisher(NServiceBus.Transports.IPublishMessages)` 
  - `void set_MessageSerializer(NServiceBus.Serialization.IMessageSerializer)` 
  - `void set_SkipDeserialization(bool)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L1029) ]
  - `void set_SubscriptionPredicatesEvaluator(NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.SubcriberSideFiltering.SubscriptionPredicatesEvaluator)` 
  - `void set_TimeToBeReceivedOnForwardedMessages(TimeSpan)` 
  - `void Shutdown()` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L988) ]
  - `NServiceBus.IBus Start(Action)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L811) ]
  - `void Subscribe<T>(Predicate<T>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L352) ]
  - `void Subscribe(Type, Predicate<object>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Unicast/UnicastBus.cs#L373) ]


#### NServiceBus.XmlSerializerConfigurationExtensions  [ [old](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializerConfigurationExtensions.cs) | [new](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializerConfigurationExtensions_Obsolete.cs) ]

##### Methods Removed

  - `NServiceBus.Settings.SerializationSettings Xml(NServiceBus.Settings.SerializationSettings, Action<NServiceBus.Serializers.XML.Config.XmlSerializationSettings>)` [ [link](https://github.com/Particular/NServiceBus/blob/4.6.7/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializerConfigurationExtensions.cs#L15) ]


### The following types have Obsoletes.

#### NServiceBus.ConfigureQueueCreation  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ConfigureQueueCreation.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure DoNotCreateQueues(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ConfigureQueueCreation_Obsolete.cs#L16) ]<br>Use `configuration.DoNotCreateQueues()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureInMemoryFaultManagement  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ConfigureInMemoryFaultManagement.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure InMemoryFaultManagement(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ConfigureInMemoryFaultManagement_obsolete.cs#L20) ]<br>Use `configuration.DiscardFailedMessagesInsteadOfSendingToErrorQueue()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureCriticalErrorAction  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/CriticalError/ConfigureCriticalErrorAction_Obsolete.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure DefineCriticalErrorAction(NServiceBus.Configure, Action<string, Exception>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/CriticalError/ConfigureCriticalErrorAction_Obsolete.cs#L17) ]<br>Use `configuration.DefineCriticalErrorAction()`, where configuration is an instance of type `BusConfiguration`. Please use `ConfigureCriticalErrorAction.DefineCriticalErrorAction()` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `void RaiseCriticalError(string, Exception)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/CriticalError/ConfigureCriticalErrorAction_Obsolete.cs#L26) ]<br>Inject an instace of `CriticalError` and call `CriticalError.Raise`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureFileShareDataBus  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/DataBus/ConfigureFileShareDataBus_Obsolete.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure FileShareDataBus(NServiceBus.Configure, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/DataBus/ConfigureFileShareDataBus_Obsolete.cs#L17) ]<br>Use `configuration.FileShareDataBus(basePath)`, where `configuration` is an instance of type `BusConfiguration`. Please use `ConfigureFileShareDataBus.FileShareDataBus(this BusConfiguration config, string basePath)` instead. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureRijndaelEncryptionService  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Encryption/ConfigureRijndaelEncryptionService_Obsolete.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure RijndaelEncryptionService(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Encryption/ConfigureRijndaelEncryptionService_Obsolete.cs#L15) ]<br>Use `configuration.RijndaelEncryptionService()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.IBus_Obsoletes  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/IBus_Obsoletes.cs) ]

Placeholder for obsoletes. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `T CreateInstance<T>(NServiceBus.IBus)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/IBus_Obsoletes.cs#L21) ]<br>Since multi message sends is obsoleted in v5 use `IBus.Send<T>()` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `T CreateInstance<T>(NServiceBus.IBus, Action<T>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/IBus_Obsoletes.cs#L34) ]<br>Since multi message sends is obsoleted in v5 use `IBus.Send<T>()` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `object CreateInstance(NServiceBus.IBus, Type)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/IBus_Obsoletes.cs#L46) ]<br>Since multi message sends is obsoleted in v5 use `IBus.Send<T>()` instead. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.InstallConfigExtensions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Installation/InstallConfigExtensions_obsolete.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure EnableInstallers(NServiceBus.Configure, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Installation/InstallConfigExtensions_obsolete.cs#L15) ]<br>Use `configuration.EnableInstallers()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureLicenseExtensions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Licensing/ConfigureLicenseExtensions.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure License(NServiceBus.Configure, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Licensing/ConfigureLicenseExtensions_Obsolete.cs#L16) ]<br>Use `configuration.License(licenseText)`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure LicensePath(NServiceBus.Configure, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Licensing/ConfigureLicenseExtensions_Obsolete.cs#L26) ]<br>Use `configuration.LicensePath(licenseFile)`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.MessageConventionException  

Since the case where this exception was thrown should not be handled by consumers of the API it has been removed. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Persistence.ConcurrencyException  

Since the case where this exception was thrown should not be handled by consumers of the API it has been removed. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Unicast.Transport.TransportMessageHandlingFailedException  

Since the case where this exception was thrown should not be handled by consumers of the API it has been removed. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Unicast.Queuing.FailedToSendMessageException  

Since the case where this exception was thrown should not be handled by consumers of the API it has been removed. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.IdGeneration.CombGuid  

This class was never intended to be exposed as part of the public API. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Utils.RegistryReader&lt;T&gt;  

This class was never intended to be exposed as part of the public API. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Utils.FileVersionRetriever  

This class was never intended to be exposed as part of the public API. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Unicast.Callback  

Please use `ICallback` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Unicast.IUnicastBus  

Please use `IBus` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Hosting.GenericHost  

This class was never intended to be exposed as part of the public API. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Hosting.IHost  

This class was never intended to be exposed as part of the public API. Will be removed in version 6.0.0. TreatAsError=True



#### System.Threading.Tasks.Schedulers.MTATaskScheduler  

This class was never intended to be exposed as part of the public API. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Log4NetBridge.ConfigureInternalLog4NetBridge  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.ConsoleLogger  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.ConsoleLoggerFactory  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.Log4NetAdapter.Log4NetAppenderFactory  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.Log4NetAdapter.Log4NetConfigurator  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.Log4NetAdapter.Log4NetLogger  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.Log4NetAdapter.Log4NetLoggerFactory  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.NLogAdapter.NLogConfigurator  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.NLogAdapter.NLogLogger  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.NLogAdapter.NLogLoggerFactory  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.NLogAdapter.NLogTargetFactory  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.NullLogger  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.Loggers.NullLoggerFactory  

Sensible defaults for logging are now built into NServicebus. To customise logging there are external nuget packages available to connect NServiceBus to the various popular logging frameworks. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Logging.LoggingLibraryException  

Since the case where this exception was thrown should not be handled by consumers of the API it has been removed. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.IWantTheEndpointConfig  

`IHandleProfile` is now passed an instance of `Configure`. `IWantCustomInitialization` is now expected to return a new instance of `Configure`. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Timeout.Core.IManageTimeouts  

Timeout management is an internal concern and cannot be replaced. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Installation.Environments.Windows  

IEnvironment is no longer required instead use the non generic `INeedToInstallSomething` and use `configuration.EnableInstallers()`, where `configuration` is an instance of type `BusConfiguration` to execute them. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Installation.IEnvironment  

`IEnvironment` is no longer required instead use the non generic `INeedToInstallSomething` and use `configuration.EnableInstallers()`, where `configuration` is an instance of type `BusConfiguration` to execute them. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Installation.INeedToInstallSomething&lt;T&gt;  

`IEnvironment` is no longer required instead use the non generic `INeedToInstallSomething` and use `configuration.EnableInstallers()`, where `configuration` is an instance of type `BusConfiguration` to execute them. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Installer&lt;T&gt;  

`IEnvironment` is no longer required instead use the non generic `INeedToInstallSomething` and use `configuration.EnableInstallers()`, where `configuration` is an instance of type `BusConfiguration` to execute them. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Install  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

`IEnvironment` is no longer required instead use the non generic `INeedToInstallSomething` and use `configuration.EnableInstallers()`, where `configuration` is an instance of type `BusConfiguration` to execute them. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Installer<T> ForInstallationOn<T>(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L352) ]<br>`IEnvironment` is no longer required instead use the non generic `INeedToInstallSomething` and use `configuration.EnableInstallers()`, where `configuration` is an instance of type `BusConfiguration` to execute them. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Installer<T> ForInstallationOn<T>(NServiceBus.Configure, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L361) ]<br>`IEnvironment` is no longer required instead use the non generic `INeedToInstallSomething` and use `configuration.EnableInstallers()`, where `configuration` is an instance of type `BusConfiguration` to execute them. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.EndpointConventions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure DefineEndpointName(NServiceBus.Configure, Func<string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L383) ]<br>Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefineEndpointName(NServiceBus.Configure, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L392) ]<br>Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureRavenPersistence  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure CustomiseRavenPersistence(NServiceBus.Configure, object)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L413) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure MessageToDatabaseMappingConvention(NServiceBus.Configure, Func<NServiceBus.IMessageContext, string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L422) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().SetMessageToDatabaseMappingConvention(convention)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure RavenPersistence(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L431) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package.` Use `configuration.UsePersistence<RavenDBPersistence>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure RavenPersistence(NServiceBus.Configure, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L440) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().SetDefaultDocumentStore(...)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure RavenPersistence(NServiceBus.Configure, string, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L449) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().SetDefaultDocumentStore(...)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure RavenPersistence(NServiceBus.Configure, Func<string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L458) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().SetDefaultDocumentStore(...)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure RavenPersistence(NServiceBus.Configure, Func<string>, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L467) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().SetDefaultDocumentStore(...)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure RavenPersistenceWithStore(NServiceBus.Configure, object)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L477) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().SetDefaultDocumentStore(documentStore)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `void RegisterDefaults()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L486) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureRavenSagaPersister  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure RavenSagaPersister(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L504) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().For(Storage.Sagas)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureRavenSubscriptionStorage  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure RavenSubscriptionStorage(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L523) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().For(Storage.Subscriptions)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureTimeoutManager  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure DisableTimeoutManager(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L541) ]<br>Use `configuration.DisableFeature<TimeoutManager>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure UseInMemoryTimeoutPersister(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L551) ]<br>Use `configuration.UsePersistence<InMemoryPersistence>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure UseRavenTimeoutPersister(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L560) ]<br>RavenDB has been moved to its own stand alone nuget 'NServiceBus.RavenDB'. Install the nuget package. Use `configuration.UsePersistence<RavenDBPersistence>().For(Storage.Timeouts)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureUnicastBus  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Address GetTimeoutManagerAddress(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L579) ]<br>Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure UnicastBus(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L588) ]<br>UnicastBus is now the default and hence calling this method is redundant. `Bus.Create(configuration)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.MessageHandlerExtensionMethods  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

Inject an instance of `IBus` in the constructor and assign that to a field for use. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.IBus Bus<T>(NServiceBus.IHandleMessages<T>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L607) ]<br>Inject an instance of `IBus` in the constructor and assign that to a field for use. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Transports.Msmq.MsmqUtilities  

`MsmqUtilities` was never intended to be exposed as part of the public API. PLease copy the required functionality into your codebase. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Unicast.Config.ConfigUnicastBus  

Please use `Configure` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Features.StorageDrivenPublisher  

Please use `NServiceBus.Features.StorageDrivenPublishing` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.TransportReceiverConfig  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs) ]

Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure UseTransport<T>(NServiceBus.Configure, Func<string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L659) ]<br>Use `configuration.UseTransport(transportDefinitionType).ConnectionString()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure UseTransport(NServiceBus.Configure, Type, Func<string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Obsoletes.cs#L668) ]<br>Use `configuration.UseTransport(transportDefinitionType).ConnectionString()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Address  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Address.cs) ]



##### Obsolete Methods

  - `void InitializeLocalAddress(string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Address.cs#L48) ]<br>Please use `ConfigureTransport<T>.LocalAddress(queue)` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `void OverridePublicReturnAddress(NServiceBus.Address)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Address.cs#L62) ]<br>Use `configuration.OverridePublicReturnAddress(address)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Configure  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure.cs) ]


##### Obsolete Fields

  - `Func<FileInfo, Assembly> LoadAssembly`<br>No longer an extension point for NSB. Will be removed in version 6.0.0. TreatAsError=True
  - `Func<string> GetEndpointNameAction`<br>Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True


##### Obsolete Methods

  - `bool WithHasBeenCalled()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L64) ]<br>Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure RunCustomAction(Action)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L73) ]<br>Simply execute this action instead of calling this method. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.IStartableBus CreateBus()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L82) ]<br>Not needed, can safely be removed. Will be removed in version 6.0.0. TreatAsError=True
  - `bool BuilderIsConfigured()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L93) ]<br>Will be removed in version 6.0.0. TreatAsError=True
  - `T GetConfigSection<T>()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L102) ]<br>Please use `ReadOnlySettings.GetConfigSection<T>` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.ObjectBuilder.IComponentConfig Component(Type, NServiceBus.DependencyLifecycle)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L112) ]<br>Configure is now instance based. Please use `configure.Configurer.ConfigureComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.ObjectBuilder.IComponentConfig<T> Component<T>(NServiceBus.DependencyLifecycle)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L122) ]<br>Configure is now instance based. Please use `configure.Configurer.ConfigureComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.ObjectBuilder.IComponentConfig<T> Component<T>(Func<T>, NServiceBus.DependencyLifecycle)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L132) ]<br>Configure is now instance based. Please use `configure.Configurer.ConfigureComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.ObjectBuilder.IComponentConfig<T> Component<T>(Func<NServiceBus.ObjectBuilder.IBuilder, T>, NServiceBus.DependencyLifecycle)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L142) ]<br>Configure is now instance based. Please use `configure.Configurer.ConfigureComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `bool HasComponent<T>()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L152) ]<br>Configure is now instance based. Please use `configure.Configurer.HasComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `bool HasComponent(Type)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L162) ]<br>Configure is now instance based. Please use `configure.Configurer.HasComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure CustomConfigurationSource(NServiceBus.Config.ConfigurationSource.IConfigurationSource)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L171) ]<br>Use `configuration.CustomConfigurationSource(myConfigSource)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L181) ]<br>Please use `Bus.Create(new BusConfiguration())` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With(string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L192) ]<br>var config = new BusConfig();
config.ScanAssembliesInDirectory(directoryToProbe);
Bus.Create(config);. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With(IEnumerable<Assembly>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L201) ]<br>Use `configuration.AssembliesToScan(listOfAssemblies)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With(Assembly[])` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L210) ]<br>Use `configuration.AssembliesToScan(listOfAssemblies)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With(IEnumerable<Type>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L219) ]<br>Use `configuration.TypesToScan(listOfTypes)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefineEndpointName(Func<string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L228) ]<br>Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefineEndpointName(string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L240) ]<br>Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Configure  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure.cs) ]


##### Obsolete Fields

  - `Func<FileInfo, Assembly> LoadAssembly`<br>No longer an extension point for NSB. Will be removed in version 6.0.0. TreatAsError=True
  - `Func<string> GetEndpointNameAction`<br>Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True


##### Obsolete Methods

  - `bool WithHasBeenCalled()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L64) ]<br>Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure RunCustomAction(Action)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L73) ]<br>Simply execute this action instead of calling this method. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.IStartableBus CreateBus()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L82) ]<br>Not needed, can safely be removed. Will be removed in version 6.0.0. TreatAsError=True
  - `bool BuilderIsConfigured()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L93) ]<br>Will be removed in version 6.0.0. TreatAsError=True
  - `T GetConfigSection<T>()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L102) ]<br>Please use `ReadOnlySettings.GetConfigSection<T>` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.ObjectBuilder.IComponentConfig Component(Type, NServiceBus.DependencyLifecycle)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L112) ]<br>Configure is now instance based. Please use `configure.Configurer.ConfigureComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.ObjectBuilder.IComponentConfig<T> Component<T>(NServiceBus.DependencyLifecycle)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L122) ]<br>Configure is now instance based. Please use `configure.Configurer.ConfigureComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.ObjectBuilder.IComponentConfig<T> Component<T>(Func<T>, NServiceBus.DependencyLifecycle)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L132) ]<br>Configure is now instance based. Please use `configure.Configurer.ConfigureComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.ObjectBuilder.IComponentConfig<T> Component<T>(Func<NServiceBus.ObjectBuilder.IBuilder, T>, NServiceBus.DependencyLifecycle)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L142) ]<br>Configure is now instance based. Please use `configure.Configurer.ConfigureComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `bool HasComponent<T>()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L152) ]<br>Configure is now instance based. Please use `configure.Configurer.HasComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `bool HasComponent(Type)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L162) ]<br>Configure is now instance based. Please use `configure.Configurer.HasComponent` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure CustomConfigurationSource(NServiceBus.Config.ConfigurationSource.IConfigurationSource)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L171) ]<br>Use `configuration.CustomConfigurationSource(myConfigSource)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L181) ]<br>Please use `Bus.Create(new BusConfiguration())` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With(string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L192) ]<br>var config = new BusConfig();
config.ScanAssembliesInDirectory(directoryToProbe);
Bus.Create(config);. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With(IEnumerable<Assembly>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L201) ]<br>Use `configuration.AssembliesToScan(listOfAssemblies)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With(Assembly[])` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L210) ]<br>Use `configuration.AssembliesToScan(listOfAssemblies)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure With(IEnumerable<Type>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L219) ]<br>Use `configuration.TypesToScan(listOfTypes)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefineEndpointName(Func<string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L228) ]<br>Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefineEndpointName(string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Configure_Obsolete.cs#L240) ]<br>Use `configuration.EndpointName(myEndpointName)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Features.FeatureSettings  

Use `configuration.EnableFeature<T>()` or `configuration.DisableFeature<T>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.ExtensionMethods  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ExtensionMethods.cs) ]



##### Obsolete Methods

  - `string GetHeader(NServiceBus.IMessage, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ExtensionMethods.cs#L61) ]<br>Please use `bus.GetMessageHeader(msg, key)` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `void SetHeader(NServiceBus.IMessage, string, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ExtensionMethods.cs#L76) ]<br>Please use `bus.SetMessageHeader(msg, key, value)` instead. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Headers  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Headers.cs) ]


##### Obsolete Fields

  - `string InvokedSagas`<br>Enriching the headers for saga related information has been moved to the SagaAudit plugin in ServiceControl. Add a reference to the Saga audit plugin in your endpoint to get more information. Will be treated as an error from version 5.1.0. Will be removed in version 6.0.0. TreatAsError=False


##### Obsolete Methods

  - `string GetMessageHeader(object, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Headers.cs#L226) ]<br>Please use `bus.GetMessageHeader(msg, key)` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `void SetMessageHeader(object, string, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Headers.cs#L243) ]<br>Please use `bus.SetMessageHeader(msg, key, value)` instead. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Headers  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Headers.cs) ]


##### Obsolete Fields

  - `string InvokedSagas`<br>Enriching the headers for saga related information has been moved to the SagaAudit plugin in ServiceControl. Add a reference to the Saga audit plugin in your endpoint to get more information. Will be treated as an error from version 5.1.0. Will be removed in version 6.0.0. TreatAsError=False


##### Obsolete Methods

  - `string GetMessageHeader(object, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Headers.cs#L226) ]<br>Please use `bus.GetMessageHeader(msg, key)` instead. Will be removed in version 6.0.0. TreatAsError=True
  - `void SetMessageHeader(object, string, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Headers.cs#L243) ]<br>Please use `bus.SetMessageHeader(msg, key, value)` instead. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.IConfigureLogging  

Please use `NServiceBus.Hosting.Profiles.IConfigureLogging` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.IConfigureLoggingForProfile&lt;T&gt;  

Please use `NServiceBus.Hosting.Profiles.IConfigureLoggingForProfile` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.IWantCustomLogging  

Please use `NServiceBus.Hosting.Profiles.IConfigureLogging` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.IInMemoryOperations  

Removed to reduce complexity and API confusion. Will be treated as an error from version 5.1.0. Will be removed in version 6.0.0. TreatAsError=False



#### NServiceBus.IMessageModule  

Please use `NServiceBus.UnitOfWork.IManageUnitsOfWork` instead. Will be removed in version 5.1.0. TreatAsError=True



#### NServiceBus.Schedule  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Scheduling/Schedule_Obsolete.cs) ]



##### Obsolete Methods

  - `NServiceBus.Schedule Every(TimeSpan)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Scheduling/Schedule_Obsolete.cs#L15) ]<br>Inject an instance of `Schedule` to your class and then call the non-static version of `Schedule.Every(TimeSpan timeSpan, Action task)`. Will be removed in version 6.0.0. TreatAsError=True
  - `void Action(Action)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Scheduling/Schedule_Obsolete.cs#L24) ]<br>Inject an instance of `Schedule` to your class and then call the non static member `Schedule.Every(TimeSpan timeSpan, Action task)`. Will be removed in version 6.0.0. TreatAsError=True
  - `void Action(string, Action)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Scheduling/Schedule_Obsolete.cs#L33) ]<br>Inject an instance of `Schedule` to your class thenuse the non-static version of `Schedule.Every(TimeSpan timeSpan, string name, Action task)`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.SecondLevelRetries.Helpers.TransportMessageHelpers  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Helpers/TransportMessageHelpers_Obsolete.cs) ]

Access the `TransportMessage.Headers` dictionary directly. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Address GetAddressOfFaultingEndpoint(NServiceBus.TransportMessage)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Helpers/TransportMessageHelpers_Obsolete.cs#L20) ]<br>Access the `TransportMessage.Headers` dictionary directly using the `FaultsHeaderKeys.FailedQ` key. Will be removed in version 6.0.0. TreatAsError=True
  - `string GetHeader(NServiceBus.TransportMessage, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Helpers/TransportMessageHelpers_Obsolete.cs#L29) ]<br>Access the `TransportMessage.Headers` dictionary directly. Will be removed in version 6.0.0. TreatAsError=True
  - `bool HeaderExists(NServiceBus.TransportMessage, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Helpers/TransportMessageHelpers_Obsolete.cs#L38) ]<br>Access the `TransportMessage.Headers` dictionary directly. Will be removed in version 6.0.0. TreatAsError=True
  - `void SetHeader(NServiceBus.TransportMessage, string, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Helpers/TransportMessageHelpers_Obsolete.cs#L47) ]<br>Access the `TransportMessage.Headers` dictionary directly. Will be removed in version 6.0.0. TreatAsError=True
  - `int GetNumberOfRetries(NServiceBus.TransportMessage)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Helpers/TransportMessageHelpers_Obsolete.cs#L56) ]<br>Access the `TransportMessage.Headers` dictionary directly using the `Headers.Retries` key. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Serializers.XML.Config.XmlSerializationSettings  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializationSettings_Obsolete.cs) ]

Use configuration.UseSerialization<XmlSerializer>(), where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Serializers.XML.Config.XmlSerializationSettings DontWrapRawXml()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializationSettings_Obsolete.cs#L20) ]<br>Use `configuration.UseSerialization<XmlSerializer>().DontWrapRawXml()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Serializers.XML.Config.XmlSerializationSettings Namespace(string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializationSettings_Obsolete.cs#L29) ]<br>Use `configuration.UseSerialization<XmlSerializer>().Namespace(namespaceToUse)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Serializers.XML.Config.XmlSerializationSettings SanitizeInput()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializationSettings_Obsolete.cs#L38) ]<br>Use `configuration.UseSerialization<XmlSerializer>().SanitizeInput()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigurePurging  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/ConfigurePurging.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure PurgeOnStartup(NServiceBus.Configure, bool)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/ConfigurePurging_Obsolete.cs#L21) ]<br>Use `configuration.PurgeOnStartup()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.LoadMessageHandlersExtentions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Config/LoadMessageHandlersExtentions_Obsolete.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure LoadMessageHandlers(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Config/LoadMessageHandlersExtentions_Obsolete.cs#L15) ]<br>It is safe to remove this method call. This is the default behavior. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure LoadMessageHandlers<TFirst>(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Config/LoadMessageHandlersExtentions_Obsolete.cs#L24) ]<br>Use `configuration.LoadMessageHandlers<TFirst>`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure LoadMessageHandlers<T>(NServiceBus.Configure, NServiceBus.First<T>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Config/LoadMessageHandlersExtentions_Obsolete.cs#L33) ]<br>Use `configuration.LoadMessageHandlers<T>`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.MonitoringConfig  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Monitoring/MonitoringConfig_Obsolete.cs) ]

Use `configuration.EnableCriticalTimePerformanceCounter()` or `configuration.EnableSLAPerformanceCounter(TimeSpan)`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure SetEndpointSLA(NServiceBus.Configure, TimeSpan)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Monitoring/MonitoringConfig_Obsolete.cs#L20) ]<br>Use `configuration.EnableSLAPerformanceCounter(TimeSpan)`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure EnablePerformanceCounters(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Monitoring/MonitoringConfig_Obsolete.cs#L30) ]<br>Use `configuration.EnableCriticalTimePerformanceCounter()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Unicast.IMessageDispatcherFactory  

Please use `Use the pipeline and replace the InvokeHandlers step` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.TransportConfiguration  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/Config/TransportConfiguration.cs) ]

Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `void ConnectionString(string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/Config/TransportConfiguration.cs#L19) ]<br>Use `configuration.UseTransport<T>().ConnectionString(connectionString)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `void ConnectionStringName(string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/Config/TransportConfiguration.cs#L28) ]<br>Use `configuration.UseTransport<T>().ConnectionStringName(name)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `void ConnectionString(Func<string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/Config/TransportConfiguration.cs#L37) ]<br>Use` configuration.UseTransport<T>().ConnectionString(connectionString)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.UseTransportExtensions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/Config/UseTransportExtensions_Obsolete.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure UseTransport<T>(NServiceBus.Configure, Action<NServiceBus.TransportConfiguration>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/Config/UseTransportExtensions_Obsolete.cs#L16) ]<br>Use `configuration.UseTransport<T>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure UseTransport(NServiceBus.Configure, Type, Action<NServiceBus.TransportConfiguration>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/Transport/Config/UseTransportExtensions_Obsolete.cs#L25) ]<br>Use `configuration.UseTransport(transportDefinitionType)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureInMemoryTimeoutPersister  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/InMemory/Obsoletes/ConfigureInMemoryTimeoutPersister.cs) ]

Use `configuration.UsePersistence<InMemoryPersistence>()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure UseInMemoryTimeoutPersister(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/InMemory/Obsoletes/ConfigureInMemoryTimeoutPersister.cs#L20) ]<br>Use `configuration.UsePersistence<InMemoryPersistence>()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Saga.ISaga  

Please use `NServiceBus.Saga.Saga` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Saga.ISaga&lt;T&gt;  

Please use `NServiceBus.Saga.Saga<T>` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Saga.IPersistSagas  

Please use `ISagaPersister` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Saga.IConfigurable  

Since `ISaga` has been merged into the abstract class `Saga` this interface is no longer required. Please use `NServiceBus.Saga.Saga` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Saga.HasCompleted  

Since `ISaga` has been merged into the abstract class `Saga` this interface is no longer required. Please use `NServiceBus.Saga.Saga.Completed` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.ScaleOutExtentions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Settings/ScaleOutExtentions.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure ScaleOut(NServiceBus.Configure, Action<NServiceBus.Settings.ScaleOutSettings>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Settings/ScaleOutExtentions.cs#L27) ]<br>Use `configuration.ScaleOut().UseSingleBrokerQueue()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Unicast.UnicastBus  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ]



##### Obsolete Methods

  - `void Raise<T>(Action<T>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs#L12) ]<br>InMemory.Raise has been removed from the core. Will be removed in version 6.0.0. TreatAsError=True
  - `void Raise<T>(T)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs#L18) ]<br>InMemory.Raise has been removed from the core. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureBinarySerializer  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Binary/Config/ConfigureBinarySerializer_Obsolete.cs) ]

Use `configuration.UseSerialization<BinarySerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure BinarySerializer(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Binary/Config/ConfigureBinarySerializer_Obsolete.cs#L20) ]<br>Use `configuration.UseSerialization<BinarySerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureDefaultBuilder  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ConfigureDefaultBuilder_Obsolete.cs) ]

Default builder will be used automatically. It is safe to remove this code. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure DefaultBuilder(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ConfigureDefaultBuilder_Obsolete.cs#L20) ]<br>Default builder will be used automatically. It is safe to remove this code. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureDistributor  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Distributor/ConfigureDistributor_Obsolete.cs) ]

The NServiceBus Distributor was moved into its own assembly (NServiceBus.Distributor.MSMQ.dll), please make sure you reference the new assembly. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.ConfigureFaultsForwarder  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ConfigureFaultsForwarder_Obsolete.cs) ]

Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure MessageForwardingInCaseOfFault(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ConfigureFaultsForwarder_Obsolete.cs#L14) ]<br>It is safe to remove this method call. This is the default behavior. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.SecondLevelRetriesConfigExtensions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Config/SecondLevelRetriesConfigExtensions.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure SecondLevelRetries(NServiceBus.Configure, Action<NServiceBus.SecondLevelRetries.Config.SecondLevelRetriesSettings>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/SecondLevelRetries/Config/SecondLevelRetriesConfigExtensions.cs#L22) ]<br>Use `configuration.SecondLevelRetries().CustomRetryPolicy()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.BinarySerializerConfigurationExtensions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Binary/Config/BinarySerializerConfigurationExtensions_Obsolete.cs) ]

Use `configuration.UseSerialization<BinarySerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure Binary(NServiceBus.Settings.SerializationSettings)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Binary/Config/BinarySerializerConfigurationExtensions_Obsolete.cs#L21) ]<br>Use `configuration.UseSerialization<BinarySerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.JsonSerializerConfigurationExtensions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/JsonSerializerConfigurationExtensions.cs) ]



##### Obsolete Methods

  - `NServiceBus.Settings.SerializationSettings Json(NServiceBus.Settings.SerializationSettings)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/JsonSerializerConfigurationExtensions.cs#L19) ]<br>Use `configuration.UseSerialization<JsonSerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Settings.SerializationSettings Bson(NServiceBus.Settings.SerializationSettings)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/JsonSerializerConfigurationExtensions.cs#L28) ]<br>Use `configuration.UseSerialization<BsonSerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.XmlSerializerConfigurationExtensions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializerConfigurationExtensions_Obsolete.cs) ]



##### Obsolete Methods

  - `NServiceBus.Configure Xml(NServiceBus.Settings.SerializationSettings, Action<NServiceBus.Serializers.XML.Config.XmlSerializationSettings>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/XmlSerializerConfigurationExtensions_Obsolete.cs#L22) ]<br>Use configuration.UseSerialization<XmlSerializer>(), where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Settings.SerializationSettings  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serialization/SerializationSettings.cs) ]

Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Settings.SerializationSettings WrapSingleMessages()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serialization/SerializationSettings.cs#L18) ]<br>In version 5 multi-message sends was removed. So Wrapping messages is no longer required. If you are communicating with version 3 ensure you are on the latest 3.3.x. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Settings.SerializationSettings DontWrapSingleMessages()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serialization/SerializationSettings.cs#L27) ]<br>In version 5 multi-message sends was removed. So Wrapping messages is no longer required. If you are communicating with version 3 ensure you are on the latest 3.3.x. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureInMemorySagaPersister  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/InMemory/Obsoletes/ConfigureInMemorySagaPersister.cs) ]

Use `configuration.UsePersistence<InMemoryPersistence>()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure InMemorySagaPersister(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/InMemory/Obsoletes/ConfigureInMemorySagaPersister.cs#L20) ]<br>Use `configuration.UsePersistence<InMemoryPersistence>()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureInMemorySubscriptionStorage  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/InMemory/Obsoletes/ConfigureInMemorySubscriptionStorage.cs) ]

Use `configuration.UsePersistence<InMemoryPersistence>()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure InMemorySubscriptionStorage(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/InMemory/Obsoletes/ConfigureInMemorySubscriptionStorage.cs#L20) ]<br>Use `configuration.UsePersistence<InMemoryPersistence>()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureJsonSerializer  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/ConfigureJsonSerializer_Obsolete.cs) ]

Use `configuration.UseSerialization<JsonSerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure JsonSerializer(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/ConfigureJsonSerializer_Obsolete.cs#L19) ]<br>Use `configuration.UseSerialization<JsonSerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure BsonSerializer(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/Json/Config/ConfigureJsonSerializer_Obsolete.cs#L28) ]<br>Use `configuration.UseSerialization<BsonSerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureMasterNode  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Distributor/MasterNode/ConfigureMasterNode_Obsolete.cs) ]

The NServiceBus Distributor was moved into its own assembly (NServiceBus.Distributor.MSMQ.dll), please make sure you reference the new assembly. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.ConfigureMsmqMessageQueue  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/Msmq/Config/ConfigureMsmqMessageQueue_Obsolete.cs) ]

Please use 'UsingTransport<MsmqTransport>' on your 'IConfigureThisEndpoint' class or use `configuration.UseTransport<MsmqTransport>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure MsmqTransport(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Transports/Msmq/Config/ConfigureMsmqMessageQueue_Obsolete.cs#L13) ]<br>Please use 'UsingTransport<MsmqTransport>' on your 'IConfigureThisEndpoint' class or use `configuration.UseTransport<MsmqTransport>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureMsmqSubscriptionStorage  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/Msmq/SubscriptionStorage/ConfigureMsmqSubscriptionStorage_Obsolete.cs) ]

Use `configuration.UsePersistence<MsmqPersistence>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure MsmqSubscriptionStorage(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/Msmq/SubscriptionStorage/ConfigureMsmqSubscriptionStorage_Obsolete.cs#L20) ]<br>Use configuration.UsePersistence<MsmqPersistence>(), where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure MsmqSubscriptionStorage(NServiceBus.Configure, string)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Persistence/Msmq/SubscriptionStorage/ConfigureMsmqSubscriptionStorage_Obsolete.cs#L29) ]<br>Use `configuration.UsePersistence<MsmqPersistence>()`, where `configuration` is an instance of type `BusConfiguration` and assign the queue name via `MsmqSubscriptionStorageConfig` section. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureXmlSerializer  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/ConfigureXmlSerializer_Obsolete.cs) ]

Use `configuration.UseSerialization<XmlSerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure XmlSerializer(NServiceBus.Configure, string, bool)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Serializers/XML/Config/ConfigureXmlSerializer_Obsolete.cs#L20) ]<br>Use `configuration.UseSerialization<XmlSerializer>()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureSettingLocalAddressNameAction  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Config/Advanced/ConfigureSettingLocalAddressNameAction_Obsolete.cs) ]

Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure DefineLocalAddressNameFunc(NServiceBus.Configure, Func<string>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Config/Advanced/ConfigureSettingLocalAddressNameAction_Obsolete.cs#L18) ]<br>Queue name is controlled by the endpoint name. The endpoint name can be configured using a `EndpointNameAttribute`, by passing a serviceName parameter to the host or calling `BusConfiguration.EndpointName` in the fluent API. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ConfigureExtensions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Config/ConfigureExtensions.cs) ]

Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.IBus SendOnly(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Config/ConfigureExtensions.cs#L18) ]<br>Please use `Bus.CreateSendOnly(new BusConfiguration())` instead. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Config.MsmqMessageQueueConfig  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Config/MsmqMessageQueueConfig.cs) ]

Use NServiceBus/Transport connectionString instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Config.IFinalizeConfiguration  

`IFinalizeConfiguration` is no longer in use. Please use the Feature concept instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Encryption.Rijndael.EncryptionService  

The Rijndael encryption functionality was an internal implementation detail of NServicebus as such it has been removed from the public API. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.IWantCustomInitialization  

Please use `INeedInitialization` or `IConfigureThisEndpoint`. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.IWantToRunBeforeConfiguration  

`IWantToRunBeforeConfiguration` is no longer in use. Please use the Feature concept instead and register a Default() in the ctor of your feature. If you used this to apply your own conventions please use use `configuration.Conventions().Defining...` , where configuration is an instance of type `BusConfiguration` available by implementing `IConfigureThisEndpoint` or `INeedInitialization`. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.MessageConventions  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageConventions_Obsolete.cs) ]

Use `configuration.Conventions().DefiningMessagesAs(definesMessageType)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure DefiningMessagesAs(NServiceBus.Configure, Func<Type, bool>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageConventions_Obsolete.cs#L20) ]<br>Use `configuration.Conventions().DefiningMessagesAs(definesMessageType)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefiningCommandsAs(NServiceBus.Configure, Func<Type, bool>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageConventions_Obsolete.cs#L30) ]<br>Use `configuration.Conventions().DefiningCommandsAs(definesCommandType)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefiningEventsAs(NServiceBus.Configure, Func<Type, bool>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageConventions_Obsolete.cs#L40) ]<br>Use `configuration.Conventions().DefiningEventsAs(definesEventType)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefiningEncryptedPropertiesAs(NServiceBus.Configure, Func<PropertyInfo, bool>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageConventions_Obsolete.cs#L50) ]<br>Use `configuration.Conventions().DefiningEncryptedPropertiesAs(definesEncryptedProperty)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefiningDataBusPropertiesAs(NServiceBus.Configure, Func<PropertyInfo, bool>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageConventions_Obsolete.cs#L60) ]<br>Use `configuration.Conventions().DefiningDataBusPropertiesAs(definesDataBusProperty)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefiningTimeToBeReceivedAs(NServiceBus.Configure, Func<Type, TimeSpan>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageConventions_Obsolete.cs#L70) ]<br>Use `configuration.Conventions().DefiningTimeToBeReceivedAs(retrieveTimeToBeReceived)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DefiningExpressMessagesAs(NServiceBus.Configure, Func<Type, bool>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/MessageConventions_Obsolete.cs#L80) ]<br>Use `configuration.Conventions().DefiningExpressMessagesAs(definesExpressMessageType)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.ObjectBuilder.Common.Config.ConfigureContainer  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ObjectBuilder/Common/ConfigureContainer_Obsolete.cs) ]

Use `configuration.UseContainer<T>()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure UsingContainer<T>(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ObjectBuilder/Common/ConfigureContainer_Obsolete.cs#L20) ]<br>Use `configuration.UseContainer<T>()`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure UsingContainer<T>(NServiceBus.Configure, T)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/ObjectBuilder/Common/ConfigureContainer_Obsolete.cs#L29) ]<br>Use `configuration.UseContainer(container)`, where configuration is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Scheduling.ScheduledTask  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Scheduling/ScheduledTask.cs) ]

The Schedule is now injectable, This won't be needed. Will be removed in version 5.1.0. TreatAsError=True



#### NServiceBus.SetLoggingLibrary  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs) ]

Log4Net and Nlog integration has been moved to a stand alone nugets, 'NServiceBus.Log4Net' and 'NServiceBus.NLog'. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure Log4Net(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs#L21) ]<br>Log4Net integration has been moved to a stand alone nuget 'NServiceBus.Log4Net'. Install the 'NServiceBus.Log4Net' nuget and run 'LogManager.Use<Log4NetFactory>();'. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure Log4Net<TAppender>(NServiceBus.Configure, Action<TAppender>)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs#L30) ]<br>Log4Net integration has been moved to a stand alone nuget 'NServiceBus.Log4Net'. Install the 'NServiceBus.Log4Net' nuget and run 'LogManager.Use<Log4NetFactory>();'. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure Log4Net(NServiceBus.Configure, object)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs#L39) ]<br>Log4Net integration has been moved to a stand alone nuget 'NServiceBus.Log4Net'. Install the 'NServiceBus.Log4Net' nuget and run 'LogManager.Use<Log4NetFactory>();'. Will be removed in version 6.0.0. TreatAsError=True
  - `void Log4Net()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs#L49) ]<br>Log4Net integration has been moved to a stand alone nuget 'NServiceBus.Log4Net'. Install the 'NServiceBus.Log4Net' nuget and run 'LogManager.Use<Log4NetFactory>();'. Will be removed in version 6.0.0. TreatAsError=True
  - `void Log4Net(Action)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs#L58) ]<br>Log4Net integration has been moved to a stand alone nuget 'NServiceBus.Log4Net'. Install the 'NServiceBus.Log4Net' nuget and run 'LogManager.Use<Log4NetFactory>();'. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure NLog(NServiceBus.Configure, Object[])` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs#L67) ]<br>Nlog integration has been moved to a stand alone nuget 'NServiceBus.NLog'. Install the 'NServiceBus.NLog' nuget and run 'LogManager.Use<NLogFactory>();'. Will be removed in version 6.0.0. TreatAsError=True
  - `void NLog()` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs#L76) ]<br>Nlog integration has been moved to a stand alone nuget 'NServiceBus.NLog'. Install the 'NServiceBus.NLog' nuget and run 'LogManager.Use<Log4NetFactory>();'. Will be removed in version 6.0.0. TreatAsError=True
  - `void Custom(NServiceBus.Logging.ILoggerFactory)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/Logging/SetLoggingLibrary_Obsolete.cs#L85) ]<br>Please use `LogManager.UseFactory(ILoggerFactory)` instead. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.TransactionalConfigManager  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/TransactionalConfigManager_Obsolete.cs) ]

Use `configuration.Transactions().Enable()` or `configuration.Transactions().Disable()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True



##### Obsolete Methods

  - `NServiceBus.Configure IsTransactional(NServiceBus.Configure, bool)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/TransactionalConfigManager_Obsolete.cs#L20) ]<br>Use `configuration.Transactions()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure DontUseTransactions(NServiceBus.Configure)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/TransactionalConfigManager_Obsolete.cs#L29) ]<br>Use `configuration.Transactions().Disable()`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure IsolationLevel(NServiceBus.Configure, IsolationLevel)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/TransactionalConfigManager_Obsolete.cs#L38) ]<br>Use `configuration.Transactions().IsolationLevel(IsolationLevel.Chaos)`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True
  - `NServiceBus.Configure TransactionTimeout(NServiceBus.Configure, TimeSpan)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/TransactionalConfigManager_Obsolete.cs#L47) ]<br>Use `configuration.Transactions().DefaultTimeout(TimeSpan.FromMinutes(5))`, where `configuration` is an instance of type `BusConfiguration`. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.TransportMessage  [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/TransportMessage.cs) ]



##### Obsolete Methods

  - `void .ctor(string, Dictionary<string, string>, NServiceBus.Address)` [ [link](https://github.com/Particular/NServiceBus/tree/master/src/NServiceBus.Core/TransportMessage.cs#L54) ]<br>headers[Headers.ReplyToAddress]=replyToAddress; var tm = new TransportMessage(id,headers). Will be treated as an error from version 5.1.0. Will be removed in version 6.0.0. TreatAsError=False

#### NServiceBus.AutomaticSubscriptions.IAutoSubscriptionStrategy  

Not an extension point any more. If you want full control over autosubscribe please turn the feature off and implement your own for-loop calling Bus.Subscribe<YourEvent>() when starting your endpoint. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Hosting.Profiles.IHandleProfile  



##### Obsolete Methods

  - `void ProfileActivated(NServiceBus.Configure)` <br>Please use `ProfileActivated(ConfigurationBuilder config)` instead. Will be removed in version 6.0.0. TreatAsError=True

#### NServiceBus.Distributor  

The NServiceBus Distributor was moved into its own assembly (NServiceBus.Distributor.MSMQ.dll), please make sure you reference the new assembly. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.MultiSite  

Please use `MultiSite Profile is now obsolete. Gateway has been moved to its own stand alone nuget 'NServiceBus.Gateway'. To enable Gateway, install the nuget package and then call `configuration.EnableFeature<Gateway>()`, where `configuration` is an instance of type `BusConfiguration`.` instead. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Master  

The NServiceBus Distributor was moved into its own assembly (NServiceBus.Distributor.MSMQ.dll), please make sure you reference the new assembly. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.Worker  

The NServiceBus Distributor was moved into its own assembly (NServiceBus.Distributor.MSMQ.dll), please make sure you reference the new assembly. Will be removed in version 6.0.0. TreatAsError=True



#### NServiceBus.AsA_Publisher  

Please use `AsA_Server` instead. Will be removed in version 6.0.0. TreatAsError=True




