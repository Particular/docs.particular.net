using System;
using System.ServiceModel;
using System.Threading.Tasks;

#region ICallbackService
[ServiceContract]
public interface ICallbackService<in TRequest, TResponse>:
    IDisposable
{
    [OperationContract]
    Task<TResponse> SendRequest(TRequest request);
}
#endregion