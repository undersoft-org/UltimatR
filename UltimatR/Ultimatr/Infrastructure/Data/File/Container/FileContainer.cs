
using System;

namespace UltimatR
{
    public class FileContainer : BlobContainer
    {
        public FileContainer(string containerName) 
            : base(containerName, 
            new BlobContainerConfiguration(), 
            new FileSystemBlobProvider(
                new DefaultBlobFilePathCalculator())) { }

        public FileContainer(
            string containerName,
            BlobContainerConfiguration configuration,
            IBlobProvider provider,
            IBlobNormalizeNamingService blobNormalizeNamingService = null) 
            : base(containerName, configuration, provider, blobNormalizeNamingService)
        {
        }

        public string BasePath => $"{Configuration.GetFileSystemConfiguration().BasePath}/{ContainerName}";
    }
}
