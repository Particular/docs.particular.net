using System;
using log4net;
using NServiceBus.UnitOfWork;

#region CustomManageUnitOfWork

public class CustomManageUnitOfWork :
    IManageUnitsOfWork
{
    static ILog log = LogManager.GetLogger("CustomManageUnitOfWork");

    public void Begin()
    {
        log.Info("Begin");
    }

    public void End(Exception exception = null)
    {
        if (exception == null)
        {
            log.Info("End Success");
        }
        else
        {
            log.Error("End Fail", exception);
        }
    }
}

#endregion