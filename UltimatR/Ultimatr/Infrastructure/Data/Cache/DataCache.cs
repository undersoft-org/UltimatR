//-----------------------------------------------------------------------
// <copyright file="DataCache.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Series;
using System.Threading.Tasks;
using System.Uniques;

namespace UltimatR
{
    public class DataCache : MassCache<IUnique>, IDataCache
    {
        public DataCache() : this(TimeSpan.FromMinutes(15), null, 259)
        {
        }

        public DataCache(TimeSpan? lifeTime = null, IDeputy callback = null, int capacity = 259) : base(
            lifeTime,
            callback,
            capacity)
        {
            if(cache == null)
            {
                cache = this;
            }
        }

        protected virtual IMassDeck<IUnique> cache { get; set; }

        internal virtual T InnerMemorize<T>(T item) where T : IUnique
        {
            uint group = typeof(T).ProxyRetypeKey32();
            if(!cache.TryGet(group, out IUnique deck))
            {
                Sleeve sleeve = SleeveFactory.Create(typeof(T).ProxyRetype(), group);
                sleeve.Combine();

                IRubrics keyrubrics = AssignKeyRubrics(sleeve, item);

                ISleeve isleeve = item.ToSleeve();

                deck = new MassCatalog<IUnique>();

                foreach(MemberRubric keyRubric in keyrubrics)
                {
                    Catalog<IUnique> subdeck = new Catalog<IUnique>();

                    subdeck.Add(item);

                    ((IMassDeck<IUnique>)deck).Put(
                        isleeve[keyRubric.RubricId],
                        keyRubric.RubricName.UniqueKey32(),
                        subdeck);
                }

                cache.Add(group, deck);

                cache.Add(item);

                return item;
            }

            if(!cache.ContainsKey(item))
            {
                IMassDeck<IUnique> _deck = (IMassDeck<IUnique>)deck;

                ISleeve isleeve = item.ToSleeve();

                foreach(MemberRubric keyRubric in isleeve.Rubrics.KeyRubrics)
                {
                    if(!_deck.TryGet(
                        isleeve[keyRubric.RubricId],
                        keyRubric.RubricName.UniqueKey32(),
                        out IUnique outdeck))
                    {
                        outdeck = new Catalog<IUnique>();

                        ((IDeck<IUnique>)outdeck).Put(item);

                        _deck.Put(isleeve[keyRubric.RubricId], keyRubric.RubricName.UniqueKey32(), outdeck);
                    } else
                    {
                        ((IDeck<IUnique>)outdeck).Put(item);
                    }
                }
                cache.Add(item);
            }

            return item;
        }

        internal virtual T InnerMemorize<T>(T item, params string[] names) where T : IUnique
        {
            Memorize(item);

            ISleeve sleeve = item.ToSleeve();

            MemberRubric[] keyrubrics = sleeve.Rubrics.Where(p => names.Contains(p.RubricName)).ToArray();

            IMassDeck<IUnique> _deck = (IMassDeck<IUnique>)cache.Get(item.UniqueSeed);

            foreach(MemberRubric keyRubric in keyrubrics)
            {
                if(!_deck.TryGet(sleeve[keyRubric.RubricId], keyRubric.RubricName.UniqueKey32(), out IUnique outdeck))
                {
                    outdeck = new Catalog<IUnique>();

                    ((IDeck<IUnique>)outdeck).Put(item);

                    _deck.Put(sleeve[keyRubric.RubricId], keyRubric.RubricName.UniqueKey32(), outdeck);
                } else
                {
                    ((IDeck<IUnique>)outdeck).Put(item);
                }
            }

            return item;
        }

        public static IRubrics AssignKeyRubrics(Sleeve sleeve, IUnique item)
        {
            if(!sleeve.Rubrics.KeyRubrics.Any())
            {
                IEnumerable<bool[]>[] rk = item.GetIdentities()
                    .AsCards()
                    .Select(
                        p => (p.Key != (int)DbIdentityType.NONE)
                            ? p.Value
                                .Select(
                                    e => new[]
                                            {
                                                sleeve.Rubrics[e.Name].IsKey = true,
                                                sleeve.Rubrics[e.Name].IsIdentity = true
                                            })
                            : p.Value.Select(h => new[] { sleeve.Rubrics[h.Name].IsIdentity = true }))
                    .ToArray();

                sleeve.Rubrics.KeyRubrics.Put(sleeve.Rubrics.Where(p => p.IsIdentity == true).ToArray());
                sleeve.Rubrics.Update();
            }

            return sleeve.Rubrics.KeyRubrics;
        }

        public virtual IMassDeck<IUnique> CacheSet<T>() where T : IUnique
        {
            if(cache.TryGet(typeof(T).ProxyRetypeKey32(), out IUnique deck))
                return (IMassDeck<IUnique>)deck;
            return null;
        }

        public virtual async Task<T> Lookup<T>(object keys) where T : IUnique
        {
            return await Task.Run(
                () =>
                {
                    if(cache.TryGet(keys, typeof(T).ProxyRetypeKey32(), out IUnique output))
                        return (T)output;
                    return default;
                });
        }

        public virtual IDeck<IUnique> Lookup<T>(Tuple<string, object> valueNamePair) where T : IUnique
        { return Lookup<T>((m) => (IDeck<IUnique>)m.Get(valueNamePair.Item2, valueNamePair.Item2.UniqueKey32())); }

        public virtual IDeck<IUnique> Lookup<T>(Func<IMassDeck<IUnique>, IDeck<IUnique>> selector) where T : IUnique
        { return selector(CacheSet<T>()); }

        public virtual T Lookup<T>(T item) where T : IUnique
        {
            ISleeve shell = item.ToSleeve();
            IRubrics mrs = shell.Rubrics.KeyRubrics;
            T[] result = new T[mrs.Count];
            int i = 0;
            if(cache.TryGet(typeof(T).ProxyRetypeKey32(), out IUnique deck))
            {
                foreach(MemberRubric mr in mrs)
                {
                    if(((IMassDeck<IUnique>)deck).TryGet(
                        shell[mr.RubricId],
                        mr.RubricName.UniqueKey32(),
                        out IUnique outdeck))
                        if(((IDeck<IUnique>)outdeck).TryGet(item, out IUnique output))
                            result[i++] = (T)output;
                }
            }

            if(result.Any(r => r == null))
                return default;
            return result[0];
        }

        public virtual T[] Lookup<T>(object key, params Tuple<string, object>[] valueNamePairs) where T : IUnique
        {
            return Lookup<T>(
                (k) => k[key],
                valueNamePairs.ForEach(
                    (vnp) => new Func<IMassDeck<IUnique>, IDeck<IUnique>>(
                        (m) => (IDeck<IUnique>)m
                                                                        .Get(vnp.Item2, vnp.Item1.UniqueKey32())))
                    .ToArray());
        }

        public virtual T[] Lookup<T>(
            Func<IDeck<IUnique>, IUnique> key,
            params Func<IMassDeck<IUnique>, IDeck<IUnique>>[] selectors)
            where T : IUnique
        {
            if(cache.TryGet(typeof(T).ProxyRetypeKey32(), out IUnique deck))
            {
                T[] result = new T[selectors.Length];
                for(int i = 0; i < selectors.Length; i++)
                {
                    result[i] = (T)key(selectors[i]((IMassDeck<IUnique>)deck));
                }
                return result;
            }

            return default;
        }

        public virtual IDeck<IUnique> Lookup<T>(object key, string propertyNames) where T : IUnique
        {
            if(CacheSet<T>().TryGet(key, propertyNames.UniqueKey32(), out IUnique outdeck))
                return (IDeck<IUnique>)outdeck;
            return default;
        }

        public virtual T Lookup<T>(T item, params string[] propertyNames) where T : IUnique
        {
            ISleeve ilValuator = item.ToSleeve();
            MemberRubric[] mrs = ilValuator.Rubrics.Where(p => propertyNames.Contains(p.RubricName)).ToArray();
            T[] result = new T[mrs.Length];

            if(cache.TryGet(typeof(T).ProxyRetypeKey32(), out IUnique deck))
            {
                int i = 0;
                foreach(MemberRubric mr in mrs)
                {
                    if(((IMassDeck<IUnique>)deck).TryGet(
                        ilValuator[mr.RubricId],
                        mr.RubricName.UniqueKey32(),
                        out IUnique outdeck))
                        if(((IDeck<IUnique>)outdeck).TryGet(item, out IUnique output))
                            result[i++] = (T)output;
                }
            }

            if(result.Any(r => r == null))
                return default;
            return result[0];
        }

        public virtual IEnumerable<T> Memorize<T>(IEnumerable<T> items) where T : IUnique
        { return items.ForEach(p => Memorize(p)); }

        public virtual T Memorize<T>(T item) where T : IUnique
        {
            if(!item.IsEventType())
                return InnerMemorize(item);
            return default(T);
        }

        public virtual T Memorize<T>(T item, params string[] names) where T : IUnique
        {
            if(InnerMemorize(item) != null)
                return InnerMemorize(item, names);
            return default(T);
        }

        public virtual async Task<T> MemorizeAsync<T>(T item) where T : IUnique
        { return await Task.Run(() => Memorize(item)); }
        public virtual async Task<T> MemorizeAsync<T>(T item, params string[] names) where T : IUnique
        { return await Task.Run(() => Memorize(item, names)); }

        public virtual IMassDeck<IUnique> Catalog => cache;
    }
}  
  