namespace UltimatR
{
    public interface IRepositoryLink<TStore, TOrigin, TTarget> : ITeleRepository<TStore, TTarget>, 
                     IDsoRelation<TOrigin, TTarget>, ILinkedObject<TStore, TOrigin> 
                     where TOrigin : Entity where TTarget : Entity where TStore : IDataStore
    {         
    }  
}
