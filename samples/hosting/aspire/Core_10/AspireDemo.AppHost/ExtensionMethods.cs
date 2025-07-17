namespace AspireDemo.AppHost;

public static class ExtensionMethods
{
    public static async Task<string> GetRabbitMqConnectionString(
        this IResourceBuilder<RabbitMQServerResource> builder)
    {
        var rabbitEnvVariables = await builder.Resource.GetEnvironmentVariableValuesAsync();
        var user = rabbitEnvVariables["RABBITMQ_DEFAULT_USER"];
        var pass = rabbitEnvVariables["RABBITMQ_DEFAULT_PASS"];
        return $"host={builder.Resource.Name}:{builder.Resource.PrimaryEndpoint.TargetPort};username={user};password={pass}";
    }
}