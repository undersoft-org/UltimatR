using IdentityModel;
using System.Collections.ObjectModel;
using System.Instant;
using System.Linq;
using System.Series;
using System.Uniques;

namespace UltimatR
{

    public class DaoToSet<TChild> : KeyedCollection<long, TChild>, IFindable where TChild : Entity
{
        protected override long GetKeyForItem(TChild item)
        {
            return (item.Id == 0) ? (long)item.AutoId() : item.Id;
}

        public TChild Single
        {
            get => this.FirstOrDefault();
        }

        public object this[object key]
        {
            get
{
                TryGetValue((long)(key.UniqueKey64()), out TChild result);
                return result;
            }
            set
            {
                Dictionary[(long)(key.UniqueKey64())] = (TChild)value;
            }
        }
    }
}