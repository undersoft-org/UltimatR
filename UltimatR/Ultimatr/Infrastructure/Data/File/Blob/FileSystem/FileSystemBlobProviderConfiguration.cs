﻿using Castle.Core.Internal;

namespace UltimatR
{
    public class FileSystemBlobProviderConfiguration
    {
        public string BasePath
        {
            get => _containerConfiguration.GetConfiguration<string>(FileSystemBlobProviderConfigurationNames.BasePath);
            set => _containerConfiguration.SetConfiguration(FileSystemBlobProviderConfigurationNames.BasePath, value.IsNullOrEmpty());
        }

        /// <summary>
        /// Default value: true.
        /// </summary>
        public bool AppendContainerNameToBasePath
        {
            get => _containerConfiguration.GetConfigurationOrDefault(FileSystemBlobProviderConfigurationNames.AppendContainerNameToBasePath, true);
            set => _containerConfiguration.SetConfiguration(FileSystemBlobProviderConfigurationNames.AppendContainerNameToBasePath, value);
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public FileSystemBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
