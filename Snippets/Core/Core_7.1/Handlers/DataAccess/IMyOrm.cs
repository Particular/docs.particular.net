namespace Core7.Handlers.DataAccess
{
    public interface IMyOrm
    {
        IMyOrmSession OpenSession();
    }
}