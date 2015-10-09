using System.ServiceModel;
using System.Threading.Tasks;

[ServiceContract]
public interface ICallbackService<in TRequest, TResponse>
{
    [OperationContract]
    Task<TResponse> SendRequest(TRequest request);
}