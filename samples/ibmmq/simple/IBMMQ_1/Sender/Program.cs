using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.Transport.IBMMQ;

Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);

#region SenderConfig
var ibmmq = new IBMMQTransport()
{
    QueueManagerName = "QM1",
    Host = "localhost",
    Port = 1414,
    Channel = "DEV.ADMIN.SVRCONN",
    User = "admin",
    Password = "passw0rd"
};

var endpointB = new EndpointConfiguration("DEV.SENDER");//upper case by convention for queues in IBM MQ
endpointB.SendFailedMessagesTo("error");
endpointB.UseTransport(ibmmq);
endpointB.UseSerialization<SystemJsonSerializer>();
// the Sender sends messages invoking SendOnly results and no receive infrastructure will be set up
endpointB.SendOnly();

builder.UseNServiceBus(endpointB);
#endregion

var host = builder.Build();

await host.StartAsync();

var instance = host.Services.GetRequiredService<IMessageSession>();

using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

while (!cts.IsCancellationRequested)
{
    Console.Write("\nHow many message to send: ");

    var readLineTask = Task.Run(Console.ReadLine, cts.Token);
    _ = await Task.WhenAny(readLineTask, Task.Delay(Timeout.Infinite, cts.Token));

    if (cts.IsCancellationRequested)
    {
        break;
    }

    #region SendingMessage

    var message = new MyMessage(Data: "MyData");

    Console.WriteLine($"Sending message: {message}");

    await instance.Send("DEV.RECEIVER", message);

    #endregion

    Console.WriteLine("Done");
}

await host.StopAsync();