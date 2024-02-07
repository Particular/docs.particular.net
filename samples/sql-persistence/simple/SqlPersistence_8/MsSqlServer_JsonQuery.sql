startcode SqlServerSagaJsonQuery
SELECT [Correlation_OrderId], OrderData.OrderDescription
FROM [NsbSamplesSqlPersistence].[dbo].[Samples_SqlPersistence_EndpointSqlServer_OrderSaga]
CROSS APPLY OPENJSON([Data]) WITH
(
   OrderId NVARCHAR(500) N'$.OrderId',
   OrderDescription NVARCHAR(2000) N'$.OrderDescription'
) as OrderData
endcode