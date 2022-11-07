namespace UltimatR
{

    public class DboRelation<TLeft, TRight> : Entity, IDboRelation<TLeft, TRight> where TLeft : class, IIdentifiable where TRight : class, IIdentifiable
    {
        public virtual long RightEntityId { get; set; }

        public virtual TRight RightEntity { get; set; }

        public virtual long LeftEntityId { get; set; }

        public virtual TLeft LeftEntity { get; set; }
    }
}