using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.Generation;
using Neuroglia.AsyncApi.IO;

class AsyncAPISchemaWriter(ILogger<AsyncAPISchemaWriter> logger, IAsyncApiDocumentGenerator apiDocumentGenerator, IAsyncApiDocumentWriter asyncApiDocumentWriter) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var options = new AsyncApiDocumentGenerationOptions()
            {
                V3BuilderSetup = asyncApi =>
                {
                    //Setup V3 documents, by configuring servers, for example:
                    asyncApi.WithTitle("Generic Host Service");
                    asyncApi.WithVersion("1.0.0");
                    asyncApi.WithLicense("Apache 2.0", new Uri("https://www.apache.org/licenses/LICENSE-2.0"));

                    asyncApi.WithServer("amqp", setup =>
                    {
                        setup
                            .WithProtocol(AsyncApiProtocol.Amqp)
                            .WithHost("sb://example.servicebus.windows.net/")
                            .WithBinding(new Neuroglia.AsyncApi.Bindings.Amqp.AmqpServerBindingDefinition
                            {
                                BindingVersion = "0.1.0",
                            });
                    });
                }
            };
            #region GenericHostCustomGenerationAndWritingToDisk
            var documents = await apiDocumentGenerator.GenerateAsync(
                markupTypes: null, options, cancellationToken);

            if (documents is not null && !documents.Any())
            {
                logger.LogInformation("No documents generated.");
            }
            else
            {
                logger.LogInformation("Found #{Count} generated document(s).", documents.Count());
                foreach (var document in documents)
                {
                    using MemoryStream stream = new();

                    await asyncApiDocumentWriter.WriteAsync(
                        document, stream, AsyncApiDocumentFormat.Json, cancellationToken);

                    var schemaFile = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        "downloads",
                        $"{document.Title}.json");

                    File.WriteAllBytes(schemaFile, stream.ToArray());
                }
            }
            #endregion
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }
}