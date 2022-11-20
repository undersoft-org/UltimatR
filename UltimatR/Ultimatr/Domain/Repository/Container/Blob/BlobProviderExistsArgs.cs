using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Castle.Components.DictionaryAdapter;
using JetBrains.Annotations;

namespace UltimatR
{
    public class BlobProviderExistsArgs : BlobProviderArgs
    {
        public BlobProviderExistsArgs(
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