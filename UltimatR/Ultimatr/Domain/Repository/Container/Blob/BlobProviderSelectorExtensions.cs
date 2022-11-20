using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace UltimatR
{
    public static class BlobProviderSelectorExtensions
    {
        public static IBlobProvider Get<TContainer>(
            [DisallowNull] this IBlobProviderSelector selector)
        {           
            return selector.Get(BlobContainerNameAttribute.GetContainerName<TContainer>());
        }
    }
}