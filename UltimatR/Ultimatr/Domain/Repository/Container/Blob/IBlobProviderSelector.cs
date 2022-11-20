using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace UltimatR
{
    public interface IBlobProviderSelector
    {
        IBlobProvider Get([DisallowNull] string containerName);
    }
}