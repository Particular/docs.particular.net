namespace Core5.Handlers
{
    public interface IMyOrm
    {
        IMyOrmSession OpenSession();
    }
}