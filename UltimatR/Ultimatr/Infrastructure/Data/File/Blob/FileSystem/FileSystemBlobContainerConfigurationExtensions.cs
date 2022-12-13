﻿using System;

namespace UltimatR
{
    public static class FileSystemBlobContainerConfigurationExtensions
    {
        public static FileSystemBlobProviderConfiguration GetFileSystemConfiguration(
            this BlobContainerConfiguration containerConfiguration)
        {
            return new FileSystemBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseFileSystem(
            this BlobContainerConfiguration containerConfiguration,
            Action<FileSystemBlobProviderConfiguration> fileSystemConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(FileSystemBlobProvider);
            containerConfiguration.NamingNormalizers.Add(new FileSystemBlobNamingNormalizer());

            fileSystemConfigureAction(new FileSystemBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}