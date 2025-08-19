using Autofac;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

#region ServiceProviderFactoryAutofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
#endregion

#region ContainerConfigurationAutofac
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterType<MyService>().SingleInstance());
#endregion

var endpointConfiguration = new EndpointConfiguration("Sample.Core");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

app.MapGet("/", context =>
{
    var endpointInstance = context.RequestServices.GetService<IMessageSession>();
    var myMessage = new MyMessage();

    return Task.WhenAll(
        endpointInstance.SendLocal(myMessage),
        context.Response.WriteAsync("Message sent"));
});

app.Run();
