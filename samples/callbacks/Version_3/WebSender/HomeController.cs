using System.Web.Mvc;
using NServiceBus;

public class HomeController : AsyncController
{
    // GET: Home
    public ActionResult Index()
    {
        return View();
    }

    #region Web_SendEnumMessage
    public void SendEnumMessageAsync()
    {
        EnumMessage message = new EnumMessage();
        AsyncManager.OutstandingOperations.Increment();
        
        MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register<Status>(status =>
            {
                AsyncManager.Parameters["status"] = status;
                AsyncManager.OutstandingOperations.Decrement();
            }, this);
    }

    public ActionResult SendEnumMessageCompleted(Status status)
    {
        return View("SendEnumMessage", status);
    }
    #endregion

    #region Web_SendIntMessage
    public void SendIntMessageAsync()
    {
        IntMessage message = new IntMessage();
        AsyncManager.OutstandingOperations.Increment();

        MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register<int>(result =>
            {
                AsyncManager.Parameters["result"] = result;
                AsyncManager.OutstandingOperations.Decrement();
            }, this);

    }

    public ActionResult SendIntMessageCompleted(int result)
    {
        return View("SendIntMessage", result);
    }
    #endregion

    #region Web_SendObjectMessage
    public void SendObjectMessageAsync()
    {
        ObjectMessage message = new ObjectMessage();
        AsyncManager.OutstandingOperations.Increment();

        MvcApplication.Bus.Send("Samples.Callbacks.Receiver", message)
            .Register(ar =>
            {
                CompletionResult localResult = (CompletionResult)ar.AsyncState;
                ObjectResponseMessage response = (ObjectResponseMessage)localResult.Messages[0];
                AsyncManager.Parameters["response"] = response;
                AsyncManager.OutstandingOperations.Decrement();
            }, this);
    }

    public ActionResult SendObjectMessageCompleted(ObjectResponseMessage response)
    {
        return View("SendObjectMessage", response);
    }
    #endregion
}
