using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus.Bridge;
using NServiceBus.Transport;

public class Duplicator
{
    public static Task DuplicateRabbitMQMessages(string queue, MessageContext message, Dispatch dispatch, Func<Dispatch, Task> forward)
    {
        #region Duplicate

        var duplicate = message.TransportTransaction.TryGet<SqlConnection>(out var _);

        return forward(async (messages, transaction, context) =>
        {
            if (duplicate)
            {
                await dispatch(messages, transaction, context).ConfigureAwait(false);
            }
            await dispatch(messages, transaction, context).ConfigureAwait(false);
        });

        #endregion
    }
}