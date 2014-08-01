---
title: API differences between V4 and V5
summary: API differences between V4 and V5 
tags:
- upgrade
- migration
- diff
---

####The following public types are missing in the new API.

[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Audit/MessageAuditer.cs) | new ] NServiceBus.Audit.MessageAuditer  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscriber.cs) | new ] NServiceBus.AutomaticSubscriptions.AutoSubscriber  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/AutomaticSubscriptions/DefaultAutoSubscriptionStrategy.cs) | new ] NServiceBus.AutomaticSubscriptions.DefaultAutoSubscriptionStrategy  
[ old | new ] NServiceBus.AutomaticSubscriptions.IAutoSubscriptionStrategy  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/AddressInitializer.cs) | new ] NServiceBus.Config.AddressInitializer  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/GatewayConfig.cs) | new ] NServiceBus.Config.ChannelCollection  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/GatewayConfig.cs) | new ] NServiceBus.Config.ChannelConfig  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/InfrastructureServices.cs) | new ] NServiceBus.Config.DisplayInfrastructureServicesStatus  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/GatewayConfig.cs) | new ] NServiceBus.Config.GatewayConfig  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/InfrastructureServices.cs) | new ] NServiceBus.Config.InfrastructureServices  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/GatewayConfig.cs) | new ] NServiceBus.Config.SiteCollection  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/GatewayConfig.cs) | new ] NServiceBus.Config.SiteConfig  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/WindowsInstallerRunner.cs) | new ] NServiceBus.Config.WindowsInstallerRunner  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/ConfigureGateway.cs) | new ] NServiceBus.ConfigureGateway  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/ConfigureSagas.cs) | new ] NServiceBus.ConfigureSagas  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/SecondLevelRetries/Config/ConfigureSecondLevelRetriesExtensions.cs) | new ] NServiceBus.ConfigureSecondLevelRetriesExtensions  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/DataBus/Config/Bootstrapper.cs) | new ] NServiceBus.DataBus.Config.Bootstrapper  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/DataBus/InMemory/InMemoryDataBus.cs) | new ] NServiceBus.DataBus.InMemory.InMemoryDataBus  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Serializers/Serializers.cs) | new ] NServiceBus.Features.Categories.Serializers  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Support/EnableDefaultFeatures.cs) | new ] NServiceBus.Features.EnableDefaultFeatures  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs) | new ] NServiceBus.Features.Feature&lt;T&gt;  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs) | new ] NServiceBus.Features.FeatureCategory  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Support/FeatureInitializer.cs) | new ] NServiceBus.Features.FeatureInitializer  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Gateway.cs) | new ] NServiceBus.Features.Gateway  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/SecondLevelRetries/Config/FeatureSettingsExtensions.cs) | new ] NServiceBus.FeatureSettingsExtensions  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Channels/Channel.cs) | new ] NServiceBus.Gateway.Channels.Channel  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Channels/ChannelFactory.cs) | new ] NServiceBus.Gateway.Channels.ChannelFactory  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Channels/ChannelTypeAttribute.cs) | new ] NServiceBus.Gateway.Channels.ChannelTypeAttribute  
[ old | new ] NServiceBus.Gateway.Channels.DataReceivedOnChannelArgs  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Channels/Http/DefaultResponder.cs) | new ] NServiceBus.Gateway.Channels.Http.DefaultResponder  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Channels/Http/HttpChannelReceiver.cs) | new ] NServiceBus.Gateway.Channels.Http.HttpChannelReceiver  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Channels/Http/HttpChannelSender.cs) | new ] NServiceBus.Gateway.Channels.Http.HttpChannelSender  
[ old | new ] NServiceBus.Gateway.Channels.Http.HttpHeaders  
[ old | new ] NServiceBus.Gateway.Channels.Http.IHttpResponder  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Channels/Http/SetDefaultResponder.cs) | new ] NServiceBus.Gateway.Channels.Http.SetDefaultResponder  
[ old | new ] NServiceBus.Gateway.Channels.IChannelFactory  
[ old | new ] NServiceBus.Gateway.Channels.IChannelReceiver  
[ old | new ] NServiceBus.Gateway.Channels.IChannelSender  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Channels/Channel.cs) | new ] NServiceBus.Gateway.Channels.ReceiveChannel  
[ old | new ] NServiceBus.Gateway.Deduplication.GatewayMessage  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/InMemory/Gateway/InMemoryDeduplication.cs) | new ] NServiceBus.Gateway.Deduplication.InMemoryDeduplication  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/Gateway/RavenDBDeduplication.cs) | new ] NServiceBus.Gateway.Deduplication.RavenDBDeduplication  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/DefaultInputAddress.cs) | new ] NServiceBus.Gateway.DefaultInputAddress  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/HeaderManagement/DataBusHeaderManager.cs) | new ] NServiceBus.Gateway.HeaderManagement.DataBusHeaderManager  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/HeaderManagement/GatewayHeaderManager.cs) | new ] NServiceBus.Gateway.HeaderManagement.GatewayHeaderManager  
[ old | new ] NServiceBus.Gateway.HeaderManagement.GatewayHeaders  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/HeaderManagement/HeaderMapper.cs) | new ] NServiceBus.Gateway.HeaderManagement.HeaderMapper  
[ old | new ] NServiceBus.Gateway.Notifications.IMessageNotifier  
[ old | new ] NServiceBus.Gateway.Notifications.INotifyAboutMessages  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Notifications/MessageNotifier.cs) | new ] NServiceBus.Gateway.Notifications.MessageNotifier  
[ old | new ] NServiceBus.Gateway.Notifications.MessageReceivedOnChannelArgs  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/InMemory/Gateway/InMemoryPersistence.cs) | new ] NServiceBus.Gateway.Persistence.InMemoryPersistence  
[ old | new ] NServiceBus.Gateway.Persistence.MessageInfo  
[ old | new ] NServiceBus.Gateway.Persistence.Raven.GatewayMessage  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/Gateway/RavenDBPersistence.cs) | new ] NServiceBus.Gateway.Persistence.Raven.RavenDbPersistence  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Receiving/ChannelException.cs) | new ] NServiceBus.Gateway.Receiving.ChannelException  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Receiving/ConfigurationBasedChannelManager.cs) | new ] NServiceBus.Gateway.Receiving.ConfigurationBasedChannelManager  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Receiving/ConventionBasedChannelManager.cs) | new ] NServiceBus.Gateway.Receiving.ConventionBasedChannelManager  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Receiving/GatewayReceiver.cs) | new ] NServiceBus.Gateway.Receiving.GatewayReceiver  
[ old | new ] NServiceBus.Gateway.Receiving.IManageReceiveChannels  
[ old | new ] NServiceBus.Gateway.Receiving.IReceiveMessagesFromSites  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Receiving/SingleCallChannelReceiver.cs) | new ] NServiceBus.Gateway.Receiving.SingleCallChannelReceiver  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Routing/Endpoints/DefaultEndpointRouter.cs) | new ] NServiceBus.Gateway.Routing.Endpoints.DefaultEndpointRouter  
[ old | new ] NServiceBus.Gateway.Routing.IRouteMessagesToEndpoints  
[ old | new ] NServiceBus.Gateway.Routing.IRouteMessagesToSites  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Routing/Site.cs) | new ] NServiceBus.Gateway.Routing.Site  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Routing/Sites/ConfigurationBasedSiteRouter.cs) | new ] NServiceBus.Gateway.Routing.Sites.ConfigurationBasedSiteRouter  
[ old | new ] NServiceBus.Gateway.Routing.Sites.KeyPrefixConventionSiteRouter  
[ old | new ] NServiceBus.Gateway.Routing.Sites.OriginatingSiteHeaderRouter  
[ old | new ] NServiceBus.Gateway.Sending.CallInfo  
[ old | new ] NServiceBus.Gateway.Sending.CallType  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Sending/GatewaySender.cs) | new ] NServiceBus.Gateway.Sending.GatewaySender  
[ old | new ] NServiceBus.Gateway.Sending.IForwardMessagesToSites  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Sending/SingleCallChannelForwarder.cs) | new ] NServiceBus.Gateway.Sending.SingleCallChannelForwarder  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Gateway/Utils/Hasher.cs) | new ] NServiceBus.Gateway.Utils.Hasher  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Hosting/Configuration/ConfigurationManager.cs) | new ] NServiceBus.Hosting.Configuration.ConfigManager  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Installation/GatewayHttpListenerInstaller.cs) | new ] NServiceBus.Installation.GatewayHttpListenerInstaller  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs) | new ] NServiceBus.Management.Retries.SecondLevelRetries  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/ObjectBuilder/Common/Config/ConfigureCommon.cs) | new ] NServiceBus.ObjectBuilder.Common.Config.ConfigureCommon  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/InMemory/InMemoryPersistence.cs) | new ] NServiceBus.Persistence.InMemory.InMemoryPersistence  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/InMemory/SagaPersister/InMemorySagaPersister.cs) | new ] NServiceBus.Persistence.InMemory.SagaPersister.InMemorySagaPersister  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/InMemory/SubscriptionStorage/InMemorySubscriptionStorage.cs) | new ] NServiceBus.Persistence.InMemory.SubscriptionStorage.InMemorySubscriptionStorage  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/InMemory/TimeoutPersister/InMemoryTimeoutPersistence.cs) | new ] NServiceBus.Persistence.InMemory.TimeoutPersister.InMemoryTimeoutPersistence  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Msmq/SubscriptionStorage/Config/SubscriptionsQueueCreator.cs) | new ] NServiceBus.Persistence.Msmq.SubscriptionStorage.Config.SubscriptionsQueueCreator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Msmq/SubscriptionStorage/MsmqSubscriptionStorage.cs) | new ] NServiceBus.Persistence.Msmq.SubscriptionStorage.MsmqSubscriptionStorage  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/RavenConventions.cs) | new ] NServiceBus.Persistence.Raven.RavenConventions  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/RavenPersistenceConstants.cs) | new ] NServiceBus.Persistence.Raven.RavenPersistenceConstants  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/RavenSessionFactory.cs) | new ] NServiceBus.Persistence.Raven.RavenSessionFactory  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/RavenUnitofWork.cs) | new ] NServiceBus.Persistence.Raven.RavenUnitOfWork  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/RavenUserInstaller.cs) | new ] NServiceBus.Persistence.Raven.RavenUserInstaller  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/SagaPersister/RavenSagaPersister.cs) | new ] NServiceBus.Persistence.Raven.SagaPersister.RavenSagaPersister  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/SagaPersister/RavenSagaPersister.cs) | new ] NServiceBus.Persistence.Raven.SagaPersister.SagaUniqueIdentity  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/StoreAccessor.cs) | new ] NServiceBus.Persistence.Raven.StoreAccessor  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/SubscriptionStorage/Subscription.cs) | new ] NServiceBus.Persistence.Raven.SubscriptionStorage.MessageTypeConverter  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/SubscriptionStorage/RavenSubscriptionStorage.cs) | new ] NServiceBus.Persistence.Raven.SubscriptionStorage.RavenSubscriptionStorage  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/SubscriptionStorage/Subscription.cs) | new ] NServiceBus.Persistence.Raven.SubscriptionStorage.Subscription  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/Raven/TimeoutPersister/RavenTimeoutPersistence.cs) | new ] NServiceBus.Persistence.Raven.TimeoutPersister.RavenTimeoutPersistence  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Persistence/SetupDefaultPersistence.cs) | new ] NServiceBus.Persistence.SetupDefaultPersistence  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/ConfigureTimeoutAsSystemMessages.cs) | new ] NServiceBus.Sagas.ConfigureTimeoutAsSystemMessages  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Scheduling/Configuration/ConfigureScheduledTaskAsSystemMessages.cs) | new ] NServiceBus.Scheduling.Configuration.ConfigureScheduledTaskAsSystemMessages  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Scheduling/Configuration/SchedulerConfiguration.cs) | new ] NServiceBus.Scheduling.Configuration.SchedulerConfiguration  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Scheduling/InMemoryScheduledTaskStorage.cs) | new ] NServiceBus.Scheduling.InMemoryScheduledTaskStorage  
[ old | new ] NServiceBus.Scheduling.IScheduledTaskStorage  
[ old | new ] NServiceBus.Scheduling.IScheduler  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Settings/Endpoint.cs) | new ] NServiceBus.Settings.Endpoint  
[ old | new ] NServiceBus.Settings.ISetDefaultSettings  
[ old | new ] NServiceBus.Settings.TransportSettings  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/SyncConfig.cs) | new ] NServiceBus.SyncConfig  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Timeout/Core/TimeoutManagerDefaults.cs) | new ] NServiceBus.Timeout.Core.TimeoutManagerDefaults  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Transports/Msmq/Config/CheckMachineNameForComplianceWithDTCLimitation.cs) | new ] NServiceBus.Transports.Msmq.Config.CheckMachineNameForComplianceWithDtcLimitation  
[ old | new ] NServiceBus.Unicast.BusAsyncResultEventArgs  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Config/DefaultToTimeoutManagerBasedDeferal.cs) | new ] NServiceBus.Unicast.Config.DefaultToTimeoutManagerBasedDeferal  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Config/DefaultTransportForHost.cs) | new ] NServiceBus.Unicast.Config.DefaultTransportForHost  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/DefaultDispatcherFactory.cs) | new ] NServiceBus.Unicast.DefaultDispatcherFactory  
[ old | new ] NServiceBus.Unicast.IMessageDispatcherFactory  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/EnableMessageDrivenPublisherIfStorageIsFound.cs) | new ] NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.EnableMessageDrivenPublisherIfStorageIsFound  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/MessageDrivenSubscriptionManager.cs) | new ] NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.MessageDrivenSubscriptionManager  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/SubcriberSideFiltering/FilteringMutator.cs) | new ] NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.SubcriberSideFiltering.FilteringMutator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/SubcriberSideFiltering/SubscriptionPredicatesEvaluator.cs) | new ] NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.SubcriberSideFiltering.SubscriptionPredicatesEvaluator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/Config/Bootstrapper.cs) | new ] NServiceBus.Unicast.Transport.Config.Bootstrapper  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/Config/AdvancedTransactionalConfig.cs) | new ] NServiceBus.Unicast.Transport.Transactional.Config.AdvancedTransactionalConfig  

####The following types changed visibility in the new API.

[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/CircuitBreakers/CircuitBreaker.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/CircuitBreakers/CircuitBreaker.cs) ] NServiceBus.CircuitBreakers.CircuitBreaker  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Config/SatelliteConfigurer.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Config/SatelliteConfigurer.cs) ] NServiceBus.Config.SatelliteConfigurer  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/DataBus/DefaultDatabusSerializer.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/DataBus/DefaultDatabusSerializer.cs) ] NServiceBus.DataBus.DefaultDataBusSerializer  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/DataBus/FileShare/FileShareDataBus.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/DataBus/FileShare/FileShareDataBus.cs) ] NServiceBus.DataBus.FileShare.FileShareDataBus  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Encryption/EncryptionMessageMutator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Encryption/EncryptionMessageMutator.cs) ] NServiceBus.Encryption.EncryptionMessageMutator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Faults/Forwarder/Config/FaultsQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Faults/Forwarder/Config/FaultsQueueCreator.cs) ] NServiceBus.Faults.Forwarder.Config.FaultsQueueCreator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Faults/Forwarder/FaultManager.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Faults/Forwarder/FaultManager.cs) ] NServiceBus.Faults.Forwarder.FaultManager  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Faults/InMemory/FaultManager.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Faults/InMemory/FaultManager.cs) ] NServiceBus.Faults.InMemory.FaultManager  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Hosting/Wcf/WcfServiceHost.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Hosting/Wcf/WcfServiceHost.cs) ] NServiceBus.Hosting.Wcf.WcfServiceHost  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Impersonation/Windows/WindowsIdentityEnricher.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Impersonation/Windows/WindowsIdentityEnricher.cs) ] NServiceBus.Impersonation.Windows.WindowsIdentityEnricher  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Installation/PerformanceMonitorUsersInstaller.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Monitoring/PerformanceMonitorUsersInstaller.cs) ] NServiceBus.Installation.PerformanceMonitorUsersInstaller  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/ObjectBuilder/Common/CommonObjectBuilder.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/ObjectBuilder/Common/CommonObjectBuilder.cs) ] NServiceBus.ObjectBuilder.Common.CommonObjectBuilder  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/ObjectBuilder/Common/SynchronizedInvoker.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/ObjectBuilder/Common/SynchronizedInvoker.cs) ] NServiceBus.ObjectBuilder.Common.SynchronizedInvoker  
[ old | new ] NServiceBus.Persistence.Msmq.SubscriptionStorage.Entry  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/ConfigureHowToFindSagaWithMessageDispatcher.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/ConfigureHowToFindSagaWithMessageDispatcher.cs) ] NServiceBus.Sagas.ConfigureHowToFindSagaWithMessageDispatcher  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Finders/HeaderSagaIdFinder.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Finders/HeaderSagaIdFinder.cs) ] NServiceBus.Sagas.Finders.HeaderSagaIdFinder&lt;T&gt;  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Finders/PropertySagaFinder.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Finders/PropertySagaFinder.cs) ] NServiceBus.Sagas.Finders.PropertySagaFinder&lt;TSaga, TMessage&gt;  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Satellites/Config/SatelliteContext.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Satellites/Config/SatelliteContext.cs) ] NServiceBus.Satellites.Config.SatelliteContext  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Satellites/SatelliteLauncher.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Satellites/SatelliteLauncher.cs) ] NServiceBus.Satellites.SatelliteLauncher  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Satellites/SatellitesQueuesCreator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Satellites/SatellitesQueuesCreator.cs) ] NServiceBus.Satellites.SatellitesQueuesCreator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Scheduling/DefaultScheduler.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Scheduling/DefaultScheduler.cs) ] NServiceBus.Scheduling.DefaultScheduler  
[ old | new ] NServiceBus.Scheduling.Messages.ScheduledTask  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Scheduling/ScheduledTaskMessageHandler.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Scheduling/ScheduledTaskMessageHandler.cs) ] NServiceBus.Scheduling.ScheduledTaskMessageHandler  
[ old | new ] NServiceBus.SecondLevelRetries.Helpers.SecondLevelRetriesHeaders  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/SecondLevelRetries/Helpers/TransportMessageHelpers.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/SecondLevelRetries/Helpers/TransportMessageHelpers.cs) ] NServiceBus.SecondLevelRetries.Helpers.TransportMessageHelpers  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetriesProcessor.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetriesProcessor.cs) ] NServiceBus.SecondLevelRetries.SecondLevelRetriesProcessor  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Serializers/Json/Internal/MessageContractResolver.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Serializers/Json/Internal/MessageContractResolver.cs) ] NServiceBus.Serializers.Json.Internal.MessageContractResolver  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Serializers/Json/Internal/MessageSerializationBinder.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Serializers/Json/Internal/MessageSerializationBinder.cs) ] NServiceBus.Serializers.Json.Internal.MessageSerializationBinder  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Serializers/Json/Internal/XContainerConverter.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Serializers/Json/Internal/XContainerConverter.cs) ] NServiceBus.Serializers.Json.Internal.XContainerConverter  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Serializers/XML/Config/MessageTypesInitializer.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Serializers/XML/Config/MessageTypesInitializer.cs) ] NServiceBus.Serializers.XML.Config.MessageTypesInitializer  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Serializers/XML/XmlSanitizingStream.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Serializers/XML/XmlSanitizingStream.cs) ] NServiceBus.Serializers.XML.XmlSanitizingStream  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/ServiceAsyncResult.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/ServiceAsyncResult.cs) ] NServiceBus.ServiceAsyncResult  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Timeout/Core/DefaultTimeoutManager.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/Core/DefaultTimeoutManager.cs) ] NServiceBus.Timeout.Core.DefaultTimeoutManager  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutDispatcherProcessor.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutDispatcherProcessor.cs) ] NServiceBus.Timeout.Hosting.Windows.TimeoutDispatcherProcessor  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutMessageProcessor.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutMessageProcessor.cs) ] NServiceBus.Timeout.Hosting.Windows.TimeoutMessageProcessor  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutPersisterReceiver.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/Hosting/Windows/TimeoutPersisterReceiver.cs) ] NServiceBus.Timeout.Hosting.Windows.TimeoutPersisterReceiver  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Timeout/Core/TimeoutManagerDeferrer.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/Core/TimeoutManagerDeferrer.cs) ] NServiceBus.Timeout.TimeoutManagerDeferrer  
[ old | new ] NServiceBus.Timeout.TimeoutManagerHeaders  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Transports/Msmq/MsmqDequeueStrategy.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Transports/Msmq/MsmqDequeueStrategy.cs) ] NServiceBus.Transports.Msmq.MsmqDequeueStrategy  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Transports/Msmq/MsmqMessageSender.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Transports/Msmq/MsmqMessageSender.cs) ] NServiceBus.Transports.Msmq.MsmqMessageSender  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Transports/Msmq/MsmqQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Transports/Msmq/MsmqQueueCreator.cs) ] NServiceBus.Transports.Msmq.MsmqQueueCreator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/MessagingBestPractices.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/MessagingBestPractices.cs) ] NServiceBus.Unicast.MessagingBestPractices  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Monitoring/CausationMutator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Monitoring/CausationMutator.cs) ] NServiceBus.Unicast.Monitoring.CausationMutator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Monitoring/CriticalTimeCalculator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Monitoring/CriticalTimeCalculator.cs) ] NServiceBus.Unicast.Monitoring.CriticalTimeCalculator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Monitoring/EstimatedTimeToSLABreachCalculator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Monitoring/EstimatedTimeToSLABreachCalculator.cs) ] NServiceBus.Unicast.Monitoring.EstimatedTimeToSLABreachCalculator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Monitoring/PerformanceCounterInitializer.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Monitoring/PerformanceCounterInitializer.cs) ] NServiceBus.Unicast.Monitoring.PerformanceCounterInitializer  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Monitoring/ProcessingStatistics.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Monitoring/ProcessingStatistics.cs) ] NServiceBus.Unicast.Monitoring.ProcessingStatistics  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Publishing/StorageDrivenPublisher.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Publishing/StorageDrivenPublisher.cs) ] NServiceBus.Unicast.Publishing.StorageDrivenPublisher  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Queuing/Installers/AuditQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Queuing/Installers/AuditQueueCreator.cs) ] NServiceBus.Unicast.Queuing.Installers.AuditQueueCreator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Queuing/Installers/EndpointInputQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Queuing/Installers/EndpointInputQueueCreator.cs) ] NServiceBus.Unicast.Queuing.Installers.EndpointInputQueueCreator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Queuing/Installers/ForwardReceivedMessagesToQueueCreator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Queuing/Installers/ForwardReceivedMessagesToQueueCreator.cs) ] NServiceBus.Unicast.Queuing.Installers.ForwardReceivedMessagesToQueueCreator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Queuing/QueuesCreator.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Queuing/QueuesCreator.cs) ] NServiceBus.Unicast.Queuing.QueuesCreator  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/NoopSubscriptionAuthorizer.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Subscriptions/MessageDrivenSubscriptions/NoopSubscriptionAuthorizer.cs) ] NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.NoopSubscriptionAuthorizer  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/TransportConnectionString.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransportConnectionString.cs) ] NServiceBus.Unicast.Transport.TransportConnectionString  
[ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/ControlMessage.cs) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransportMessageExtensions.cs) ] NServiceBus.Unicast.Transport.TransportMessageExtensions  

####The following members are missing on the public types.

- NServiceBus.AutomaticSubscriptions.Config.AutoSubscribeSettings
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs#L45) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/AutomaticSubscriptions/Config/AutoSubscribeSettings.cs) ] NServiceBus.AutomaticSubscriptions.Config.AutoSubscribeSettings CustomAutoSubscriptionStrategy&lt;T&gt;()

- NServiceBus.Configure
  - System.Func&lt;string&gt; DefineEndpointVersionRetriever
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs) ] void add_ConfigurationComplete(System.Action)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Configure.cs#L81) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs#L60) ] bool BuilderIsConfigured()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Configure.cs#L335) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure.cs#L201) ] void Initialize()
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs) ] void remove_ConfigurationComplete(System.Action)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Configure.cs#L182) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs) ] void ScaleOut(System.Action&lt;NServiceBus.Settings.ScaleOutSettings&gt;)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Configure.cs#L47) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs#L43) ] bool WithHasBeenCalled()
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs) ] NServiceBus.Config.ConfigurationSource.IConfigurationSource ConfigurationSource { get; set; }
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Configure.cs#L159) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs) ] NServiceBus.Settings.Endpoint Endpoint { get;  }
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Configure.cs#L496) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs) ] string EndpointName { get;  }
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Configure.cs#L165) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Configure_Obsolete.cs) ] NServiceBus.Settings.TransportSettings Transports { get;  }

- NServiceBus.Features.Audit
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Audit/Audit.cs#L39) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Audit/Audit.cs) ] bool IsEnabledByDefault { get;  }

- NServiceBus.Features.AutoSubscribe
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscribe.cs#L32) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/AutomaticSubscriptions/AutoSubscribe.cs) ] bool IsEnabledByDefault { get;  }

- NServiceBus.Features.Feature
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L140) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] System.Collections.Generic.IEnumerable&lt;NServiceBus.Features.Feature&gt; ByCategory(NServiceBus.Features.FeatureCategory)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L91) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] void Disable&lt;T&gt;()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L99) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] void Disable(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L107) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] void DisableByDefault(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L57) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] void Enable&lt;T&gt;()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L65) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] void Enable(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L81) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] void EnableByDefault(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L73) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs#L110) ] void EnableByDefault&lt;T&gt;()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L17) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] void Initialize()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L115) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] bool IsEnabled&lt;T&gt;()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L124) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] bool IsEnabled(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L200) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] bool op_Equality(NServiceBus.Features.Feature, NServiceBus.Features.Feature)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L210) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] bool op_Inequality(NServiceBus.Features.Feature, NServiceBus.Features.Feature)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L24) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] bool ShouldBeEnabled()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L132) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] NServiceBus.Features.FeatureCategory Category { get;  }
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Features/Feature.cs#L48) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Features/Feature.cs) ] bool Enabled { get;  }

- NServiceBus.Features.Sagas
  - System.Collections.Generic.IDictionary&lt;System.Type, System.Collections.Generic.IDictionary&lt;System.Type, System.Collections.Generic.KeyValuePair&lt;System.Reflection.PropertyInfo, System.Reflection.PropertyInfo&gt;&gt;&gt; SagaEntityToMessageToPropertyLookup
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L329) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs#L263) ] void ConfigureFinder(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L300) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs#L240) ] void ConfigureSaga(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L35) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs) ] bool FindAndConfigureSagasIn(System.Collections.Generic.IEnumerable&lt;System.Type&gt;)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L192) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs#L153) ] System.Reflection.MethodInfo GetFindByMethodForFinder(NServiceBus.Saga.IFinder, object)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L242) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs#L182) ] System.Collections.Generic.IEnumerable&lt;System.Type&gt; GetFindersForMessageAndEntity(System.Type, System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L275) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs#L215) ] System.Collections.Generic.IEnumerable&lt;System.Type&gt; GetSagaDataTypes()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L181) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs#L142) ] System.Type GetSagaEntityTypeForSagaType(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L170) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs#L131) ] System.Type GetSagaTypeForSagaEntityType(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L280) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs#L220) ] bool IsSagaType(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Sagas/Sagas.cs#L118) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Sagas/Sagas.cs) ] bool ShouldMessageStartSaga(System.Type, System.Type)

- NServiceBus.Features.SecondLevelRetries
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs#L48) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/SecondLevelRetries/SecondLevelRetries.cs) ] bool IsEnabledByDefault { get;  }

- NServiceBus.Features.TimeoutManager
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/TimeoutManager.cs) ] NServiceBus.Address DispatcherAddress { get; set; }
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/TimeoutManager.cs) ] NServiceBus.Address InputAddress { get; set; }
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Timeout/TimeoutManager.cs#L15) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/TimeoutManager.cs) ] bool IsEnabledByDefault { get;  }

- NServiceBus.MessageInterfaces.MessageMapper.Reflection.MessageMapper
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L148) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L154) ] System.Type CreateTypeFrom(System.Type, System.Reflection.Emit.ModuleBuilder)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L139) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L145) ] string GetNewTypeName(System.Type)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L49) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/MessageInterfaces/MessageMapper/Reflection/MessageMapper.cs#L49) ] void InitType(System.Type, System.Reflection.Emit.ModuleBuilder)

- NServiceBus.MonitoringConfig
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/MonitoringConfig.cs#L20) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/MonitoringConfig.cs) ] System.TimeSpan EndpointSLA(NServiceBus.Configure)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/MonitoringConfig.cs#L39) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/MonitoringConfig.cs) ] bool PerformanceCountersEnabled(NServiceBus.Configure)

- NServiceBus.ObjectBuilder.IConfigureComponents
  - [ old | new ] bool HasComponent(System.Type)

- NServiceBus.Serializers.Json.JsonMessageSerializer
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Serializers/Json/JsonMessageSerializer.cs#L39) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Serializers/Json/JsonMessageSerializer.cs) ] object DeserializeObject(string, System.Type)

- NServiceBus.Serializers.XML.XmlMessageSerializer
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Serializers/XML/XmlMessageSerializer.cs) ] bool SkipWrappingElementForSingleMessages { get; set; }

- NServiceBus.Settings.SettingsHolder
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Settings/SettingsHolder.cs#L178) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Settings/SettingsHolder.cs) ] void ApplyTo&lt;T&gt;()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Settings/SettingsHolder.cs#L163) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Settings/SettingsHolder.cs#L230) ] void PreventChanges()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Settings/SettingsHolder.cs#L113) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Settings/SettingsHolder.cs) ] void Reset()

- NServiceBus.Timeout.Core.TimeoutData
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Timeout/Core/TimeoutData.cs) ] string CorrelationId { get; set; }

- NServiceBus.Transports.Msmq.MsmqUnitOfWork
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs#L29) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs#L43) ] void ClearTransaction()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs#L19) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Transports/Msmq/MsmqUnitOfWork.cs#L29) ] void SetTransaction(System.Messaging.MessageQueueTransaction)

- NServiceBus.Unicast.Queuing.IWantQueueCreated
  - [ old | new ] bool IsDisabled { get;  }

- NServiceBus.Unicast.Transport.ITransport
  - [ old | new ] void AbortHandlingCurrentMessage()
  - [ old | new ] void add_FailedMessageProcessing(System.EventHandler&lt;NServiceBus.Unicast.Transport.FailedMessageProcessingEventArgs&gt;)
  - [ old | new ] void remove_FailedMessageProcessing(System.EventHandler&lt;NServiceBus.Unicast.Transport.FailedMessageProcessingEventArgs&gt;)
  - [ old | new ] void Start(NServiceBus.Address)
  - [ old | new ] void Stop()
  - [ old | new ] int MaximumMessageThroughputPerSecond { get;  }

- NServiceBus.Unicast.Transport.TransactionSettings
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/TransactionSettings.cs#L31) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransactionSettings.cs) ] NServiceBus.Unicast.Transport.TransactionSettings Default { get;  }
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransactionSettings.cs) ] bool DontUseDistributedTransactions { get; set; }

- NServiceBus.Unicast.Transport.TransportReceiver
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L169) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs) ] void ChangeNumberOfWorkerThreads(int)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L479) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L447) ] void DisposeManaged()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L179) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs) ] void Start(NServiceBus.Address)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L118) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs) ] int MaxThroughputPerSecond { get; set; }
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs#L62) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/Transport/TransportReceiver.cs) ] int NumberOfWorkerThreads { get;  }

- NServiceBus.Unicast.UnicastBus
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void add_MessageReceived(MessageReceivedDelegate)
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void add_MessagesSent(System.EventHandler&lt;NServiceBus.Unicast.MessagesEventArgs&gt;)
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void add_NoSubscribersForMessage(System.EventHandler&lt;NServiceBus.Unicast.MessageEventArgs&gt;)
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void add_Started(System.EventHandler)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L696) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback Defer(System.TimeSpan, object[])
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L707) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback Defer(System.DateTime, object)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L712) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback Defer(System.DateTime, object[])
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void Dispose()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L910) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void DisposeManaged()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L916) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void DoNotContinueDispatchingCurrentMessageToHandlers()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L307) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void Publish&lt;T&gt;(T[])
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L997) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void Raise&lt;T&gt;(System.Action&lt;T&gt;)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L1002) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void Raise&lt;T&gt;(T)
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void remove_MessageReceived(MessageReceivedDelegate)
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void remove_MessagesSent(System.EventHandler&lt;NServiceBus.Unicast.MessagesEventArgs&gt;)
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void remove_NoSubscribersForMessage(System.EventHandler&lt;NServiceBus.Unicast.MessageEventArgs&gt;)
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void remove_Started(System.EventHandler)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L484) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void Reply&lt;T&gt;(System.Action&lt;T&gt;)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L627) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback Send&lt;T&gt;(NServiceBus.Address, string, System.Action&lt;T&gt;)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L637) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback Send(string, string, object)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L647) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback Send(string, string, object[])
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L657) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback Send(NServiceBus.Address, string, object[])
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L667) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback Send(NServiceBus.Address, string, object)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L539) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback SendLocal(object[])
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L684) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.ICallback SendToSites(System.Collections.Generic.IEnumerable&lt;string&gt;, object[])
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L949) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void Shutdown()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L767) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.IBus Start()
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L772) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.IBus Start(System.Action)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L350) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void Subscribe&lt;T&gt;(System.Predicate&lt;T&gt;)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L371) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] void Subscribe(System.Type, System.Predicate&lt;object&gt;)
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L933) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.IMessageContext CurrentMessageContext { get;  }
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L944) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.IInMemoryOperations InMemory { get;  }
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.Address MasterNodeAddress { get; set; }
  - [ [old](https://github.com/Particular/NServiceBus/blob/4.6.3/src/NServiceBus.Core/Unicast/UnicastBus.cs#L923) | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] System.Collections.Generic.IDictionary&lt;string, string&gt; OutgoingHeaders { get;  }
  - [ old | [new](https://github.com/Particular/NServiceBus/blob/5.0.0-beta2/src/NServiceBus.Core/Unicast/UnicastBus_Obsolete.cs) ] NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.SubcriberSideFiltering.SubscriptionPredicatesEvaluator SubscriptionPredicatesEvaluator { get; set; }

