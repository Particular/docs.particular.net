using System.Linq;
using NServiceBus.Bridge;

public static class FuncUtils
{
    public static InterceptMessageForwarding Fold(params InterceptMessageForwarding[] interceptors)
    {
        return interceptors.Aggregate(interceptors[0], Fold);
    }

    static InterceptMessageForwarding Fold(InterceptMessageForwarding left, InterceptMessageForwarding right)
    {
        return (queue, message, dispatch, forward) =>
        {
            return left(queue, message, dispatch, d =>
            {
                return right(queue, message, d, forward);
            });
        };
    }
}