using System.Reflection;

namespace Microsoft.Extensions.Configuration
{
    public class ConfigurationOptions
    {
        public Assembly UserSecretsAssembly { get; set; }

        public string UserSecretsId { get; set; }

        public string FileName { get; set; } = "settings";

        public string EnvironmentName { get; set; }

        public string BasePath { get; set; }

        public string EnvironmentVariablesPrefix { get; set; }

        public string[] CommandLineArgs { get; set; }
    }
}