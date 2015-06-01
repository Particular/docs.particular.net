using System;
using System.IO;
using NServiceBus;
using NServiceBus.Persistence;
using UnitTesting.SagaTest;


class NHibernateCustomMappings
{
    #region NHibernateLoadMappingsFromFileSystem

    static void AddMappingsFromFilesystem(NHibernate.Cfg.Configuration nhConfiguration)
    {
        var folder = Directory.GetCurrentDirectory();
        var hmbFiles = Directory.GetFiles(folder, "*.hbm.xml", SearchOption.TopDirectoryOnly);

        foreach (var file in hmbFiles)
        {
            nhConfiguration.AddFile(file);
        }
    }

    #endregion

    #region NHibernateInitWithFluentNHibernate

    static NHibernate.Cfg.Configuration BuildConfiguration(NHibernate.Cfg.Configuration nhConfiguration)
    {
        return FluentNHibernate.Cfg.Fluently.Configure(nhConfiguration)
            .Mappings(cfg =>
            {
                cfg.FluentMappings.AddFromAssemblyOf<MySagaData>();
            }).BuildConfiguration();
    }

    #endregion

    #region NHibernateInitWithNHibernateMappingAttributes

    static void AddAttributeMappings(NHibernate.Cfg.Configuration nhConfiguration)
    {
        var attributesSerializer = new NHibernate.Mapping.Attributes.HbmSerializer { Validate = true };

        using (var stream = attributesSerializer.Serialize(typeof(MySagaData).Assembly))
        {
            nhConfiguration.AddInputStream(stream);
        }
    }

    #endregion
}