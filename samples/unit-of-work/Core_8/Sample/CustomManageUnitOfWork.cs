using System;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.UnitOfWork;

#pragma warning disable 618

#region CustomManageUnitOfWork

sealed class CustomManageUnitOfWork :
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

#pragma warning restore 618
