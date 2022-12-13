using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace UltimatR
{
    public class BlobProviderGetArgs : BlobProviderArgs
    {
        public BlobProviderGetArgs(
            [DisallowNull] string containerName,
            [DisallowNull] BlobContainerConfiguration configuration,
            [DisallowNull] string blobName,
            CancellationToken cancellationToken = default)
            : base(
                containerName,
                configuration,
                blobName,
                cancellationToken)
        {
        }
    }
}