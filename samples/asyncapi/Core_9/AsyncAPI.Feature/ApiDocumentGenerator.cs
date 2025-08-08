#nullable enable
using Json.Schema;
using Json.Schema.Generation;
using Microsoft.Extensions.DependencyInjection;
using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.FluentBuilders.v3;
using Neuroglia.AsyncApi.Generation;
using Neuroglia.AsyncApi.v3;

namespace Infrastructure;

public class ApiDocumentGenerator : IAsyncApiDocumentGenerator
{
    private IServiceProvider serviceProvider;

    public ApiDocumentGenerator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<IAsyncApiDocument>> GenerateAsync(IEnumerable<Type> markupTypes, AsyncApiDocumentGenerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        //only creating one document for the one NSB endpoint
        var documents = new List<IAsyncApiDocument>(1);

        var document = serviceProvider.GetRequiredService<IV3AsyncApiDocumentBuilder>();
        options.V3BuilderSetup?.Invoke(document);

        await GenerateChannels(document, options, cancellationToken);

        documents.Add(document.Build());

        return documents;
    }

    protected async Task GenerateChannels(IV3AsyncApiDocumentBuilder document, AsyncApiDocumentGenerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(options);
        IV3ChannelDefinitionBuilder channelBuilder = null!;
        
        var typeCache = serviceProvider.GetRequiredService<TypeCache>();

        //get all published events
        //TODO should all published events be coming from the one channel, ie this endpoint name?
        foreach (var (actualType, publishedType) in typeCache.PublishedEventCache.Select(kvp => (kvp.Key, kvp.Value)))
        {
            var channelName =$"{publishedType.FullName!}";
            document.WithChannel(channelName, channel =>
            {
                channelBuilder = channel;
                channel
                    //.WithAddress() //Can this somehow be the endpointname address thats publishing the message
                    .WithDescription(actualType.FullName);
            });
            await GenerateV3OperationForAsync(document, channelName, channelBuilder, actualType, publishedType, options, cancellationToken);
        }

        ////get all subscribed events??
        //foreach (var (actualType, subscribedType) in typeCache.SubscribedEventCache.Select(kvp => (kvp.Value.ActualType, kvp.Value.SubscribedType)))
        //{
        //    var channelName = $"{subscribedType.FullName}";
        //    document.WithChannel(channelName, channel =>
        //    {
        //        channelBuilder = channel;
        //        channel
        //            //.WithAddress() //Can this somehow be the endpointname address thats publishing the message
        //            .WithDescription(actualType.FullName);
        //    });
        //    await GenerateV3OperationForAsync(document, channelName, channelBuilder, actualType, subscribedType, options, cancellationToken);
        //}

    }

    protected Task GenerateV3OperationForAsync(IV3AsyncApiDocumentBuilder document, string channelName, IV3ChannelDefinitionBuilder channel, Type actualType, Type producedType, AsyncApiDocumentGenerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentException.ThrowIfNullOrWhiteSpace(channelName);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(actualType);
        ArgumentNullException.ThrowIfNull(producedType);
        ArgumentNullException.ThrowIfNull(options);

        var requestMessagePayloadSchema = new JsonSchemaBuilder().FromType(actualType, JsonSchemaGeneratorConfiguration.Default).Build();
        var messageName = producedType.FullName!;

        //NOTE not sure how a component message can be used since the operation message requires a reference to a channel message
        //var messageComponentReference = $"#/components/messages/{publishedType.FullName!}";
        //document.WithMessageComponent(messageName, message =>
        //    message
        //        .WithName(messageName)
        //        .WithPayloadSchema(schema => schema
        //            .WithFormat("application/vnd.aai.asyncapi+json;version=3.0.0")
        //            .WithSchema(requestMessagePayloadSchema)));

        var messageChannelReference = $"#/channels/{channelName}/messages/{producedType.FullName!}";
        channel.WithMessage(messageName, message =>
        {
            message
                .WithName(messageName)
                .WithPayloadSchema(schema => schema
                    .WithFormat("application/vnd.aai.asyncapi+json;version=3.0.0")
                    .WithSchema(requestMessagePayloadSchema));
        });

        var operationName = $"{producedType.FullName!}";
        document.WithOperation(operationName, operation =>
        {
            operation
                .WithAction(V3OperationAction.Send)
                .WithChannel($"#/channels/{channelName}")
                .WithMessage(messageChannelReference);
        });
        return Task.CompletedTask;
    }
}