using System.Threading.Tasks;

namespace NServiceBus.TransactionalSession.FakeExtensions
{
    public static class OpenExtensions
    {
        // This extension method is an evil trick to allow showing the Mongo and NHibernate specific Open methods
        // Normally this is not required but we need it here because all snippets from all persisters are in the same project
        public static Task Open(this ITransactionalSession session)
        {
            return Task.CompletedTask;
        }
    }
}