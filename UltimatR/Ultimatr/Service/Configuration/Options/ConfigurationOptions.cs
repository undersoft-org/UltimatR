//-----------------------------------------------------------------------
// <copyright file="ConfigurationOptions.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Reflection;

namespace Microsoft.Extensions.Configuration
{
    public class ConfigurationOptions
    {
        public string BasePath { get; set; }

        public string[] CommandLineArgs { get; set; }

        public string EnvironmentName { get; set; }

        public string EnvironmentVariablesPrefix { get; set; }

        public string GeneralFileName { get; set; } = "appconfig";

        public string[] OptionalFileNames { get; set; }

        public Assembly UserSecretsAssembly { get; set; }

        public string UserSecretsId { get; set; }
    }
}