using Infrastructure;
//using Saunter;
//using Saunter.AsyncApiSchema.v2;
//using Saunter.Generation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddTransient<IDocumentGenerator, ApiDocumentGenerator>();
//builder.Services.AddAsyncApiSchemaGeneration(options =>
//{
//    options.AsyncApi = new AsyncApiDocument
//    {
//        Info = new Info("Publisher API", "3.0.0")
//        {
//            Description = "Some example.",
//            License = new License("Apache 2.0")
//            {
//                Url = "https://www.apache.org/licenses/LICENSE-2.0"
//            }
//        },
//        Servers = { { "amqp", new Server("sb://example.servicebus.windows.net/", "amqp") } }
//    };
//});

var endpointConfiguration = new EndpointConfiguration("Publisher");
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
//var conventions = endpointConfiguration.Conventions();
//conventions.Add(new PublishedEventsConvention());
//conventions.Add(new SubscribedEventsConvention());
endpointConfiguration.SendOnly();
endpointConfiguration.EnableAsyncApiSupport();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapAsyncApiDocuments();
    //    endpoints.MapAsyncApiUi();
    //});
}

app.Run();