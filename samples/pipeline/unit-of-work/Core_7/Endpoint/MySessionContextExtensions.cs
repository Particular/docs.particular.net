using System.Threading.Tasks;
using NServiceBus;


#region session-context-extensions
static class MySessionContextExtensions
{
    public static IMySession GetSession(this IMessageHandlerContext context)
    {
        return context.Extensions.Get<IMySession>();
    }

    public static Task Store<T>(this IMessageHandlerContext context, T entity)
    {
        return context.GetSession().Store(entity);
    }
}
#endregion