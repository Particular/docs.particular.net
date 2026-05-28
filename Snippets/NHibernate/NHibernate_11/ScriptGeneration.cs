using System;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode.Conformist;
using NServiceBus;
using NServiceBus.NHibernate;

namespace NHibernate_11;

class ScriptGeneration
{
    static void GenerateOutboxScript()
    {
        #region GenerateOutboxScript

        var outboxScript = ScriptGenerator<MsSql2012Dialect>.GenerateOutboxScript();

        #endregion

        Console.WriteLine(outboxScript);
    }

    static void GenerateSagaScript()
    {
        #region GenerateSagaScript

        var sagaScript = ScriptGenerator<MsSql2012Dialect>.GenerateSagaScript<ExampleSaga>();

        #endregion

        Console.WriteLine(sagaScript);
    }

    static void GenerateScript()
    {
        #region GenerateScript

        var entityScript = ScriptGenerator<MsSql2012Dialect>.GenerateScript(typeof(ExampleEntity));

        #endregion

        Console.WriteLine(entityScript);
    }

    [Saga]
    class ExampleSaga : Saga<ExampleSagaData>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ExampleSagaData> mapper)
        {
        }
    }

    public class ExampleSagaData : ContainSagaData
    {
    }

    class EntityMapping : ClassMapping<ExampleEntity>
    {
    }

    class ExampleEntity
    {
    }
}