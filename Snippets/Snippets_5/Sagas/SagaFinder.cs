using System;
using System.Linq;
using NServiceBus.Persistence.NHibernate;
using NServiceBus.RavenDB.Persistence;
using NServiceBus.Saga;

public class SagaFinder
{
    #region saga-finder

    // NHibernate example:
    public class MyNHibernateSagaFinder :
        IFindSagas<MySagaData>.Using<MyMessage>
    {
        public NHibernateStorageContext StorageContext { get; set; }

        public MySagaData FindBy(MyMessage message)
        {
            //your custom finding logic here, e.g.
            return StorageContext.Session.QueryOver<MySagaData>()
                .Where(x => x.SomeID == message.SomeID && x.SomeData == message.SomeData)
                .SingleOrDefault();
        }
    }

    // RavenDb example:
    public class MyRavenDbSagaFinder :
        IFindSagas<MySagaData>.Using<MyMessage>
    {
        public ISessionProvider SessionProvider { get; set; }

        public MySagaData FindBy(MyMessage message)
        {
            //your custom finding logic here, e.g.
            return SessionProvider.Session
                .Query<MySagaData>()
                .SingleOrDefault(x => x.SomeID == message.SomeID && x.SomeData == message.SomeData);
        }
    }

    #endregion

    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
        public string SomeID { get; set; }
        public string SomeData { get; set; }
    }

    public class MyMessage
    {
        public string SomeID { get; set; }
        public string SomeData { get; set; }
    }

}