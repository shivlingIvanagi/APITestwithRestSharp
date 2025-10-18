using System.IO;
using Microsoft.Extensions.Configuration;

namespace ApiTestFramework.Config
{
    public class TestConfig
    {
        private static IConfiguration? _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();
                }
                return _configuration;
            }
        }

        public static string BaseUrl => Configuration["ApiSettings:BaseUrl"] ?? "https://jsonplaceholder.typicode.com";
        public static int TimeoutSeconds => int.Parse(Configuration["ApiSettings:TimeoutSeconds"] ?? "30");
        public static string? ApiKey => Configuration["ApiSettings:ApiKey"];
    }
}
