using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Series;
using System.Uniques;

namespace UltimatR
{
    public class DaoSet<TEntity> : KeyedCollection<long, TEntity>, IFindable where TEntity : Entity
    {
        public DaoSet() : base()
        {

        }
        public DaoSet(IEnumerable<TEntity> list)
        {
            foreach (var item in list)
                Add(item);
        }

        protected override long GetKeyForItem(TEntity item)
        {
            return (item.Id == 0) ? (long)item.AutoId() : item.Id;
        }

        public TEntity Single
        {
            get => this.FirstOrDefault();
        }

        public object this[object key]
        {
            get
            {
                TryGetValue((long)(key.UniqueKey64()), out TEntity result);
                return result;
            }
            set
            {
                Dictionary[(long)(key.UniqueKey64())] = (TEntity)value;
            }
        }
    }
}