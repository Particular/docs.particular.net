
using System.Security.Principal;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;
public class InputLoopService(IMessageSession messageSession, IPrincipalAccessor principalAccessor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        #region send-message


        await Task.WhenAll(SendMessage(1, messageSession), SendMessage(2, messageSession));

        #endregion

        Console.WriteLine("Message sent!");

    }

    async Task SendMessage(int userNumber, IMessageSession messageSession)
    {
        var identity = new GenericIdentity($"FakeUser{userNumber}");
        principalAccessor.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);

        var message = new MyMessage();
        await messageSession.Send("Samples.UsernameHeader.Endpoint2", message);
    }

}
