
// <copyright file="FunctionOperation.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;




    /// <summary>
    /// Class FunctionOperation.
    /// Implements the <see cref="System.Instant.Mathset.UnsignedOperator" />
    /// </summary>
    /// <seealso cref="System.Instant.Mathset.UnsignedOperator" />
    [Serializable]
    public class FunctionOperation : UnsignedOperator
    {
        #region Fields

        /// <summary>
        /// The effx
        /// </summary>
        internal FunctionType effx;

        #endregion

        #region Constructors






        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOperation" /> class.
        /// </summary>
        /// <param name="ee">The ee.</param>
        /// <param name="fx">The fx.</param>
        public FunctionOperation(Formula ee, FunctionType fx) : base(ee)
        {
            effx = fx;
        }

        #endregion

        #region Enums

        /// <summary>
        /// Enum FunctionType
        /// </summary>
        public enum FunctionType { Cos, Sin, Ln, Log };

        #endregion

        #region Properties

















        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override MathsetSize Size
        {
            get { return e.Size; }
        }

        #endregion

        #region Methods






        /// <summary>
        /// Compiles the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="cc">The cc.</param>
        public override void Compile(ILGenerator g, CompilerContext cc)
        {
            if (cc.IsFirstPass())
            {
                e.Compile(g, cc);
                return;
            }
            MethodInfo mi = null;

            switch (effx)
            {
                case FunctionType.Cos: mi = typeof(Math).GetMethod("Cos"); break;
                case FunctionType.Sin: mi = typeof(Math).GetMethod("Sin"); break;
                case FunctionType.Ln: mi = typeof(Math).GetMethod("Log"); break;
                case FunctionType.Log: mi = typeof(Math).GetMethod("Log10"); break;
                default:
                    break;
            }
            if (mi == null) return;

            e.Compile(g, cc);

            g.EmitCall(OpCodes.Call, mi, null);
        }

        #endregion
    }
}
