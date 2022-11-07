

/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;




    /// <summary>
    /// Class CompilerConstructors.
    /// </summary>
    public class CompilerConstructors
    {
        #region Fields

        /// <summary>
        /// The data member ctor
        /// </summary>
        protected readonly ConstructorInfo dataMemberCtor = typeof(DataMemberAttribute).GetConstructor(Type.EmptyTypes);
        /// <summary>
        /// The data member props
        /// </summary>
        protected readonly PropertyInfo[]  dataMemberProps = new[] { typeof(DataMemberAttribute).GetProperty("Order"),
                                                                        typeof(DataMemberAttribute).GetProperty("Name") };
        /// <summary>
        /// The figure display ctor
        /// </summary>
        protected readonly ConstructorInfo figureDisplayCtor = typeof(FigureDisplayAttribute).GetConstructor(new Type[] { typeof(string) });
        /// <summary>
        /// The figure identity ctor
        /// </summary>
        protected readonly ConstructorInfo figureIdentityCtor = typeof(FigureIdentityAttribute).GetConstructor(Type.EmptyTypes);
        /// <summary>
        /// The figure key ctor
        /// </summary>
        protected readonly ConstructorInfo figureKeyCtor = typeof(FigureKeyAttribute).GetConstructor(Type.EmptyTypes);
        /// <summary>
        /// The key ctor
        /// </summary>
        protected readonly ConstructorInfo keyCtor = typeof(KeyAttribute).GetConstructor(Type.EmptyTypes);
        /// <summary>
        /// The figure required ctor
        /// </summary>
        protected readonly ConstructorInfo figureRequiredCtor = typeof(FigureRequiredAttribute).GetConstructor(Type.EmptyTypes);
        /// <summary>
        /// The figure link ctor
        /// </summary>
        protected readonly ConstructorInfo figureLinkCtor = typeof(FigureLinkAttribute).GetConstructor(Type.EmptyTypes);
        /// <summary>
        /// The required ctor
        /// </summary>
        protected readonly ConstructorInfo requiredCtor = typeof(RequiredAttribute).GetConstructor(Type.EmptyTypes);
        /// <summary>
        /// The figures treatment ctor
        /// </summary>
        protected readonly ConstructorInfo figuresTreatmentCtor = typeof(FigureTreatmentAttribute).GetConstructor(Type.EmptyTypes);
        /// <summary>
        /// The marshal as ctor
        /// </summary>
        protected readonly ConstructorInfo marshalAsCtor = typeof(MarshalAsAttribute).GetConstructor(new Type[] { typeof(UnmanagedType) });
        /// <summary>
        /// The structure layout ctor
        /// </summary>
        protected readonly ConstructorInfo structLayoutCtor = typeof(StructLayoutAttribute).GetConstructor(new Type[] { typeof(LayoutKind) });
        /// <summary>
        /// The structure layout fields
        /// </summary>
        protected readonly FieldInfo[] structLayoutFields = new[] { typeof(StructLayoutAttribute).GetField("CharSet"),
                                                                        typeof(StructLayoutAttribute).GetField("Pack") };

        #endregion
    }
}
