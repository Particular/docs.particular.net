using NServiceBus;
using NServiceBus.AuditFilter;

public class Usage
{
    void FilterAuditByAttribute(EndpointConfiguration endpointConfiguration)
    {
        #region DefaultIncludeInAudit

        endpointConfiguration.FilterAuditQueue(FilterResult.IncludeInAudit);

        #endregion
        #region DefaultExcludeFromAudit

        endpointConfiguration.FilterAuditQueue(FilterResult.ExcludeFromAudit);

        #endregion
        #region FilterAuditByDelegate

        endpointConfiguration.FilterAuditQueue(
            filter: (instance, headers) =>
            {
                if (instance is MyMessage)
                {
                    return FilterResult.ExcludeFromAudit;
                }

                return FilterResult.IncludeInAudit;
            });

        #endregion
    }
    public class MyMessage
    {

    }
}