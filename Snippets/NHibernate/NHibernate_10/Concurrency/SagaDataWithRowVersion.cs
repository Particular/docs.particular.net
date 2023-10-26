using NServiceBus;
using NServiceBus.SagaPersisters.NHibernate;
using System;

#region NHibernateConcurrencyRowVersion
#pragma warning disable NSB0012 // Saga data classes should inherit ContainSagaData - [RowVersion] requires non-derived class
public class SagaDataWithRowVersion :
    IContainSagaData
{
    [RowVersion]
    public virtual int MyVersion { get; set; }

    public virtual string OriginalMessageId { get; set; }
    public virtual string Originator { get; set; }
    public virtual Guid Id { get; set; }
}
#pragma warning restore NSB0012 // Saga data classes should inherit ContainSagaData
#endregion