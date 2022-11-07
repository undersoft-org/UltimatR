using System;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Series;
using System.Threading;
using System.Threading.Tasks;
using System.Uniques;

namespace UltimatR
{
    public class DataCache<TStore, TEntity> : DataCache<TStore>, IDataCache<TStore, TEntity> where TEntity : IUnique
    {
        protected override IMassDeck<IUnique> cache { get; set; }        

        public override ulong UniqueSeed { get; set; }

        public DataCache(IDataCache<TStore> datacache) : base()
        {
            if (base.cache == null || this.cache == null)
            {
                Mapper = datacache.Mapper;
                base.cache = datacache;
                var seed = typeof(TEntity).ProxyRetypeKey32();
                if (!base.Catalog.TryGet(seed, out IUnique deck))
                {
                    deck = new MassCatalog<IUnique>();
                    base.Catalog.Add(seed, deck);
                }
                this.cache = (IMassDeck<IUnique>)deck;

                UniqueSeed = seed;
                base.UniqueSeed = typeof(TStore).ProxyRetypeKey32();
            }
        }

        public override IMassDeck<IUnique> Catalog
        {
            get => cache;
        }

        public async Task<TEntity> Lookup(object keys)
        {
            return await Lookup<TEntity>(keys);
        }

        public TEntity[] Lookup(object key, params Tuple<string, object>[] valueNamePairs)
        {
           return Lookup<TEntity>(key, valueNamePairs);
        }

        public IDeck<IUnique> Lookup(Tuple<string, object> valueNamePair)
        {
            return Lookup<TEntity>((m) => (IDeck<IUnique>)m.Get(valueNamePair.Item2, valueNamePair.Item1.UniqueKey32()));
        }

        public IDeck<IUnique> Lookup(Func<IMassDeck<IUnique>, IDeck<IUnique>> selector)
        {
            return Lookup<TEntity>(selector);
        }

        public TEntity[] Lookup(Func<IDeck<IUnique>, IUnique> key,
            params Func<IMassDeck<IUnique>, IDeck<IUnique>>[] selectors)
        {
            return Lookup<TEntity>(key, selectors);
        }

        public IDeck<IUnique> Lookup(object key, string propertyNames)
        {
            return Lookup<TEntity>(key, propertyNames);
        }

        public TEntity Lookup(TEntity item, params string[] propertyNames)
        {
            return Lookup<TEntity>(item, propertyNames);
        }

        public TEntity Lookup(TEntity item)
        {
            return Lookup<TEntity>(item);
        }

        public IMassDeck<IUnique> CacheSet()
        {
            return CacheSet<TEntity>();
        }
         
        public IEnumerable<TEntity> Memorize(IEnumerable<TEntity> items)
        {
            return Memorize<TEntity>(items);
        }

        public TEntity Memorize(TEntity item)
        {
            return Memorize<TEntity>(item);
        }

        public async Task<TEntity> MemorizeAsync(TEntity item)
{
            return await MemorizeAsync<TEntity>(item);
        }

        public TEntity Memorize(TEntity item, params string[] names) 
        {
            return Memorize<TEntity>(item, names);
        }

        public async Task<TEntity> MemorizeAsync(TEntity item, params string[] names)
        {
            return await MemorizeAsync<TEntity>(item, names);
        }
    }

    public class DataCache<TStore> : DataCache, IDataCache<TStore>
    {
        protected override IMassDeck<IUnique> cache { get; set; }

        public IDataMapper Mapper { get; set; }

        public DataCache(IDataCache cache) 
        {            
            if (base.cache == null || this.cache == null)
            {
                Mapper = ServiceManager.GetObject<IRepositoryMapper>();
                base.cache = cache;
                var seed = typeof(TStore).UniqueKey32();
                UniqueSeed = seed;
                if (!base.Catalog.TryGet(seed, out IUnique deck))
                {
                    deck = new MassCatalog<IUnique>();
                    base.Catalog.Add(seed, deck);
                }
                this.cache = (IMassDeck<IUnique>)deck;
            }
        }

        public DataCache(TimeSpan? lifeTime = null, Deputy callback = null, int capacity = 257)
            : base(lifeTime, callback, capacity)
        {
            if (cache == null)
            {
                var seed = typeof(TStore).UniqueKey32();
                UniqueSeed = seed;
                if (!base.Catalog.TryGet(seed, out IUnique deck))
                {
                    deck = new MassCatalog<IUnique>();
                    base.Catalog.Add(seed, deck);
                }
                cache = (IMassDeck<IUnique>)deck;
            }
        }

        public override IMassDeck<IUnique> Catalog
        {
            get => cache;
        }
    }

    public class DataCache : MassCache<IUnique>, IDataCache
    {       
        protected virtual IMassDeck<IUnique> cache { get; set; }

        public DataCache() : this(TimeSpan.FromMinutes(15), null, 259)
        {           
        }

        public DataCache(TimeSpan? lifeTime = null, Deputy callback = null, int capacity = 259)
            : base(lifeTime, callback, capacity)
        {
            if (cache == null)
            {
                cache = this;              
            }
        }

        public virtual IMassDeck<IUnique> Catalog
        {
            get => cache;
        }

        public virtual async Task<T> Lookup<T>(object keys) where T : IUnique
        {
            return await Task.Run(() =>
            {
                if (cache.TryGet(keys, typeof(T).ProxyRetypeKey32(), out IUnique output))
                    return (T)output;
                return default;
            });
        }
        public virtual T[] Lookup<T>(object key, params Tuple<string, object>[] valueNamePairs) where T : IUnique
        {
            return Lookup<T>((k) => k[key], valueNamePairs.ForEach((vnp) =>
                                                        new Func<IMassDeck<IUnique>, IDeck<IUnique>>(
                                                                    (m) => (IDeck<IUnique>)m
                                                                        .Get(vnp.Item2,
                                                                             vnp.Item1.UniqueKey32())))
                                                                                .ToArray());
        }
       
        public virtual IDeck<IUnique> Lookup<T>(Tuple<string, object> valueNamePair) where T : IUnique
        {
            return Lookup<T>((m) => (IDeck<IUnique>)m.Get(valueNamePair.Item2, valueNamePair.Item2.UniqueKey32()));
        }
        
        public virtual IDeck<IUnique> Lookup<T>(Func<IMassDeck<IUnique>, IDeck<IUnique>> selector) where T : IUnique
        {
           return selector(CacheSet<T>());                   
        }
        
        public virtual T[] Lookup<T>(Func<IDeck<IUnique>, IUnique> key, 
                              params Func<IMassDeck<IUnique>, IDeck<IUnique>>[] selectors) where T : IUnique
        {
            if (cache.TryGet(typeof(T).ProxyRetypeKey32(), out IUnique deck))
            {
                T[] result = new T[selectors.Length];
                for (int i = 0; i < selectors.Length; i++)
                {
                    result[i] = (T)key(selectors[i](((IMassDeck<IUnique>)deck)));
                }
                return result;
            }

            return default;
        }

        public virtual IDeck<IUnique> Lookup<T>(object key, string propertyNames) where T : IUnique
        {          
                if (CacheSet<T>().TryGet(key, propertyNames.UniqueKey32(), out IUnique outdeck))
                    return (IDeck<IUnique>)outdeck;
            return default;
        } 
       
        public virtual T Lookup<T>(T item, params string[] propertyNames) where T : IUnique
        {
            ISleeve ilValuator = item.ToSleeve();
            var mrs = ilValuator.Rubrics.Where(p => propertyNames.Contains(p.RubricName)).ToArray();
            T[] result = new T[mrs.Length];
            int i = 0;
            if (cache.TryGet(typeof(T).ProxyRetypeKey32(), out IUnique deck))
            {
                foreach (var mr in mrs)
                {
                    if (((IMassDeck<IUnique>)deck).TryGet(ilValuator[mr.RubricId],
                        mr.RubricName.UniqueKey32(), out IUnique outdeck))
                        if (((IDeck<IUnique>)outdeck).TryGet(item, out IUnique output))
                            result[i++] = (T)output;
                }
            }

            if (result.Any(r => r == null))
                return default;
            return result[0];
        }
      
        public virtual T Lookup<T>(T item) where T : IUnique
        {
            ISleeve shell = item.ToSleeve();
            var mrs = shell.Rubrics.KeyRubrics;
            T[] result = new T[mrs.Count];
            int i = 0;
            if (cache.TryGet(typeof(T).ProxyRetypeKey32(), out IUnique deck))
            {
                foreach (var mr in mrs)
                {
                    if (((IMassDeck<IUnique>)deck).TryGet(shell[mr.RubricId],
                            mr.RubricName.UniqueKey32(), out IUnique outdeck))
                        if (((IDeck<IUnique>)outdeck).TryGet(item, out IUnique output))
                            result[i++] = (T)output;
                }
            }

            if (result.Any(r => r == null))
                return default;
            return result[0];
        }
     
        public virtual IMassDeck<IUnique> CacheSet<T>() where T : IUnique
        {
            if (cache.TryGet(typeof(T).ProxyRetypeKey32(), out IUnique deck))
                return ((IMassDeck<IUnique>)deck); 
            return null; 
        }

        public virtual IEnumerable<T> Memorize<T>(IEnumerable<T> items) where T : IUnique
        {
            return items.ForEach(p => Memorize(p));
        }
        
        public virtual T Memorize<T>(T item) where T : IUnique
        {
            if (!item.IsEventType())            
              return InnerMemorize(item);                            
            return default(T);
        }        
        public virtual T Memorize<T>(T item, params string[] names) where T : IUnique
        {
            if (InnerMemorize(item) != null)
                return InnerMemorize(item, names);
            return default(T);
        }

        public virtual async Task<T> MemorizeAsync<T>(T item) where T : IUnique
        {
            return await Task.Run(() => Memorize(item));
        }
        public virtual async Task<T> MemorizeAsync<T>(T item, params string[] names) where T : IUnique
        {
            return await Task.Run(() => Memorize(item, names));
        }

        public virtual T InnerMemorize<T>(T item, params string[] names) where T : IUnique
        {
            Memorize(item);

            var sleeve = item.ToSleeve();

            var keyrubrics = sleeve.Rubrics.Where(p => names.Contains(p.RubricName)).ToArray();

            var _deck = ((IMassDeck<IUnique>)cache.Get(item.UniqueSeed));

            foreach (var keyRubric in keyrubrics)
            {
                if (!_deck.TryGet(sleeve[keyRubric.RubricId], keyRubric.RubricName.UniqueKey32(), out IUnique outdeck))
                {
                    outdeck = new Catalog<IUnique>();

                    ((IDeck<IUnique>)outdeck).Put(item);

                    _deck.Put(sleeve[keyRubric.RubricId],
                        keyRubric.RubricName.UniqueKey32(), outdeck);
                }
                else
                {
                    ((IDeck<IUnique>)outdeck).Put(item);
                }
            }

            return item;
        }
        public virtual T InnerMemorize<T>(T item) where T : IUnique
        {
            uint group = typeof(T).ProxyRetypeKey32();
            if (!cache.TryGet(group, out IUnique deck))
            {
                var sleeve = SleeveFactory.Create(typeof(T).ProxyRetype(), group);
                sleeve.Combine();

                var keyrubrics = AssignKeyRubrics(sleeve, item);

                var isleeve = item.ToSleeve();

                deck = new MassCatalog<IUnique>();

                foreach (var keyRubric in keyrubrics)
                {
                    var subdeck = new Catalog<IUnique>();

                    subdeck.Add(item);

                    ((IMassDeck<IUnique>)deck).Put(
                        isleeve[keyRubric.RubricId],
                        keyRubric.RubricName.UniqueKey32(), subdeck);
                }

                cache.Add(group, deck);

                cache.Add(item);

                return item;
            }

            if (!cache.ContainsKey(item))
            {
                IMassDeck<IUnique> _deck = (IMassDeck<IUnique>)deck;

                var isleeve = item.ToSleeve();

                foreach (var keyRubric in isleeve.Rubrics.KeyRubrics)
                {
                    if (!_deck.TryGet(isleeve[keyRubric.RubricId],
                        keyRubric.RubricName.UniqueKey32(),
                        out IUnique outdeck))
                    {
                        outdeck = new Catalog<IUnique>();

                        ((IDeck<IUnique>)outdeck).Put(item);

                        _deck.Put(isleeve[keyRubric.RubricId],
                            keyRubric.RubricName.UniqueKey32(), outdeck);
                    }
                    else
                    {
                        ((IDeck<IUnique>)outdeck).Put(item);
                    }
                }
                cache.Add(item);
            }

            return item;
        }

        public static IRubrics AssignKeyRubrics(Sleeve sleeve, IUnique item)
        {
            if (!sleeve.Rubrics.KeyRubrics.Any())
            {

                var rk = item.GetIdentities().AsCards()
                    .Select(p => (p.Key != (int)DbIdentityType.NONE)
                        ? p.Value.Select(e => new[]
                        {
                            sleeve.Rubrics[e.Name].IsKey = true,
                            sleeve.Rubrics[e.Name].IsIdentity = true
                        })
                        : p.Value.Select(h => new[]
                        {
                            sleeve.Rubrics[h.Name].IsIdentity = true

                        })).ToArray();

                sleeve.Rubrics.KeyRubrics.Put(sleeve.Rubrics.Where(p => p.IsIdentity == true).ToArray());
                sleeve.Rubrics.Update();
            }

            return sleeve.Rubrics.KeyRubrics;
        } 
    }

    public static class GlobalCache
    {
        private static IDataCache cache;             

        static GlobalCache()
        {
            cache = new DataCache(TimeSpan.FromMinutes(15), null, 513);
        }

        public static IDataCache Catalog
        {
            get => cache;
        }     

        public static async Task<T> Lookup<T>(object keys) where T : IUnique
        {
            return await Task.Run(() =>
            {
                if (cache.TryGet(keys, typeof(T).ProxyRetypeKey32(), out IUnique output))
                    return (T)output;
                return default;
            });
        }

        public static T ToCache<T>(this T item) where T : IUnique
        {           
            return cache.Memorize<T>(item);
        }

        public static async Task<T> ToCacheAsync<T>(this T item) where T : IUnique
        {
            return await Task.Run(() => ToCache<T>(item));
        } 

        public static T ToCache<T>(this T item, params string[] names) where T : IUnique
        {
            return cache.Memorize(item, names);  
        } 

        public static async Task<T> ToCacheAsync<T>(this T item, params string[] names) where T : IUnique
        {
            return await Task.Run(() => ToCache(item, names));
        }
    }    
      
}  
  