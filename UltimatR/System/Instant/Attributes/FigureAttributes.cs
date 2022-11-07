
// <copyright file="FigureAttributes.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Instant.Treatments;
    using System.Runtime.InteropServices;




    /// <summary>
    /// Class FigureAsAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.FigureAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.FigureAttribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureAsAttribute : FigureAttribute
    {
        #region Fields

        /// <summary>
        /// The size constant
        /// </summary>
        public int SizeConst;
        /// <summary>
        /// The value
        /// </summary>
        public UnmanagedType Value;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="FigureAsAttribute" /> class.
        /// </summary>
        /// <param name="unmanaged">The unmanaged.</param>
        public FigureAsAttribute(UnmanagedType unmanaged)
        {
            Value = unmanaged;
        }

        #endregion
    }




    /// <summary>
    /// Class FigureAttribute.
    /// Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureAttribute : Attribute
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureAttribute" /> class.
        /// </summary>
        public FigureAttribute()
        {
        }

        #endregion
    }




    /// <summary>
    /// Class FigureDisplayAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.FigureAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.FigureAttribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureDisplayAttribute : FigureAttribute
    {
        #region Fields

        /// <summary>
        /// The name
        /// </summary>
        public string Name;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="FigureDisplayAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public FigureDisplayAttribute(string name)
        {
            Name = name;
        }

        #endregion
    }




    /// <summary>
    /// Class FigureIdentityAttribute.
    /// Implements the <see cref="System.Instant.FigureAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.FigureAttribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureIdentityAttribute : FigureAttribute
    {
        #region Fields

        /// <summary>
        /// The is autoincrement
        /// </summary>
        public bool IsAutoincrement = false;
        /// <summary>
        /// The order
        /// </summary>
        public short Order = 0;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureIdentityAttribute" /> class.
        /// </summary>
        public FigureIdentityAttribute()
        {
        }

        #endregion
    }




    /// <summary>
    /// Class FigureKeyAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.FigureIdentityAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.FigureIdentityAttribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureKeyAttribute : FigureIdentityAttribute
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureKeyAttribute" /> class.
        /// </summary>
        public FigureKeyAttribute()
        {
        }

        #endregion
    }




    /// <summary>
    /// Class FigureLinkAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.FigureAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.FigureAttribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureLinkAttribute : FigureAttribute
    {
        #region Fields

        /// <summary>
        /// The link rubric
        /// </summary>
        public string LinkRubric;
        /// <summary>
        /// The target name
        /// </summary>
        public string TargetName;
        /// <summary>
        /// The target type
        /// </summary>
        public Type TargetType;

        #endregion

        #region Constructors






        /// <summary>
        /// Initializes a new instance of the <see cref="FigureLinkAttribute" /> class.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <param name="linkRubric">The link rubric.</param>
        public FigureLinkAttribute(string targetName, string linkRubric)
        {
            TargetName = targetName;
            LinkRubric = linkRubric;
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="FigureLinkAttribute" /> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="linkRubric">The link rubric.</param>
        public FigureLinkAttribute(Type targetType, string linkRubric)
        {
            TargetType = targetType;
            TargetName = targetType.Name;
            LinkRubric = linkRubric;
        }

        #endregion
    }




    /// <summary>
    /// Class FigureRequiredAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.FigureAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.FigureAttribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureRequiredAttribute : FigureAttribute
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureRequiredAttribute" /> class.
        /// </summary>
        public FigureRequiredAttribute()
        {
        }

        #endregion
    }




    /// <summary>
    /// Class FigureSizeAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.FigureAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.FigureAttribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class FigureSizeAttribute : FigureAttribute
    {
        #region Fields

        /// <summary>
        /// The size constant
        /// </summary>
        public int SizeConst;
        /// <summary>
        /// The value
        /// </summary>
        public UnmanagedType Value;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="FigureSizeAttribute" /> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public FigureSizeAttribute(int size)
        {
            SizeConst = size;
        }

        #endregion
    }




    /// <summary>
    /// Class FigureTreatmentAttribute.
    /// Implements the <see cref="System.Instant.FigureAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.FigureAttribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FigureTreatmentAttribute : FigureAttribute
    {
        #region Fields

        /// <summary>
        /// The aggregate operand
        /// </summary>
        public AggregateOperand AggregateOperand = AggregateOperand.None;
        /// <summary>
        /// The summary operand
        /// </summary>
        public AggregateOperand SummaryOperand = AggregateOperand.None;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="FigureTreatmentAttribute" /> class.
        /// </summary>
        public FigureTreatmentAttribute()
        {
        }

        #endregion
    }
}
