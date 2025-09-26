#nullable enable
using Json.Schema;
using Json.Schema.Generation;
using Microsoft.Extensions.DependencyInjection;
using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.FluentBuilders.v3;
using Neuroglia.AsyncApi.Generation;
using Neuroglia.AsyncApi.v3;

namespace AsyncAPI.Feature;

public class ApiDocumentGenerator(IServiceProvider serviceProvider) : IAsyncApiDocumentGenerator
{
    public Task<IEnumerable<IAsyncApiDocument>> GenerateAsync(IEnumerable<Type> markupTypes, AsyncApiDocumentGenerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        //only creating one document for the one NSB endpoint
        var documents = new List<IAsyncApiDocument>(1);

        var document = serviceProvider.GetRequiredService<IV3AsyncApiDocumentBuilder>();
        options.V3BuilderSetup?.Invoke(document);

        GenerateChannels(document, options);

        documents.Add(document.Build());

        return Task.FromResult<IEnumerable<IAsyncApiDocument>>(documents);
    }

    void GenerateChannels(IV3AsyncApiDocumentBuilder document, AsyncApiDocumentGenerationOptions options)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(options);
        IV3ChannelDefinitionBuilder channelBuilder = null!;

        var typeCache = serviceProvider.GetRequiredService<TypeCache>();

        //get all published events
        foreach (var (actualType, publishedType) in typeCache.PublishedEventCache)
        {
            var channelName = publishedType.FullName!;
            document.WithChannel(channelName, channel =>
            {
                channelBuilder = channel;
                channel
                    .WithAddress(typeCache.EndpointName)
                    .WithDescription(actualType.FullName);
            });
            GenerateV3OperationFor(document, channelName, channelBuilder, actualType, publishedType, options);
        }

        //NOTE this is where more channels and operations can be defined, for example subscribed to events, sent/received commands and messages
    }

    static void GenerateV3OperationFor(IV3AsyncApiDocumentBuilder document, string channelName, IV3ChannelDefinitionBuilder channel, Type actualType, Type producedType, AsyncApiDocumentGenerationOptions options)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentException.ThrowIfNullOrWhiteSpace(channelName);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(actualType);
        ArgumentNullException.ThrowIfNull(producedType);
        ArgumentNullException.ThrowIfNull(options);

        var requestMessagePayloadSchema = new JsonSchemaBuilder().FromType(actualType, JsonSchemaGeneratorConfiguration.Default).Build();
        var messageName = producedType.FullName!;

        var messageChannelReference = $"#/channels/{channelName}/messages/{producedType.FullName!}";
        channel.WithMessage(messageName, message =>
        {
            message
                .WithName(messageName)
                .WithPayloadSchema(schema => schema
                    .WithFormat("application/vnd.aai.asyncapi+json;version=3.0.0")
                    .WithSchema(requestMessagePayloadSchema));
        });

        var operationName = producedType.FullName!;
        document.WithOperation(operationName, operation =>
        {
            operation
                .WithAction(V3OperationAction.Send)
                .WithChannel($"#/channels/{channelName}")
                .WithMessage(messageChannelReference);
        });
    }
}