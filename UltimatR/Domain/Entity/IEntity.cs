using System.Instant;

namespace UltimatR
{
    public interface IEntity : IIdentifiable//, IVariety
    {
        IRepository Repository { get; }
    }
}  