namespace DataAccessLibrary
{
    public static class GlobalConfig
    {
        public static string GetConnectionString(string connectionStringName = "Default")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
