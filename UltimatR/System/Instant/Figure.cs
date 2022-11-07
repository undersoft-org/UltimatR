
namespace System.Instant
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Series;

    public class Figure<T> : Figure
    {
        public Figure(FigureMode modeType = FigureMode.Reference) : base(typeof(T), modeType) { }
        public Figure(string figureTypeName, FigureMode modeType = FigureMode.Reference) : base(typeof(T), figureTypeName, modeType) { }
    }

    public class Figure : IInstant
    {
        #region Fields
        public InstantBuilder instantBuilder = new InstantBuilder();
        public IDeck<RubricBuilder> rubricBuilders = new Catalog<RubricBuilder>();
        private MemberRubric[] memberRubrics;
        private Type compiledType;

        #endregion

        #region Constructors

        public Figure(IList<MemberInfo> figureMembers, FigureMode modeType = FigureMode.Reference)
            : this(figureMembers.ToArray(), null, modeType)
        {
        }
        public Figure(IList<MemberInfo> figureMembers, string figureTypeName, FigureMode modeType = FigureMode.Reference)
        {
            Name = (figureTypeName != null
                && figureTypeName != "")
                 ? figureTypeName
                 : DateTime.Now.ToBinary().ToString();

            mode = modeType;

            instantBuilder = new InstantBuilder();

            rubricBuilders = instantBuilder
                .CreateBuilders(instantBuilder
                .PrepareMembers(figureMembers));

            Rubrics = new MemberRubrics(rubricBuilders.Select(m => m.Member).ToArray());
            Rubrics.KeyRubrics = new MemberRubrics();
        }
        public Figure(MemberRubrics figureRubrics, string figureTypeName, FigureMode modeType = FigureMode.Reference)
            : this(figureRubrics.ToArray(), figureTypeName, modeType)
        {
        }
        public Figure(Type figureModelType, FigureMode modeType = FigureMode.Reference) : this(figureModelType, null, modeType)
        {
        }
        public Figure(Type figureModelType, string figureTypeName, FigureMode modeType = FigureMode.Reference)
        {
            BaseType = figureModelType;

            if (modeType == FigureMode.Derived)
                IsDerived = true;

            Name = figureTypeName == null
                 ? figureModelType.Name
                 : figureTypeName;
            mode = modeType;

            instantBuilder = new InstantBuilder();
            rubricBuilders = instantBuilder.CreateBuilders(figureModelType);

            Rubrics = new MemberRubrics(rubricBuilders.Select(m => m.Member).ToArray());
            Rubrics.KeyRubrics = new MemberRubrics();
        }

        #endregion

        #region Properties

        public Type BaseType { get; set; }

        public bool IsDerived { get; set; }

        public string Name { get; set; }

        public IRubrics Rubrics { get; set; }

        public int Size { get; set; }

        public Type Type { get; set; }

        private FigureMode mode { get; set; }

        #endregion

        #region Methods

        public IFigure Combine()
        {
            if (this.Type == null)
            {
                try
                {
                    switch (mode)
                    {
                        case FigureMode.Reference:
                            combineDynamicType(new FigureCompileReferenceType(this, rubricBuilders));
                            break;
                        case FigureMode.ValueType:
                            combineDynamicType(new FigureCompileValueType(this, rubricBuilders));
                            break;
                        case FigureMode.Derived:
                            combineDerivedType(new FigureCompileDerivedType(this, rubricBuilders));
                            break;
                        default:
                            break;
                    }

                    Rubrics.Update();
                }
                catch (Exception ex)
                {
                    throw new FigureCompilerException("Sleeve compilation at runtime failed see inner exception", ex);
                }
            }
            return newFigure();
        }

        public object New()
        {
            if (this.Type == null)
                return Combine();
            return this.Type.New();
        }

        private IFigure newFigure()
        {
            if (this.Type == null)
                return Combine();
            return (IFigure)this.Type.New();
        }

        private void combineDerivedType(FigureCompiler compiler)
        {
            var fcdt = compiler;
            compiledType = fcdt.CompileFigureType(Name);
            Rubrics.KeyRubrics.Add(fcdt.Identities.Values);
            Type = compiledType.New().GetType();

            if (!(Rubrics.AsValues().Any(m => m.Name == "SerialCode")))
            {
                var f = this.Type.GetField("serialcode", BindingFlags.NonPublic | BindingFlags.Instance);

                if (!Rubrics.TryGet("serialcode", out MemberRubric mr))
                {
                    mr = new MemberRubric(f);
                    mr.FigureField = f;
                    Rubrics.Insert(0, mr);
                }
                mr.RubricName = "SerialCode";
            }
        }

        private void combineDynamicType(FigureCompiler compiler)
        {
            var fcvt = compiler;
            compiledType = fcvt.CompileFigureType(Name);
            Rubrics.KeyRubrics.Add(fcvt.Identities.Values);
            Type = compiledType.New().GetType();
            Size = Marshal.SizeOf(Type);
        }

        private MemberRubric[] createMemberRurics(IList<MemberInfo> membersInfo)
        {
            return membersInfo.Select(m => !(m is MemberRubric rubric) ? m.MemberType == MemberTypes.Field ? new MemberRubric((FieldInfo)m) :
                                                                            m.MemberType == MemberTypes.Property ? new MemberRubric((PropertyInfo)m) :
                                                                            null : rubric).Where(p => p != null).ToArray();
        }

        #endregion
    }

    public class FigureCompilerException : Exception
    {
        public FigureCompilerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
