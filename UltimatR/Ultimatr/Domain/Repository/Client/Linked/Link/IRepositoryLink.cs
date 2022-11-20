namespace UltimatR
{
    public interface IRepositoryLink<TStore, TOrigin, TTarget> : ILinkedRepository<TStore, TTarget>, 
                     IDsoRelation<TOrigin, TTarget>, ILinkedObject<TStore, TOrigin> 
                     where TOrigin : Entity where TTarget : Entity where TStore : IDataStore
    {         
    }  
}
