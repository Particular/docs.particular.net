using System;
using NServiceBus;
using NServiceBus.SagaPersisters.NHibernate;

#region NHibernateConcurrencyRowVersion
public class SagaDataWithRowVersion :
    IContainSagaData
{
    [RowVersion]
    public virtual int MyVersion { get; set; }

    public virtual string OriginalMessageId { get; set; }
    public virtual string Originator { get; set; }
    public virtual Guid Id { get; set; }
}
#endregion