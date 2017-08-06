namespace Core5.Sagas.FindSagas
{
    using System;

    public class DbSessionProvider
    {
        public DbSession GetDbSession()
        {
            throw new NotImplementedException();
        }
    }

    public class DbSession
    {
        public MySagaData GetSagaFromDB(string messageSomeId, string messageSomeData)
        {
            throw new NotImplementedException();
        }
    }
}