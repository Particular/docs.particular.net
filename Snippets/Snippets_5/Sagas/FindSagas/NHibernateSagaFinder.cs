namespace Snippets5.Sagas.FindSagas
{
    using NServiceBus.Persistence.NHibernate;
    using NServiceBus.Saga;

    public class NHibernateSagaFinder
    {
        #region nhiebarenate-saga-finder

        public class MyNHibernateSagaFinder :
            IFindSagas<MySagaData>.Using<MyMessage>
        {
            public NHibernateStorageContext StorageContext { get; set; }

            public MySagaData FindBy(MyMessage message)
            {
                //your custom finding logic here, e.g.
                return StorageContext.Session.QueryOver<MySagaData>()
                    .Where(x =>
                        x.SomeID == message.SomeID &&
                        x.SomeData == message.SomeData)
                    .SingleOrDefault();
            }
        }

        #endregion

    }
}