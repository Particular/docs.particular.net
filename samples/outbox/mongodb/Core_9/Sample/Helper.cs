using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
public static class Helper
{
    public static Task SendDuplicates<TMessage>(IMessageSession context, TMessage message, int totalCount)
    {
        var duplicatedMessageId = Guid.NewGuid().ToString();

        var tasks = Enumerable.Range(0, totalCount)
            .Select(i =>
            {
                var options = new SendOptions();
                options.RouteToThisEndpoint();
                options.SetMessageId(duplicatedMessageId);

                return context.Send(message, options);
            });

        return Task.WhenAll(tasks);
    }
}