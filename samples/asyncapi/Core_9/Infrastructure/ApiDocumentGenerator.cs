#nullable enable
using System.Reflection;
using System.Threading.Channels;
using Json.Schema;
using Json.Schema.Generation;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Neuroglia;
using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.Bindings;
using Neuroglia.AsyncApi.FluentBuilders.v3;
using Neuroglia.AsyncApi.Generation;
using Neuroglia.AsyncApi.v2;
using Neuroglia.AsyncApi.v3;
using NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions;

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

        //get all published events
        var typeCache = serviceProvider.GetRequiredService<TypeCache>();

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
    }

    protected Task GenerateV3OperationForAsync(IV3AsyncApiDocumentBuilder document, string channelName, IV3ChannelDefinitionBuilder channel, Type actualType, Type publishedType, AsyncApiDocumentGenerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentException.ThrowIfNullOrWhiteSpace(channelName);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(actualType);
        ArgumentNullException.ThrowIfNull(publishedType);
        ArgumentNullException.ThrowIfNull(options);

        var requestMessagePayloadSchema = new JsonSchemaBuilder().FromType(actualType, JsonSchemaGeneratorConfiguration.Default).Build();
        var messageName = publishedType.FullName!;

        //NOTE not sure how a component message can be used since the operation message requires a reference to a channel message
        //var messageComponentReference = $"#/components/messages/{publishedType.FullName!}";
        //document.WithMessageComponent(messageName, message =>
        //    message
        //        .WithName(messageName)
        //        .WithPayloadSchema(schema => schema
        //            .WithFormat("application/vnd.aai.asyncapi+json;version=3.0.0")
        //            .WithSchema(requestMessagePayloadSchema)));

        var messageChannelReference = $"#/channels/{channelName}/messages/{publishedType.FullName!}";
        channel.WithMessage(messageName, message =>
        {
            message
                .WithName(messageName)
                .WithPayloadSchema(schema => schema
                    .WithFormat("application/vnd.aai.asyncapi+json;version=3.0.0")
                    .WithSchema(requestMessagePayloadSchema));
        });

        var operationName = $"{publishedType.FullName!}";
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