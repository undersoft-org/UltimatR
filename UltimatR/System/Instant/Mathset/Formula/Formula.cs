
// <copyright file="Formula.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System.Reflection.Emit;




    /// <summary>
    /// Class Formula.
    /// </summary>
    [Serializable]
    public abstract class Formula
    {
        #region Fields

        /// <summary>
        /// The combined formula
        /// </summary>
        [NonSerialized] public CombinedFormula CombinedFormula;
        /// <summary>
        /// The left formula
        /// </summary>
        [NonSerialized] public Formula LeftFormula;
        /// <summary>
        /// The right formula
        /// </summary>
        [NonSerialized] public Formula RightFormula;

        #endregion

        #region Properties




        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public virtual MathsetSize Size
        {
            get
            {
                return new MathsetSize(0, 0);
            }
        }

        #endregion

        #region Methods






        /// <summary>
        /// Coses the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>Formula.</returns>
        public static Formula Cos(Formula e)
        {
            return new FunctionOperation(e, FunctionOperation.FunctionType.Cos);
        }






        /// <summary>
        /// Logs the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>Formula.</returns>
        public static Formula Log(Formula e)
        {
            return new FunctionOperation(e, FunctionOperation.FunctionType.Log);
        }







        /// <summary>
        /// Memories the pow.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>Formula.</returns>
        public static Formula MemPow(Formula e1, Formula e2)
        {
            return new PowerOperation(e1, e2);
        }






        /// <summary>
        /// Sins the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>Formula.</returns>
        public static Formula Sin(Formula e)
        {
            return new FunctionOperation(e, FunctionOperation.FunctionType.Sin);
        }






        /// <summary>
        /// Combines the mathset.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns>CombinedMathset.</returns>
        public CombinedMathset CombineMathset(CombinedFormula m)
        {
            return Compiler.Compile(m);
        }







        /// <summary>
        /// Combines the mathset.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="m">The m.</param>
        /// <returns>CombinedMathset.</returns>
        public CombinedMathset CombineMathset(Formula f, LeftFormula m)
        {
            CombinedMathset mathline = Compiler.Compile(new CombinedFormula(m, f));
            return mathline;
        }







        /// <summary>
        /// Compiles the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="cc">The cc.</param>
        public abstract void Compile(ILGenerator g, CompilerContext cc);





        /// <summary>
        /// Computes the specified cm.
        /// </summary>
        /// <param name="cm">The cm.</param>
        public void Compute(CombinedMathset cm)
        {
            Evaluator e = new Evaluator(cm.Compute);
            e();
        }






        /// <summary>
        /// Creates the evaluator.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns>Evaluator.</returns>
        public Evaluator CreateEvaluator(CombinedFormula m)
        {
            CombinedMathset mathline = CombineMathset(m);
            Evaluator ev = new Evaluator(mathline.Compute);
            return ev;
        }






        /// <summary>
        /// Creates the evaluator.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>Evaluator.</returns>
        public Evaluator CreateEvaluator(CombinedMathset e)
        {
            Evaluator ev = new Evaluator(e.Compute);
            return ev;
        }







        /// <summary>
        /// Creates the evaluator.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="m">The m.</param>
        /// <returns>Evaluator.</returns>
        public Evaluator CreateEvaluator(Formula f, LeftFormula m)
        {
            CombinedMathset mathline = CombineMathset(f, m);
            Evaluator ev = new Evaluator(mathline.Compute);
            return ev;
        }






        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="o">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object o)
        {
            if (o == null)
                return false;
            return this.Equals(o);
        }





        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }







        /// <summary>
        /// Pows the specified e2.
        /// </summary>
        /// <param name="e2">The e2.</param>
        /// <returns>Formula.</returns>
        public Formula Pow(Formula e2)
        {
            return new PowerOperation(this, e2);
        }








        /// <summary>
        /// Prepares the specified f.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="m">The m.</param>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        /// <returns>CombinedFormula.</returns>
        public CombinedFormula Prepare(Formula f, LeftFormula m, bool partial = false)
        {
            CombinedFormula = new CombinedFormula(m, f, partial);
            CombinedFormula.LeftFormula = m;
            CombinedFormula.RightFormula = f;
            return CombinedFormula;
        }







        /// <summary>
        /// Prepares the specified m.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        /// <returns>CombinedFormula.</returns>
        public CombinedFormula Prepare(LeftFormula m, bool partial = false)
        {
            CombinedFormula = new CombinedFormula(m, this, partial);
            CombinedFormula.LeftFormula = m;
            CombinedFormula.RightFormula = this;
            return CombinedFormula;
        }





        /// <summary>
        /// Transposes this instance.
        /// </summary>
        /// <returns>Formula.</returns>
        public Formula Transpose()
        {
            return new TransposeOperation(this);
        }

        #endregion



        /// <summary>
        /// Implements the + operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator +(Formula e1, Formula e2)
        {
            return new BinaryOperation(e1, e2, new Plus());
        }


        /// <summary>
        /// Implements the - operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator -(Formula e1, Formula e2)
        {
            return new BinaryOperation(e1, e2, new Minus());
        }


        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator *(Formula e1, Formula e2)
        {
            return new BinaryOperation(e1, e2, new Multiply());
        }


        /// <summary>
        /// Implements the / operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator /(Formula e1, Formula e2)
        {
            return new BinaryOperation(e1, e2, new Divide());
        }


        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator ==(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new Equal());
        }


        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator !=(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new NotEqual());
        }


        /// <summary>
        /// Implements the &lt; operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator <(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new Less());
        }


        /// <summary>
        /// Implements the | operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator |(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new OrOperand());
        }


        /// <summary>
        /// Implements the &gt; operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator >(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new Greater());
        }


        /// <summary>
        /// Implements the &amp; operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>The result of the operator.</returns>
        public static Formula operator &(Formula e1, Formula e2)
        {
            return new CompareOperation(e1, e2, new AndOperand());
        }


        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="o">The o.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Formula e1, object o)
        {
            if (o == null)
                return NullCheck.IsNotNull(e1);
            else
                return !e1.Equals(o);
        }


        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="o">The o.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Formula e1, object o)
        {
            if (o == null)
                return NullCheck.IsNull(e1);
            else
                return e1.Equals(o);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double" /> to <see cref="Formula" />.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Formula(double f)
        {
            return new UnsignedFormula(f);
        }
    }




    /// <summary>
    /// Class NullCheck.
    /// </summary>
    public static class NullCheck
    {
        #region Methods






        /// <summary>
        /// Determines whether [is not null] [the specified o].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns><c>true</c> if [is not null] [the specified o]; otherwise, <c>false</c>.</returns>
        public static bool IsNotNull(object o)
        {
            if (o is ValueType)
                return false;
            else
                return !ReferenceEquals(o, null);
        }






        /// <summary>
        /// Determines whether the specified o is null.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns><c>true</c> if the specified o is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull(object o)
        {
            if (o is ValueType)
                return false;
            else
                return ReferenceEquals(o, null);
        }

        #endregion
    }




    /// <summary>
    /// Class SizeMismatchException.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class SizeMismatchException : Exception
    {
        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="SizeMismatchException" /> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public SizeMismatchException(string s) : base(s)
        {
        }

        #endregion
    }
}
