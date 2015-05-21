using NUnit.Framework;

namespace Operations.RavenDB.Tests
{
    using System;
    using Raven.Client.Document;

    [TestFixture]
    [Explicit]
    public class UserCreationTests
    {
        [Test]
        public void Foo()
        {
            #region raven-add-user-usage

            using (DocumentStore documentStore = new DocumentStore
            {
                Url = "http://localhost:8083/"
            })
            {
                documentStore.Initialize();
                UserCreation.AddUserToDatabase(documentStore, Environment.UserName);
            }

            #endregion
        }
    }

}