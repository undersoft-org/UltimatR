using AutoMapper;
using Microsoft.OData.Client;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Extract;
using System.Instant;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Uniques;

namespace UltimatR
{
    [Microsoft.OData.Client.Key("Id")]
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public class Identifiable : ValueProxy, IIdentifiable
    {
        #region Fields

        protected Ussn serialcode;
        protected int[] keyOrdinals;

        [JsonIgnore] public bool IsGUID => GetFlag(3);

        #endregion

        #region Constructors

        public Identifiable()
        {

        }
        public Identifiable(bool autoId) : this()
        {
            if (!autoId) return;

            serialcode.UniqueKey = Unique.New;
            _ = this.ToCacheAsync();
        }
        public Identifiable(object id) : this(id.UniqueKey64())
        {
        }
        public Identifiable(ulong id) : this()
        {
            serialcode.UniqueKey = id;
            _ = this.ToCacheAsync();
        }

        #endregion

        #region Properties  

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Number { get; set; }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual long Id
        {
            get => (long)UniqueKey;
            set => UniqueKey = (ulong)value;
        }

        [JsonIgnore]
        [NotMapped] [IgnoreMap]
        [IgnoreClientProperty]
        public virtual bool Obsolete
        {
            get => GetFlag(2);
            set => SetFlag(value, 2);
        }

        [JsonIgnore] 
        [NotMapped] [IgnoreMap] 
        [IgnoreClientProperty]
        public virtual bool Inactive 
        {
            get => GetFlag(1);
            set => SetFlag(value, 1); 
        }

        [JsonIgnore]
        [NotMapped][IgnoreMap]
        [IgnoreClientProperty]
        public virtual bool Locked
        {
            get => GetFlag(0);
            set => SetFlag(value, 0);
        }

        [JsonIgnore]
        public virtual byte Priority
        {
            get => GetPriority();
            set => SetPriority(value);
        }
      
        [JsonIgnore] 
        [NotMapped] [IgnoreMap] 
        [IgnoreClientProperty]
        public virtual int EntityTypeId
        {
            get => (int)UniqueSeed;
            set => UniqueSeed = (ulong)value;
        }
        
        [JsonIgnore] 
        [NotMapped] [IgnoreMap] 
        [IgnoreClientProperty]
        public virtual short EntityOriginId
        {
            get
            {
                if (base.OriginId != 0 && serialcode.BlockX == 0)
                    serialcode.BlockX = (ushort)base.OriginId;
                return (short)serialcode.BlockX;
            }
            set
            {
                serialcode.BlockX = (ushort)value;
                base.OriginId = value;
            }
        }
        
        [JsonIgnore] 
        [NotMapped] [IgnoreMap] 
        [IgnoreClientProperty]
        public virtual byte EntityFlags
        {
            get => (byte)serialcode.GetFlags();
            set => serialcode.SetFlagBits(new BitVector32(value));
        }

        [JsonIgnore] [NotMapped]
        public virtual DateTime SystemTime
        {
            get => DateTime.FromBinary(serialcode.TimeBlock);
            set => serialcode.TimeBlock = value.ToBinary();
        }
        
        [JsonIgnore] 
        [NotMapped] [IgnoreMap] 
        [IgnoreClientProperty]
        public override ulong UniqueKey
        {
            get => serialcode.UniqueKey;
            set
            {
                if (value != 0 && !serialcode.Equals(value))
                    serialcode.UniqueKey = value;
            }
        }
        
        [JsonIgnore] 
        [NotMapped] [IgnoreMap] 
        [IgnoreClientProperty]
        public override ulong UniqueSeed
        {
            get => serialcode.UniqueSeed == 0
                ? serialcode.UniqueSeed = this.ProxyRetypeKey32()
                : serialcode.UniqueSeed; 
            set
            {
                if (value != 0 && value != serialcode.UniqueSeed)
                    serialcode.UniqueSeed = this.ProxyRetypeKey32();
            }
        }

        [JsonIgnore] 
        [NotMapped] [IgnoreMap] 
        [IgnoreClientProperty]
        public virtual Guid GUID
        {
            get => serialcode.GUID;
            set
            {
                if (IsGUID)
                    serialcode.GUID = value;
            }
        }

        [JsonIgnore] [Required] 
        [StringLength(32)] 
        [ConcurrencyCheck]
        public virtual string SerialCode
        {
            get => serialcode;
            set => serialcode.FromHexTetraChars(value.ToCharArray());
        }

        [JsonIgnore]
        [NotMapped] [IgnoreMap]
        [IgnoreClientProperty]
        Ussn IFigure.SerialCode
        {
            get => serialcode;
            set => serialcode = value;
        }

        #endregion

        #region Methods

        public long AutoId()
        {
            var key = serialcode.UniqueKey;
            if (key != 0)
                return (long)key;

            ulong id = Unique.New;
            serialcode.UniqueKey = id;
            return (long)id;
        }
        public long SetId(object id)
        {
            if (id == null)
                return AutoId();
            else if (id.GetType().IsPrimitive)
                return SetId((long)id);
            else
                return SetId((long)id.UniqueKey64());
        }
        public long SetId(long id)
        {
            ulong ulongid = (ulong)id;
            var key = serialcode.UniqueKey;
            if (ulongid != 0 && key != ulongid)
                return (long)(serialcode.UniqueKey = ulongid);
            return AutoId();
        }

        public byte GetPriority()
        {
            return serialcode.GetPriority();
        }
        public byte SetPriority(byte priority)
        {
            return serialcode.SetPriority(priority);
        }
        public byte ComparePriority(IIdentifiable entity)
        {
            return serialcode.ComparePriority(entity.GetPriority());
        }

        public TEntity Sign<TEntity>(TEntity entity, object id) where TEntity : class, IIdentifiable
        {
            entity.SetId(id);
            entity.SystemTime = DateTime.Now;
            var originId = entity.EntityOriginId;
            return entity;
        }
        public TEntity Sign<TEntity>(TEntity entity) where TEntity : class, IIdentifiable
        {
            entity.AutoId();
            entity.SystemTime = DateTime.Now;
            var originId = entity.EntityOriginId;
            return entity;
        }
        public TEntity Stamp<TEntity>(TEntity entity) where TEntity : class, IIdentifiable
        {
            if (!entity.IsGUID)
                entity.SystemTime = DateTime.Now;
            return entity;
        }

        public IIdentifiable Sign(object id)
        {
            return Sign(this, id);
        }
        public IIdentifiable Sign()
        {
            return Sign(this);
        }
        public IIdentifiable Stamp()
        {
            return Stamp(this);
        }

        public TEntity Sign<TEntity>(object id) where TEntity : Identifiable
        {
            return Sign((TEntity)this, id);
        }
        public TEntity Sign<TEntity>() where TEntity : Identifiable
        {
            return Sign((TEntity)this);
        }
        public TEntity Stamp<TEntity>() where TEntity : Identifiable
        {
            return Stamp((TEntity)this);
        }

        public void SetFlag(ushort position)
        {
            serialcode.SetFlagBit(position);
        }
        public void ClearFlag(ushort position)
        {
            serialcode.ClearFlagBit(position);
        }
        public bool GetFlag(ushort position)
        {
            return serialcode.GetFlagBit(position);
        }
        public void SetFlag(bool flag, ushort position)
        {
            serialcode.SetFlag(flag, position);
        }

        public int CompareTo(IIdentifiable other)
        {
            return serialcode.CompareTo(other);
        }
        public override int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }

        public bool Equals(BitVector32 other)
        {
            return ((IEquatable<BitVector32>)serialcode).Equals(other);
        }
        public bool Equals(DateTime other)
        {
            return ((IEquatable<DateTime>)serialcode).Equals(other);
        }
        public bool Equals(IIdentifiable other)
        {
            return serialcode.Equals(other);
        }
        public bool Equals(ISerialNumber other)
        {
            return ((IEquatable<ISerialNumber>)serialcode).Equals(other);
        }
        public override bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }

        public override byte[] GetBytes()
        {
            return this.GetStructureBytes();
        }
        public override byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        public virtual int[]    UniqueOrdinals()
        {
            if (keyOrdinals == null)
            {
                var pks = sleeve.Rubrics.KeyRubrics;
                if (pks.Any())
                {
                    keyOrdinals = pks.Select(p => p.RubricId).ToArray();
                }
            }
            return keyOrdinals;
        }
        public virtual object[] UniqueValues()
        {
            var ids = keyOrdinals;
            if (ids == null)
                ids = UniqueOrdinals();
            return ids.Select(k => this[k]).ToArray();
        }
        public virtual ulong    CompactKey()
        {
            return UniqueValues().UniqueKey64();
        }

        #endregion
    }
}
