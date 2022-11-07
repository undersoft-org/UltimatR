
// <copyright file="FigureCard.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>




/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Extract;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Series;
    using System.Uniques;

    /// <summary>
    /// Class FigureCard.
    /// Implements the <see cref="System.Series.CardBase{System.Instant.IFigure}" />
    /// Implements the <see cref="System.Instant.IFigure" />
    /// Implements the <see cref="System.IEquatable{System.Instant.IFigure}" />
    /// Implements the <see cref="System.IComparable{System.Instant.IFigure}" />
    /// </summary>
    /// <seealso cref="System.Series.CardBase{System.Instant.IFigure}" />
    /// <seealso cref="System.Instant.IFigure" />
    /// <seealso cref="System.IEquatable{System.Instant.IFigure}" />
    /// <seealso cref="System.IComparable{System.Instant.IFigure}" />
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class FigureCard : CardBase<IFigure>, IFigure, IEquatable<IFigure>, IComparable<IFigure>
    {
        /// <summary>
        /// The presets
        /// </summary>
        private IDeck<object> presets;

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureCard" /> class.
        /// </summary>
        /// <param name="figures">The figures.</param>
        public FigureCard(IFigures figures)
        {
            Figures = figures;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureCard" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="figures">The figures.</param>
        public FigureCard(object key, IFigure value, IFigures figures) : base(key, value)
        {
            Figures = figures;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureCard" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="figures">The figures.</param>
        public FigureCard(ulong key, IFigure value, IFigures figures) : base(key, value)
        {
            Figures = figures;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureCard" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="figures">The figures.</param>
        public FigureCard(IFigure value, IFigures figures) : base(value)
        {
            Figures = figures;
            CompactKey();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureCard" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="figures">The figures.</param>
        public FigureCard(ICard<IFigure> value, IFigures figures) : base(value)
        {
            Figures = figures;
            CompactKey();
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified field identifier.
        /// </summary>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>System.Object.</returns>
        public object this[int fieldId]
        {
            get => GetPreset(fieldId);
            set => SetPreset(fieldId, value);
        }
        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public object this[string propertyName]
        {
            get => GetPreset(propertyName);
            set => SetPreset(propertyName, value);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public override void Set(object key, IFigure value)
        {
            this.value = value;
            this.value.UniqueKey = key.UniqueKey();
        }
        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void Set(IFigure value)
        {
            this.value = value;
        }
        /// <summary>
        /// Sets the specified card.
        /// </summary>
        /// <param name="card">The card.</param>
        public override void Set(ICard<IFigure> card)
        {
            this.value = card.Value;
        }

        /// <summary>
        /// Equalses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Equals(ulong key)
        {
            return Key == key;
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="y">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object y)
        {
            return Key.Equals(y.UniqueKey());
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(IFigure other)
        {
            return Key == other.UniqueKey;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Value.GetUniqueBytes().BitAggregate64to32().ToInt32();
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(object other)
        {
            return (int)(Key - other.UniqueKey64());
        }
        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }
        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public override int CompareTo(ICard<IFigure> other)
        {
            return (int)(Key - other.Key);
        }
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public int CompareTo(IFigure other)
        {
            return (int)(Key - other.UniqueKey);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public override byte[] GetBytes()
        {
            if (!Figures.Prime && presets != null)
            {

                IFigure f = Figures.NewFigure();
                f.ValueArray = ValueArray;
                f.SerialCode = value.SerialCode;
                byte[] ba = f.GetBytes();
                f = null;
                return ba;
            }
            else
                return value.GetBytes();
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public unsafe override byte[] GetUniqueBytes()
        {
            return value.GetUniqueBytes();
        }

        /// <summary>
        /// Uniques the ordinals.
        /// </summary>
        /// <returns>System.Int32[].</returns>
        public override int[] UniqueOrdinals()
        {
            return Figures.KeyRubrics.Ordinals;
        }

        /// <summary>
        /// Uniques the values.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public override object[] UniqueValues()
        {
            int[] ordinals = UniqueOrdinals();
            if (ordinals != null)
                return ordinals.Select(x => value[x]).ToArray();
            return null;
        }

        /// <summary>
        /// Compacts the key.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        public override ulong CompactKey()
        {
            ulong key = value.UniqueKey;
            if (key == 0)
            {
                IRubrics r = Figures.KeyRubrics;
                var objs = r.Ordinals.Select(x => value[x]).ToArray();
                key = objs.Any() ? objs.UniqueKey64(r.BinarySizes, r.BinarySize) : Unique.New;
                value.UniqueKey = key;
            }
            return key;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public override ulong Key
        {
            get => value.UniqueKey;
            set => this.value.UniqueKey = value;
        }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public override ulong UniqueKey
        {
            get => value.UniqueKey;
            set => this.value.UniqueKey = value;
        }

        /// <summary>
        /// Gets or sets the value array.
        /// </summary>
        /// <value>The value array.</value>
        public object[] ValueArray
        {
            get
            {
                if (Figures.Prime || presets == null)
                    return value.ValueArray;
                object[] valarr = value.ValueArray;
                presets.AsCards().Select(x => valarr[x.Key] = x.Value).ToArray();
                return valarr;
            }
            set
            {
                int l = value.Length;
                for (int i = 0; i < l; i++)
                    SetPreset(i, value[i]);
            }
        }

        /// <summary>
        /// Gets or sets the serial code.
        /// </summary>
        /// <value>The serial code.</value>
        public Ussn SerialCode
        {
            get => value.SerialCode;
            set => this.value.SerialCode = value;
        }

        /// <summary>
        /// Gets or sets the figures.
        /// </summary>
        /// <value>The figures.</value>
        public IFigures Figures { get; set; }

        /// <summary>
        /// Gets the preset.
        /// </summary>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>System.Object.</returns>
        public object GetPreset(int fieldId)
        {
            if (presets != null && !Figures.Prime)
            {
                object val = presets.Get(fieldId);
                if (val != null)
                    return val;
            }
            return value[fieldId];
        }
        /// <summary>
        /// Gets the preset.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Field doesn't exist</exception>
        public object GetPreset(string propertyName)
        {
            if (presets != null && !Figures.Prime)
            {
                MemberRubric rubric = Figures.Rubrics[propertyName.UniqueKey()];
                if (rubric != null)
                {
                    object val = presets.Get(rubric.FieldId);
                    if (val != null)
                        return val;
                }
                else
                    throw new IndexOutOfRangeException("Field doesn't exist");
            }
            return value[propertyName];
        }

        /// <summary>
        /// Gets the presets.
        /// </summary>
        /// <returns>ICard&lt;System.Object&gt;[].</returns>
        public ICard<object>[] GetPresets()
        {
            return presets.AsCards().ToArray();
        }

        /// <summary>
        /// Sets the preset.
        /// </summary>
        /// <param name="fieldId">The field identifier.</param>
        /// <param name="value">The value.</param>
        public void SetPreset(int fieldId, object value)
        {
            if (GetPreset(fieldId).Equals(value))
                return;
            if (!Figures.Prime)
            {
                if (presets == null)
                    presets = new Deck<object>(9);
                presets.Put(fieldId, value);
            }
            else
                this.value[fieldId] = value;
        }
        /// <summary>
        /// Sets the preset.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.IndexOutOfRangeException">Field doesn't exist</exception>
        public void SetPreset(string propertyName, object value)
        {
            MemberRubric rubric = Figures.Rubrics[propertyName.UniqueKey()];
            if (rubric != null)
                SetPreset(rubric.FieldId, value);
            else
                throw new IndexOutOfRangeException("Field doesn't exist");
        }

        /// <summary>
        /// Writes the presets.
        /// </summary>
        public void WritePresets()
        {
            foreach (var c in presets.AsCards())
                value[(int)c.Key] = c.Value;
            presets = null;
        }

        /// <summary>
        /// Gets a value indicating whether [have presets].
        /// </summary>
        /// <value><c>true</c> if [have presets]; otherwise, <c>false</c>.</value>
        public bool HavePresets
          => presets != null ?
          true :
          false;
    }
}
