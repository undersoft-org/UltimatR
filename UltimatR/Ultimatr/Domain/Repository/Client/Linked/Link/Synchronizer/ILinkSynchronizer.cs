using Microsoft.OData.Client;

namespace UltimatR
{
    public interface ILinkSynchronizer
    {
        void AddRepository(IRepository repository);

        void OnLinked(object sender, LoadCompletedEventArgs args);

        void AcquireLinker();

        void ReleaseLinker();

        void AcquireResult();

        void ReleaseResult();
    }
}

