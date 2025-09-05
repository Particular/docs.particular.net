## Custom database catalogs

By default, the SQL Server transport uses the catalog defined in the `Initial Catalog` or `Database` section of the provided connection string.

The catalog can be overwritten using the `DefaultCatalog` method:

snippet: sqlserver-default-catalog
