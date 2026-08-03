namespace Core.OpenTelemetry;

public class PerformanceMetrics
{
    public static void EnablePerformanceMetrics(EndpointConfiguration endpointConfiguration)
    {
        #region opentelemetry-performance-metrics

        var performanceMetrics = endpointConfiguration.PerformanceMetrics();
        performanceMetrics.EnableSagaFetchTime = true;
        performanceMetrics.EnableDeserializeTime = true;
        performanceMetrics.EnableSerializeTime = true;
        performanceMetrics.EnableOutboxFetchTime = true;
        performanceMetrics.EnableOutboxStoreTime = true;
        performanceMetrics.EnablePersistenceTime = true;

        #endregion
    }
}
