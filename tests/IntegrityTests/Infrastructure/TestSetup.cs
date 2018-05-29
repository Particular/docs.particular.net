﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YamlDotNet.Serialization;

namespace IntegrityTests
{
    [SetUpFixture]
    public class TestSetup
    {
        internal static string[] RootDirectories;
        internal static string DocsRootPath;

        internal static Dictionary<string, ComponentMetadata> ComponentMetadata;

        [OneTimeSetUp]
        public void SetupRootDirectories()
        {
            // ENHANCEMENT: Execute git commands when TeamCity environment variables are present
            // to filter the set of files we run against based on changes in a given PR

            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            DocsRootPath = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\.."));

            RootDirectories = new[] { DocsRootPath };

            string componentsYamlPath = Path.Combine(TestSetup.DocsRootPath, "components\\components.yaml");
            var componentsText = File.ReadAllText(componentsYamlPath);

            var builder = new DeserializerBuilder();
            var deserializer = builder.Build();

            var allData = deserializer.Deserialize<List<ComponentMetadata>>(componentsText);
            ComponentMetadata = allData.ToDictionary(m => m.Key, StringComparer.OrdinalIgnoreCase);
        }
    }
}