public static class AutomaticRoutingConst
{
    static AutomaticRoutingConst()
    {
        SqlHelper.EnsureDatabaseExists(ConnectionString);
    }

    public static string ConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesAutomaticRouting;Integrated Security=True";
}