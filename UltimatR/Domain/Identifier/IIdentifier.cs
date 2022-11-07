using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace UltimatR
{
    public interface IIdentifier<TEntity> : IIdentifier
    {
        new long EntityId { get; set; }
        TEntity   Entity { get; set; }
    }
    public interface IIdentifier
    {
        long   EntityId { get; set; }

        IdKind  Kind { get; set; }

        string  Name { get; set; }
        
        string  Value { get; set; } 
        
        long   Key { get; }
    }
}