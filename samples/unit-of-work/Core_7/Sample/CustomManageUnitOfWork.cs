using System;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.UnitOfWork;

#region CustomManageUnitOfWork

public class CustomManageUnitOfWork :
    IManageUnitsOfWork
{
    static ILog log = LogManager.GetLogger("CustomManageUnitOfWork");

    public Task Begin()
    {
        log.Info("Begin");
        return Task.CompletedTask;
    }

    public Task End(Exception exception = null)
    {
        if (exception == null)
        {
            log.Info("End Success");
        }
        else
        {
            log.Error("End Fail", exception);
        }
        return Task.CompletedTask;
    }
}

#endregion