namespace Core6.Handlers.DataAccess
{
    public interface IMyOrm
    {
        IMyOrmSession OpenSession();
    }
}