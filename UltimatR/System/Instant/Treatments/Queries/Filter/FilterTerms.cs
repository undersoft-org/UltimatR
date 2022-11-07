﻿namespace System.Instant.Treatments
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    #region Enums

    [Serializable]
    public enum OperandType
    {
        Equal,
        EqualOrMore,
        EqualOrLess,
        More,
        Less,
        Like,
        NotLike,
        Contains,
        None
    }

    [Serializable]
    public enum LogicType
    {
        And,
        Or
    }

    [Serializable]
    public enum FilterStage
    {
        None,
        First,
        Second,
        Third
    }

    #endregion

    [Serializable]
    public class FilterTerm : ICloneable, IFilterTerm
    {
        #region Fields

        public string valueTypeName;
        [NonSerialized] private IFigures figures;
        [NonSerialized] private Type valueType;

        #endregion

        #region Constructors

        public FilterTerm()
        {
            Stage = FilterStage.First;
        }

        public FilterTerm(IFigures figures)
        {
            Stage = FilterStage.First;
            this.figures = figures;
        }

        public FilterTerm(IFigures figures, string filterColumn, string operand, object value, string logic = "And",
            int stage = 1)
        {
            RubricName = filterColumn;
            OperandType tempOperand1;
            Enum.TryParse(operand, true, out tempOperand1);
            Operand = tempOperand1;
            Value = value;
            LogicType tempLogic;
            Enum.TryParse(logic, true, out tempLogic);
            Logic = tempLogic;
            this.figures = figures;
            if (figures != null)
            {
                MemberRubric[] filterRubrics =
                    this.figures.Rubrics.AsValues().Where(c => c.RubricName == RubricName).ToArray();
                if (filterRubrics.Length > 0)
                {
                    FilterRubric = filterRubrics[0];
                    ValueType = FilterRubric.RubricType;
                }
            }

            Stage = (FilterStage)Enum.ToObject(typeof(FilterStage), stage);
        }

        public FilterTerm(MemberRubric filterColumn, OperandType operand, object value, LogicType logic = LogicType.And,
            FilterStage stage = FilterStage.First)
        {
            Operand = operand;
            Value = value;
            Logic = logic;
            ValueType = filterColumn.RubricType;
            RubricName = filterColumn.RubricName;
            FilterRubric = filterColumn;
            Stage = stage;
        }

        public FilterTerm(string filterColumn, OperandType operand, object value, LogicType logic = LogicType.And,
            FilterStage stage = FilterStage.First)
        {
            RubricName = filterColumn;
            Operand = operand;
            Value = value;
            Logic = logic;
            Stage = stage;
        }

        public FilterTerm(string filterColumn, string operand, object value, string logic = "And", int stage = 1)
        {
            RubricName = filterColumn;
            OperandType tempOperand1;
            Enum.TryParse(operand, true, out tempOperand1);
            Operand = tempOperand1;
            Value = value;
            LogicType tempLogic;
            Enum.TryParse(logic, true, out tempLogic);
            Logic = tempLogic;
            Stage = (FilterStage)Enum.ToObject(typeof(FilterStage), stage);
        }

        #endregion

        #region Properties

        public IFigures Figures
        {
            get { return figures; }
            set
            {
                figures = value;
                if (FilterRubric == null && value != null)
                {
                    MemberRubric[] filterRubrics = figures.Rubrics.AsValues()
                        .Where(c => c.RubricName == RubricName).ToArray();
                    if (filterRubrics.Length > 0)
                    {
                        FilterRubric = filterRubrics[0];
                        ValueType = FilterRubric.RubricType;
                    }
                }
            }
        }

        public MemberRubric FilterRubric { get; set; }

        [DisplayName("Pos")] public int Index { get; set; }

        public LogicType Logic { get; set; }

        public OperandType Operand { get; set; }

        public string RubricName { get; set; }

        public FilterStage Stage { get; set; } = FilterStage.First;

        public object Value { get; set; }

        public Type ValueType
        {
            get
            {
                if (valueType == null && valueTypeName != null)
                    valueType = Type.GetType(valueTypeName);
                return valueType;
            }
            set
            {
                valueType = value;
                valueTypeName = value.FullName;
            }
        }

        #endregion

        #region Methods

        public object Clone()
        {
            FilterTerm clone = (FilterTerm)this.MemberwiseClone();
            clone.FilterRubric = FilterRubric;
            return clone;
        }

        public FilterTerm Clone(object value)
        {
            FilterTerm clone = (FilterTerm)this.MemberwiseClone();
            clone.FilterRubric = FilterRubric;
            clone.Value = value;
            return clone;
        }

        public bool Compare(FilterTerm term)
        {
            if (RubricName != term.RubricName)
                return false;
            if (!Value.Equals(term.Value))
                return false;
            if (!Operand.Equals(term.Operand))
                return false;
            if (!Stage.Equals(term.Stage))
                return false;
            if (!Logic.Equals(term.Logic))
                return false;

            return true;
        }

        #endregion
    }

    [Serializable]
    public class FilterTerms : Collection<FilterTerm>, ICollection
    {
        #region Fields

        [NonSerialized] private IFigures figures;

        #endregion

        #region Constructors

        public FilterTerms() { }

        public FilterTerms(IFigures figures)
        {
            this.Figures = figures;
        }

        #endregion

        #region Properties

        public IFigures Figures
        {
            get { return figures; }
            set { figures = value; }
        }

        public bool IsReadOnly => throw new NotImplementedException();

        #endregion

        #region Methods

        public new int Add(FilterTerm value)
        {
            value.Figures = figures;
            value.Index = ((IList)this).Add(value);
            return value.Index;
        }

        public void Add(ICollection<FilterTerm> terms)
        {
            foreach (FilterTerm term in terms)
            {
                term.Figures = Figures;
                term.Index = Add(term);
            }
        }

        public void Add(IFilterTerm item)
        {
            Add(new FilterTerm(item.RubricName, item.Operand, item.Value, item.Logic, item.Stage));
        }

        public object AddNew()
        {
            return (object)((IBindingList)this).AddNew();
        }

        public FilterTerms Clone()
        {
            FilterTerms ft = new FilterTerms();
            foreach (FilterTerm t in this)
            {
                FilterTerm _t = new FilterTerm(t.RubricName, t.Operand, t.Value, t.Logic, t.Stage);
                ft.Add(_t);
            }

            return ft;
        }

        public bool Contains(IFilterTerm item)
        {
            return Contains(new FilterTerm(item.RubricName, item.Operand, item.Value, item.Logic, item.Stage));
        }

        public bool Contains(string RubricName)
        {
            return this.AsEnumerable().Where(c => c.RubricName == RubricName).Any();
        }

        public void CopyTo(IFilterTerm[] array, int arrayIndex)
        {
            Array.Copy(this.AsQueryable().Cast<IFilterTerm>().ToArray(), array, Count);
        }

        public FilterTerm Find(FilterTerm data)
        {
            foreach (FilterTerm lDetailValue in this)
                if (lDetailValue == data)
                    return lDetailValue;
            return null;
        }

        public List<FilterTerm> Get(int stage)
        {
            FilterStage filterStage = (FilterStage)Enum.ToObject(typeof(FilterStage), stage);
            return this.AsEnumerable().Where(c => filterStage.Equals(c.Stage)).ToList();
        }

        public List<FilterTerm> Get(List<string> RubricNames)
        {
            return this.AsEnumerable().Where(c => RubricNames.Contains(c.FilterRubric.RubricName)).ToList();
        }

        public FilterTerm[] Get(string RubricName)
        {
            return this.AsEnumerable().Where(c => c.RubricName == RubricName).ToArray();
        }

        public int IndexOf(object value)
        {
            for (int i = 0; i < Count; i++)
                if (ReferenceEquals(this[i], value))
                    return i;
            return -1;
        }

        public void Remove(ICollection<FilterTerm> value)
        {
            foreach (FilterTerm term in value)
                Remove(term);
        }

        public bool Remove(IFilterTerm item)
        {
            return Remove(new FilterTerm(item.RubricName, item.Operand, item.Value, item.Logic, item.Stage));
        }

        public void Renew(ICollection<FilterTerm> terms)
        {
            bool diffs = false;
            if (Count != terms.Count)
            {
                diffs = true;
            }
            else
            {
                foreach (FilterTerm term in terms)
                {
                    if (Contains(term.RubricName))
                    {
                        int same = 0;
                        foreach (FilterTerm myterm in Get(term.RubricName))
                        {
                            if (!myterm.Compare(term))
                                same++;
                        }

                        if (same != 0)
                        {
                            diffs = true;
                            break;
                        }
                    }
                    else
                    {
                        diffs = true;
                        break;
                    }
                }
            }

            if (diffs)
            {
                Clear();
                foreach (FilterTerm term in terms)
                    Add(term);
            }
        }

        public void Reset()
        {
            this.Clear();
        }

        public void SetRange(FilterTerm[] data)
        {
            for (int i = 0; i < data.Length; i++)
                this[i] = data[i];
        }

        #endregion
    }
}