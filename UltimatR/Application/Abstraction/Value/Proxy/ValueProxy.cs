using System;
using System.Instant;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Uniques;
using Microsoft.OData.Client;

namespace UltimatR
{
    [StructLayout(LayoutKind.Sequential)]
    public abstract class ValueProxy : Origin, IValueProxy
    {
        protected ISleeve sleeve;

        public virtual object this[string propertyName]
        {
            get
            {
                return sleeve[propertyName];
            }
            set
            {
                sleeve[propertyName] = value;
            }
        }
        public virtual object this[int id]
        {
            get
            {
                return sleeve[id];
            }
            set
            {
                sleeve[id] = value;
            }
        }

        object[] IFigure.ValueArray
        {
            get => sleeve.ValueArray;
            set => sleeve.ValueArray = value;
        }

        Ussn IFigure.SerialCode
        {
            get => sleeve.SerialCode;
            set => sleeve.SerialCode = value;
        }

        [JsonIgnore]
        [IgnoreDataMember]
        [IgnoreClientProperty]
        IRubrics IValueProxy.Rubrics => sleeve.Rubrics;

        [JsonIgnore]
        [IgnoreDataMember]
        [IgnoreClientProperty]
        ISleeve IValueProxy.Valuator => sleeve;

        public virtual bool Equals(IUnique? other)
        {
            return sleeve.Equals(other);
        }

        public virtual int CompareTo(IUnique? other)
        {
            return sleeve.CompareTo(other);
        }

        [JsonIgnore]
        [IgnoreDataMember]
        [IgnoreClientProperty]
        public virtual ulong UniqueKey
        {
            get => sleeve.UniqueKey;
            set => sleeve.UniqueKey = value;
        }

        [JsonIgnore]
        [IgnoreDataMember]
        [IgnoreClientProperty]
        public virtual ulong UniqueSeed
        {
            get => sleeve.UniqueSeed;
            set => sleeve.UniqueSeed = value;
        }

        public virtual byte[] GetBytes()
        {
            return sleeve.GetBytes();
        }

        public virtual byte[] GetUniqueBytes()
        {
            return sleeve.GetUniqueBytes();
        }
    }
}