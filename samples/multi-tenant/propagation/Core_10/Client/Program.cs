const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
var random = new Random();
Console.Title = "Client";

var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Propagation.Client");
endpointConfiguration.SendOnly();

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var routing = endpointConfiguration.UseTransport(new LearningTransport());
routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.MultiTenant.Propagation.Sales");

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press <enter> to send messages");
while (true)
{
    Console.ReadLine();
    var tenantId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());

    #region SetTenantId

    var options = new SendOptions();
    options.SetHeader("tenant_id", tenantId);
    await endpointInstance.Send(new PlaceOrder(), options);

    #endregion

    Console.WriteLine($"Placed order for tenant {tenantId}");
}