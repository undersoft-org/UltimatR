using System;
using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public interface IIdentifiable : IUnique, IEquatable<IIdentifiable>, IComparable<IIdentifiable>, IEquatable<BitVector32>,
                                     IEquatable<DateTime>, IEquatable<ISerialNumber>, IValueProxy
    {               
        int  Number { get; set; }

        long     Id { get; set; }
         
        Guid   GUID { get; set; }

        bool IsGUID { get; }

        [JsonIgnore] bool Obsolete { get; set; }
        [JsonIgnore] byte Priority { get; set; }
        [JsonIgnore] bool Inactive { get; set; }
        [JsonIgnore] bool Locked   { get; set; }

        [JsonIgnore] byte   EntityFlags { get; set; } 
        [JsonIgnore] short  EntityOriginId { get; set; } 
        [JsonIgnore] int    EntityTypeId { get; set; }

        [JsonIgnore] DateTime   SystemTime { get; set; }

        [JsonIgnore] new string SerialCode { get; set; }

        void SetFlag(ushort position);
        void ClearFlag(ushort position);
        void SetFlag(bool flag, ushort position);
        bool GetFlag(ushort position);

        long AutoId();
        long SetId(long id);
        long SetId(object id);

        IIdentifiable Sign(object id);
        IIdentifiable Sign();
        IIdentifiable Stamp();

        TEntity Sign<TEntity>(object id) where TEntity : Identifiable;
        TEntity Sign<TEntity>() where TEntity : Identifiable;
        TEntity Stamp<TEntity>() where TEntity : Identifiable;

        TEntity Sign<TEntity>(TEntity entity, object id) where TEntity : class, IIdentifiable;
        TEntity Sign<TEntity>(TEntity entity) where TEntity : class, IIdentifiable;
        TEntity Stamp<TEntity>(TEntity entity) where TEntity : class, IIdentifiable;

        byte GetPriority();
        byte SetPriority(byte priority);
        byte ComparePriority(IIdentifiable entity);
    } 
}