
// <copyright file="CompilerContext.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Mathset namespace.
/// </summary>
namespace System.Instant.Mathset
{
    using System;
    using System.Reflection.Emit;

    #region Delegates




    /// <summary>
    /// Delegate Evaluator
    /// </summary>
    public delegate void Evaluator();

    #endregion




    /// <summary>
    /// Class CompilerContext.
    /// </summary>
    [Serializable]
    public class CompilerContext
    {
        #region Fields

        /// <summary>
        /// The index variable count
        /// </summary>
        [NonSerialized] internal int indexVariableCount;
        /// <summary>
        /// The index variables
        /// </summary>
        [NonSerialized] internal int[] indexVariables;
        /// <summary>
        /// The parameter count
        /// </summary>
        [NonSerialized] internal int paramCount;
        /// <summary>
        /// The parameter tables
        /// </summary>
        [NonSerialized] internal IFigures[] paramTables = new IFigures[10];
        /// <summary>
        /// The pass
        /// </summary>
        [NonSerialized] internal int pass = 0;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerContext" /> class.
        /// </summary>
        public CompilerContext()
        {
            indexVariableCount = 0;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return paramCount; }
        }




        /// <summary>
        /// Gets the parameter cards.
        /// </summary>
        /// <value>The parameter cards.</value>
        public IFigures[] ParamCards
        {
            get { return paramTables; }
        }

        #endregion

        #region Methods






        /// <summary>
        /// Gens the local load.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="a">a.</param>
        public static void GenLocalLoad(ILGenerator g, int a)
        {
            switch (a)
            {
                case 0: g.Emit(OpCodes.Ldloc_0); break;
                case 1: g.Emit(OpCodes.Ldloc_1); break;
                case 2: g.Emit(OpCodes.Ldloc_2); break;
                case 3: g.Emit(OpCodes.Ldloc_3); break;
                default:
                    g.Emit(OpCodes.Ldloc, a);
                    break;
            }
        }






        /// <summary>
        /// Gens the local store.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="a">a.</param>
        public static void GenLocalStore(ILGenerator g, int a)
        {
            switch (a)
            {
                case 0: g.Emit(OpCodes.Stloc_0); break;
                case 1: g.Emit(OpCodes.Stloc_1); break;
                case 2: g.Emit(OpCodes.Stloc_2); break;
                case 3: g.Emit(OpCodes.Stloc_3); break;
                default:
                    g.Emit(OpCodes.Stloc, a);
                    break;
            }
        }






        /// <summary>
        /// Adds the specified v.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>System.Int32.</returns>
        public int Add(IFigures v)
        {
            int index = GetIndexOf(v);
            if (index < 0)
            {
                paramTables[paramCount] = v;
                return indexVariableCount + paramCount++;
            }
            return index;
        }





        /// <summary>
        /// Allocs the index variable.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int AllocIndexVariable()
        {
            return indexVariableCount++;
        }





        /// <summary>
        /// Generates the local initialize.
        /// </summary>
        /// <param name="g">The g.</param>
        public void GenerateLocalInit(ILGenerator g)
        {
            
            for (int i = 0; i < indexVariableCount; i++)
                g.DeclareLocal(typeof(int));

            
            string paramFieldName = "DataParameters";

            for (int i = 0; i < paramCount; i++)
                g.DeclareLocal(typeof(IFigures));

            for (int i = 0; i < paramCount; i++)
                g.DeclareLocal(typeof(IFigure));

            g.DeclareLocal(typeof(double));

            
            for (int i = 0; i < paramCount; i++)
            {
                
                g.Emit(OpCodes.Ldarg_0); 
                g.Emit(OpCodes.Ldfld, typeof(CombinedMathset).GetField(paramFieldName));
                g.Emit(OpCodes.Ldc_I4, i);
                g.Emit(OpCodes.Ldelem_Ref);
                g.Emit(OpCodes.Stloc, indexVariableCount + i);
            }
        }






        /// <summary>
        /// Gets the buffor index of.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>System.Int32.</returns>
        public int GetBufforIndexOf(IFigures v)
        {
            for (int i = 0; i < paramCount; i++)
                if (paramTables[i] == v) return indexVariableCount + i + paramCount + 1;
            return -1;
        }






        /// <summary>
        /// Gets the index of.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>System.Int32.</returns>
        public int GetIndexOf(IFigures v)
        {
            for (int i = 0; i < paramCount; i++)
                if (paramTables[i] == v) return indexVariableCount + i;
            return -1;
        }







        /// <summary>
        /// Gets the index variable.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>System.Int32.</returns>
        public int GetIndexVariable(int number)
        {
            return indexVariables[number];
        }






        /// <summary>
        /// Gets the sub index of.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>System.Int32.</returns>
        public int GetSubIndexOf(IFigures v)
        {
            for (int i = 0; i < paramCount; i++)
                if (paramTables[i] == v) return indexVariableCount + i + paramCount;
            return -1;
        }





        /// <summary>
        /// Determines whether [is first pass].
        /// </summary>
        /// <returns><c>true</c> if [is first pass]; otherwise, <c>false</c>.</returns>
        public bool IsFirstPass()
        {
            return pass == 0;
        }




        /// <summary>
        /// Nexts the pass.
        /// </summary>
        public void NextPass()
        {
            pass++;
            
            indexVariables = new int[indexVariableCount];
            for (int i = 0; i < indexVariableCount; i++)
                indexVariables[i] = i;
        }






        /// <summary>
        /// Sets the index variable.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="value">The value.</param>
        public void SetIndexVariable(int number, int value)
        {
            indexVariables[number] = value;
        }

        #endregion
    }
}
