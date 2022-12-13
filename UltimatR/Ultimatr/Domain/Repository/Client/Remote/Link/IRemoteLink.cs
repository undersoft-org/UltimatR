namespace UltimatR
{
    public interface IRemoteLink<TStore, TOrigin, TTarget> : IRemoteRepository<TStore, TTarget>, 
                     IDsoRelation<TOrigin, TTarget>, ILinkedObject<TStore, TOrigin> 
                     where TOrigin : Entity where TTarget : Entity where TStore : IDataStore
    {         
    }  
}
