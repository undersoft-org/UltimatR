using System.ComponentModel;

namespace UltimatR
{
    public interface IDboRelation<TLeft, TRight>: IIdentifiable where TLeft : class, IIdentifiable where TRight : class, IIdentifiable
    {
        TLeft LeftEntity { get; set; }
        long LeftEntityId { get; set; }
        TRight RightEntity { get; set; }
        long RightEntityId { get; set; }
    }
}