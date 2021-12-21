namespace Router
{
    class NamespaceDescription
    {
        public NamespaceDescription(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
        public string Name { get; }
    }
}