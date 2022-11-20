using System.Collections.Generic;

namespace System.Instant
{
    using System.Collections;
    using System.Series;

    using Linq;

    using Uniques;

    public interface IVariety : IFigure
    {
        ISleeve Preset { get; }
        ISleeve Entry { get; }

        IRubrics Rubrics { get; set; }

        object Clone();

        void MapPreset();
        void MapDevisor();
        void MapEntry();

        E Patch<E>(IDeputy traceChanges = null) where E : class;
        E Patch<E>(E item, IDeputy traceChanges = null) where E : class;

        object PatchSelf();

        E Put<E>(IDeputy traceChanges = null) where E : class;
        E Put<E>(E item, IDeputy traceChanges = null) where E : class;

        object PutSelf();

        Vary[] Detect<E>(E item, IDeputy traceChanges = null) where E : class;
    }

    public interface IVariety<T> : IVariety
    {
        T Devisor { get; }

        new T Clone();

        new T PatchSelf();
        T Patch(T item, IDeputy traceChanges = null);

        new T PutSelf();
        T Put(T item, IDeputy traceChanges = null);

        Vary[] Detect(T item, IDeputy traceChanges = null);
    }

    public struct Vary
    {
        public int TargetIndex;
        public object OriginValue;
        public object TargetValue;
        public Type OriginType;
        public Type TargetType;
    }

    public class Variety<T> : Variety, IVariety<T> where T : class
    {
        public Variety() { }
        public Variety(T item) : base(item) { }

        public T Patch(T item, IDeputy callback = null)
        {
            base.Patch<T>(item);
            return item;
        }

        public T PatchFrom(T source)
        {
            ValueArray = factory.Combine(source).ValueArray;
            base.PatchSelf();
            return (T)(entry.Devisor);
        }

        public new T PatchSelf()
        {
            base.PatchSelf();
            return (T)(entry.Devisor);
        }

        public T Put(T item, IDeputy callback = null)
        {
            base.Put<T>(item);
            return item;
        }

        public T PutFrom(T source)
        {
            ValueArray = factory.Combine(source).ValueArray;
            base.PutSelf();
            return (T)(entry.Devisor);
        }

        public new T PutSelf()
        {
            base.PutSelf();
            return (T)(entry.Devisor);
        }

        public Vary[] Detect(T item, IDeputy callback = null)
        {
            return base.Detect<T>(item);
        }

        public new T Clone()
        {
            var clone = typeof(T).New<T>();
            var _clone = factory.Combine(clone);
            _clone.ValueArray = entry.ValueArray;
            return clone;
        }

        public ISleeve EntryProxy => entry;
        public ISleeve PresetProxy => (ISleeve)Preset;

        public new T Entry => (T)(preset.Devisor);
        public new T Preset => (T)((preset == null) ? preset = factory.Combine(entry) : preset);

        public new T Devisor
        {
            get => (T)(entry.Devisor);
            set => entry.Devisor = value;
        }
    }

    public class Variety : IVariety
    {
        protected HashSet<int> trackset;
        protected Sleeve factory;
        protected ISleeve entry;
        protected ISleeve preset;
        protected Type type => factory.BaseType;
        protected int otherVariationCount = 0;
        protected int otherMutationsCount = 0;
        protected int sameVariationCount = 0;
        protected int sameMutationsCount = 0;
        protected bool traceable;

        public ISleeve Entry
        {
            get => entry;
            set => entry = value;
        }

        public ISleeve Preset
        {
            get => ((preset == null)
                ? preset = factory.Combine(entry)
                : preset);
            set => preset = value;
        }

        public Action<Variety, object> NextVariation { get; set; }

        public IDeputy TraceEvent { get; set; }

        public Variety() { }

        public Variety(object item, IDeputy traceChanges) : this(item.GetType())
        {
            TraceEvent = traceChanges;
            traceable = true;
            if (item.GetType().IsAssignableTo(typeof(ISleeve)))
                Combine(item as ISleeve);
            else
                Combine(item);
        }

        public Variety(object item) : this(item.GetType())
        {
            if (item.GetType().IsAssignableTo(typeof(ISleeve)))
                Combine(item as ISleeve);
            else
                Combine(item);
        }

        public Variety(ISleeve sleeve) : this(sleeve.Devisor.GetType())
        {
            Combine(sleeve);
        }

        public Variety(Type type)
        {
            factory = SleeveFactory.Create(type);
        }

        public void Combine(ISleeve sleeve)
        {
            entry = sleeve;
            traceable = factory.Traceable;
        }

        public void Combine(object item)
        {
            entry = factory.Combine(item);
            traceable = factory.Traceable;
        }

        protected void setDeltaMarks()
        {
            foreach (var rubric in Rubrics)
            {
                if (rubric.DeltaMark != null)
                    preset[rubric.RubricId] = rubric.DeltaMark;
            }
        }

        protected void setBy(ISleeve target, Vary[] changes, int count)
        {
            var _target = target;
            var _changes = changes;

            for (int i = 0; i < count; i++)
            {
                var vary = _changes[i];
                var ti = vary.TargetIndex;
                var ov = vary.OriginValue;

                if (vary.TargetType
                    .IsAssignableTo
                        (vary.OriginType))
                {
                    _target[ti] = ov;
                }
            }
        }

        protected void set(ISleeve target, Vary[] changes, int count)
        {
            var _target = target;
            var _changes = changes;

            for (int i = 0; i < count; i++)
            {
                var vary = _changes[i];
                _target[vary.TargetIndex] = vary.OriginValue;
            }
        }

        public object Patch(object item, IDeputy traceChanges = null)
        {
            Vary[] changes;
            if (traceChanges != null)
                traceable = true;

            NextVariation = (o, t) => o.Patch(t, traceChanges);

            ISleeve target = item.ToSleeve();
            if (item.GetType() != type)
                setBy(target, changes = otherTypeVariations(target),
                    otherVariationCount);
            else
                set(target, changes = sameTypeVariations(target),
                    sameVariationCount);
           
            if (traceable)
                traceChanges.Publish(item, changes);

            return item;
        }

        public E Patch<E>(E item, IDeputy traceChanges = null) where E : class
        {
            Vary[] changes;
            if (traceChanges != null)
                traceable = true;

            NextVariation = (o, t) => o.Patch(t, traceChanges);

            ISleeve target = item.ToSleeve();
            if (typeof(E) != type)
                setBy(target, changes = otherTypeVariations(target),
                    otherVariationCount);
            else
                set(target, changes = sameTypeVariations(target),
                    sameVariationCount);
           
            if (traceable)
                TraceEvent.Publish(item, changes);

            return item;
        }

        public E Patch<E>(IDeputy traceChanges = null) where E : class
        {
            return Patch(typeof(E).New<E>(), traceChanges);
        }

        public object PatchSelf()
        {
            NextVariation = (o, s) => o.PatchSelf();

            set(entry, sameTypeVariations(entry), sameVariationCount);
            return Devisor;
        }

        public object Put(object item, IDeputy traceChanges = null)
        {
            Vary[] changes;
            if (traceChanges != null)
                traceable = true;

            NextVariation = (o, t) => o.Put(t, traceChanges);

            ISleeve target = item.ToSleeve();
            if (item.GetType() != type)
                setBy(target, changes = otherTypeMutations(target),
                    otherMutationsCount);
            else
                set(target, changes = sameTypeMutations(target),
                    sameMutationsCount);
            if (traceable)
                TraceEvent.Publish(item, changes);

            return item;
        }

        public E Put<E>(E item, IDeputy traceChanges = null) where E : class
        {
            Vary[] changes;
            if (traceChanges != null)
                traceable = true;

            NextVariation = (o, t) => o.Put(t, traceChanges);

            ISleeve target = item.ToSleeve();
            if (typeof(E) != type)
                setBy(target, changes = otherTypeMutations(target),
                    otherMutationsCount);
            else
                set(target, changes = sameTypeMutations(target),
                    sameMutationsCount);
            if (traceable)
                TraceEvent.Publish(item, changes);

            return item;
        }

        public E Put<E>(IDeputy traceChanges = null) where E : class
        {
            return Put(typeof(E).New<E>(), traceChanges);
        }

        public object PutSelf()
        {
            NextVariation = (c, h) => c.PutSelf();

            set(entry, sameTypeMutations(entry), sameMutationsCount);
            return Devisor;
        }

        public Vary[] Detect(object item, IDeputy traceChanges = null)
        {
            Vary[] changes;
            if (traceChanges != null)
                traceable = true;

            NextVariation = (o, t) => o.Detect(t, traceChanges);

            ISleeve target = item.ToSleeve();
            if (item.GetType() != type)
                changes = otherTypeVariations(target);
            else
                changes = sameTypeVariations(target);

            if (traceable)
                traceChanges.Publish(item, changes);

            return changes;
        }

        public Vary[] Detect<E>(E item, IDeputy traceChanges = null) where E : class
        {
            Vary[] changes;
            if (traceChanges != null)
                traceable = true;

            NextVariation = (o, t) => o.Detect(t, traceChanges);

            ISleeve target = item.ToSleeve();
            if (typeof(E) != type)
                changes = otherTypeVariations(target);
            else
                changes = sameTypeVariations(target);

            if (traceable)
                traceChanges.Publish(item, changes);

            return changes;
        }

        public void MapPreset()
        {
            var presetShell = factory.Combine(preset);
            Preset.ValueArray = presetShell.ValueArray;
        }

        public void MapEntry()
        {
            var presetShell = factory.Combine(Preset);
            presetShell.ValueArray = preset.ValueArray;
        }

        public void MapDevisor()
        {
            Preset.ValueArray = entry.ValueArray;
        }

        public void SafeMapPreset()
        {
            var presetShell = factory.Combine(preset);

            presetShell.Rubrics.Select(v =>
                preset.Rubrics.ContainsKey(v.RubricName)
                    ? preset[v.RubricName] = presetShell[v.RubricId]
                    : null).ToArray();
        }

        public void SafeMapEntry()
        {
            var presetShell = factory.Combine(preset);

            preset.Rubrics.Select(v =>
                presetShell.Rubrics.ContainsKey(v.RubricName)
                    ? presetShell[v.RubricName] = preset[v.RubricId]
                    : null).ToArray();
        }

        public void SafeMapDevisor()
        {
            Rubrics.Select(v =>
                preset.Rubrics.ContainsKey(v.RubricName)
                    ? preset[v.RubricName] = entry[v.RubricId]
                    : null).ToArray();
        }

        public object Clone()
        {
            var clone = type.New();
            var _clone = factory.Combine(clone);
            _clone.ValueArray = entry.ValueArray;
            return clone;
        }

        protected Vary[] sameTypeVariations(ISleeve target)
        {
            sameVariationCount = 0;
            var _target = target;
            var _sameVariations = new Vary[Rubrics.Count];

            Rubrics.ForEach((rubric) =>
            {
                if (!rubric.IsKey && !ExcludedRubrics.ContainsKey(rubric))
                {
                    var targetndex = rubric.RubricId;
                    var originValue = Entry[targetndex];
                    var targetValue = _target[targetndex];

                    if (!originValue.NullOrEquals(rubric.DeltaMark) &&
                        !originValue.Equals(targetValue))
                    {
                        if (targetValue == null ||
                            !trySetByRecursion(originValue, targetValue, true))
                        {
                            _sameVariations[sameVariationCount++] = new Vary()
                            {
                                TargetIndex = targetndex,
                                OriginValue = originValue
                            };
                        }
                    }
                }
            });
            return _sameVariations;
        }

        protected Vary[] otherTypeVariations(ISleeve target)
        {
            otherVariationCount = 0;
            var _target = target;
            var _customVariations = new Vary[Rubrics.Count];

            Rubrics.ForEach((originRubric) =>
            {
                if (!originRubric.IsKey && !ExcludedRubrics.ContainsKey(originRubric))
                {
                    var name = originRubric.Name;
                    if (_target.Rubrics.TryGet(name, out MemberRubric targetRubric))
                    {
                        var originValue = Entry[originRubric.RubricId];
                        var targetIndex = targetRubric.RubricId;
                        var targetValue = _target[targetIndex];

                        if (!originValue.NullOrEquals(originRubric.DeltaMark) &&
                            !originValue.Equals(targetValue))
                        {
                            if (targetValue == null ||
                                !trySetByRecursion(originValue, targetValue))
                            {
                                _customVariations[otherVariationCount++] = new Vary()
                                {
                                    TargetIndex = targetIndex,
                                    OriginValue = originValue,
                                    OriginType = originRubric.RubricType,
                                    TargetType = targetRubric.RubricType
                                };
                            }
                        }
                    }
                }
            });
            return _customVariations;
        }

        protected Vary[] sameTypeMutations(ISleeve target)
        {
            sameMutationsCount = 0;
            var _target = target;
            var _sameMutations = new Vary[Rubrics.Count];

            Rubrics.ForEach((rubric) =>
            {
                if (!rubric.IsKey && !ExcludedRubrics.ContainsKey(rubric))
                {
                    var index = rubric.RubricId;
                    var originValue = Entry[index];
                    var targetValue = _target[index];

                    if (originValue != null && (targetValue == null ||
                                                !trySetByRecursion(originValue, targetValue, true)))
                    {
                        _sameMutations[sameMutationsCount++] = new Vary()
                        {
                            TargetIndex = index,
                            OriginValue = originValue
                        };
                    }
                }
            });
            return _sameMutations;
        }

        protected Vary[] otherTypeMutations(ISleeve target)
        {
            otherMutationsCount = 0;
            var _target = target;
            var _customMutations = new Vary[Rubrics.Count];

            Rubrics.ForEach((originRubric) =>
            {
                if (!originRubric.IsKey && !ExcludedRubrics.ContainsKey(originRubric))
                {
                    var name = originRubric.Name;
                    if (_target.Rubrics.TryGet(name, out MemberRubric targetRubric))
                    {
                        var originValue = Entry[originRubric.RubricId];
                        var targetndex = targetRubric.RubricId;
                        var targetValue = _target[targetndex];

                        if (originValue != null && (targetValue == null ||
                                                    !trySetByRecursion(originValue, targetValue)))
                        {
                            _customMutations[otherMutationsCount++] = new Vary()
                            {
                                TargetIndex = targetndex,
                                OriginValue = originValue,
                                OriginType = originRubric.RubricType,
                                TargetType = targetRubric.RubricType
                            };
                        }
                    }
                }
            });
            return _customMutations;
        }

        private bool trySetByRecursion(object originValue, object targetValue, bool sameType = false)
        {
            var targetType = targetValue.GetType();
            var originType = originValue.GetType();

            if (originType.IsValueType ||
                (originType == typeof(string)))
                return false;

            if (originType.IsAssignableTo(typeof(ICollection)))
            {
                ICollection originItems = (ICollection)originValue;
                var originElementType = originType.FindElementType();
                if (originElementType == null || !originElementType.IsValueType)
                {
                    if (targetType.IsAssignableTo(typeof(ICollection)))
                    {
                        ICollection targetItems = (ICollection)targetValue;
                        var targetElementType = targetType.FindElementType();
                        if (targetElementType == null || !targetElementType.IsValueType)
                        {
                            if (targetType.IsAssignableTo(typeof(IFindable)) &&
                                originElementType.IsAssignableTo(typeof(IUnique)))
                            {
                                IFindable targetFindable = (IFindable)targetValue;

                                foreach (var originItem in originItems)
                                {
                                    var targetItem = targetFindable[originItem];

                                    if (targetItem != null)
                                        NextVariation(new Variety(originItem), targetItem);
                                    else if (!sameType)
                                        ((IList)targetItems).Add(
                                            new Variety(originItem).PutTo(targetElementType.New(), TraceEvent));
                                    else
                                        ((IList)targetItems).Add(originItem);
                                }

                                return true;
                            }

                            tryGreedyLookup(originItems, targetItems);
                        }
                    }
                }
            }

            NextVariation(new Variety(originValue), targetValue);
            return false;
        }

        private bool tryGreedyLookup(ICollection originItems, ICollection targetItems)
        {
            bool firstloop = true;
            foreach (var originItem in originItems)
            {
                if (!firstloop || originItem.GetType()
                        .IsAssignableTo(typeof(IUnique)))
                {
                    foreach (var targetItem in targetItems)
                    {
                        if (!firstloop || targetItem.GetType()
                                .IsAssignableTo(typeof(IUnique)))
                        {
                            if (((IUnique)originItem).Equals((IUnique)targetItem))
                            {
                                NextVariation(new Variety(originItem, TraceEvent), targetItem);
                            }

                            firstloop = false;
                        }
                        else return false;
                    }
                }
                else return false;
            }

            return true;
        }

        private static IRubrics excludedRubrics;

        public IRubrics ExcludedRubrics
        {
            get => excludedRubrics ??= new MemberRubrics(new MemberRubric[]
            {
                Rubrics["UniqueSeed"],
                Rubrics["EntityTypeId"],
                Rubrics["EntityOriginId"],
                Rubrics["EntityFlags"],
                Rubrics["SystemTime"],
                Rubrics["SerialCode"],
                Rubrics["sleeve"]
            });
        }

        public bool Equals(IUnique other)
        {
            return entry.Equals(other);
        }

        public int CompareTo(IUnique? other)
        {
            return entry.CompareTo(other);
        }

        public ulong UniqueKey
        {
            get => entry.UniqueKey;
            set => entry.UniqueKey = value;
        }

        public ulong UniqueSeed
        {
            get => entry.UniqueSeed;
            set => entry.UniqueSeed = value;
        }

        public byte[] GetBytes()
        {
            return entry.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return entry.GetUniqueBytes();
        }

        public object this[string propertyName]
        {
            get => GetPreset(propertyName);
            set => SetPreset(propertyName, value);
        }

        public object this[int fieldId]
        {
            get => GetPreset(fieldId);
            set => SetPreset(fieldId, value);
        }

        public object[] ValueArray
        {
            get => entry.ValueArray;
            set => entry.ValueArray = value;
        }

        public Ussn SerialCode
        {
            get => entry.SerialCode;
            set => entry.SerialCode = value;
        }

        public IRubrics Rubrics
        {
            get => entry.Rubrics;
            set => entry.Rubrics = value;
        }

        public object Devisor
        {
            get => entry.Devisor;
            set => entry.Devisor = value;
        }

        public object GetPreset(int fieldId)
        {
            if (trackset != null &&
                trackset.Contains(fieldId))
            {
                return preset[fieldId];
            }
            return entry[fieldId];
        }

        public object GetPreset(string propertyName)
        {
            if (trackset != null)
            {
                int id = 0;
                var rubric = Rubrics[propertyName];
                if (rubric != null && trackset.Contains(id =
                        Rubrics[propertyName].RubricId))
                {
                    return preset[id];
                }
                else
                    return null;
            }

            return entry[propertyName];
        }

        public int[] GetPresets()
        {
            return trackset.ToArray();
        }

        public void SetPreset(int fieldId, object value)
        {
            if (GetPreset(fieldId).Equals(value))
                return;
          
            if (trackset == null)
                trackset = new HashSet<int>();
            
            trackset.Add(fieldId);
            preset[fieldId] = value;

            //if (traceable)
            //    TraceEvent.Publish(this, value);
        }

        public void SetPreset(string propertyName, object value)
        {
            if (GetPreset(propertyName).Equals(value))
                return;

            if (trackset == null)
                trackset = new HashSet<int>();

            int id = Rubrics[propertyName].RubricId;
            trackset.Add(id);
            preset[id] = value;
        }

        public void WritePresets()
        {
            foreach (int id in trackset)
                entry[id] = preset[id];
            trackset = null;
        }

        public bool HavePresets
            => trackset != null;
    }
}