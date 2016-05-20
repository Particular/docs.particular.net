﻿namespace Testing_5.Saga
{
    using System;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    public class Tests
    {
        #region TestingSaga
        [Test]
        public void Run()
        {
            Test.Initialize();
            Test.Saga<MySaga>()
                    .ExpectReplyToOriginator<MyResponse>() // In version 4 the typo in Originator was fixed.
                    .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
                    .ExpectPublish<MyEvent>()
                    .ExpectSend<MyCommand>()
                .When(s => s.Handle(new StartsSaga()))
                    .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                    .AssertSagaCompletionIs(true);
        }
        #endregion
    }
}