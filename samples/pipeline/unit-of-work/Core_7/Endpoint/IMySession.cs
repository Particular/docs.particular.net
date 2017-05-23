using System.Threading.Tasks;

interface IMySession
{
    Task Store<T>(T entity);
}