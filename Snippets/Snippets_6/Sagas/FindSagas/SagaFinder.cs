
namespace Snippets6.Sagas.FindSagas
{
    public class SagaFinder
    {
        #region TODOsaga-finder

        //todo: update when we have versions of NH + raven for v6 (also remove TODO from region name
        //// NHibernate example:
        //public class MyNHibernateSagaFinder :
        //    IFindSagas<MySagaData>.Using<MyMessage>
        //{
        //    public NHibernateStorageContext StorageContext { get; set; }

        //    public MySagaData FindBy(MyMessage message)
        //    {
        //        //your custom finding logic here, e.g.
        //        return StorageContext.Session.QueryOver<MySagaData>()
        //            .Where(x => x.SomeID == message.SomeID && x.SomeData == message.SomeData)
        //            .SingleOrDefault();
        //    }
        //}

        //// RavenDb example:
        //public class MyRavenDbSagaFinder :
        //    IFindSagas<MySagaData>.Using<MyMessage>
        //{
        //    public ISessionProvider SessionProvider { get; set; }

        //    public MySagaData FindBy(MyMessage message)
        //    {
        //        //your custom finding logic here, e.g.
        //        return SessionProvider.Session
        //            .Query<MySagaData>()
        //            .SingleOrDefault(x => x.SomeID == message.SomeID && x.SomeData == message.SomeData);
        //    }
        //}

        #endregion


    }
}