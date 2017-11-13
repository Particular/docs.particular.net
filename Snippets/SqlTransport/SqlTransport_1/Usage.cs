using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region Usage

        configure.UseTransport<SqlServer>("connectionStringName");

        #endregion
    }
}