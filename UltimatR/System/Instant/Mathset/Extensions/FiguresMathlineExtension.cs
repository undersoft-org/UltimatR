
// <copyright file="FiguresMathlineExtension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using System.Collections.Generic;
    using System.Instant.Mathset;
    using System.Linq;




    /// <summary>
    /// Class FiguresMathsetExtension.
    /// </summary>
    public static class FiguresMathsetExtension
    {
        #region Methods






        /// <summary>
        /// Computes the specified figures.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <returns>IFigures.</returns>
        public static IFigures Compute(this IFigures figures)
        {
            figures.Computations.Select(c => c.Compute()).ToArray();
            return figures;
        }







        /// <summary>
        /// Computes the specified rubrics.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="rubrics">The rubrics.</param>
        /// <returns>IFigures.</returns>
        public static IFigures Compute(this IFigures figures, IList<MemberRubric> rubrics)
        {
            IComputation[] ic = rubrics.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.Select(c => c.Compute()).ToArray();
            return figures;
        }







        /// <summary>
        /// Computes the specified rubric names.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="rubricNames">The rubric names.</param>
        /// <returns>IFigures.</returns>
        public static IFigures Compute(this IFigures figures, IList<string> rubricNames)
        {
            IComputation[] ic = rubricNames.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.Select(c => c.Compute()).ToArray();
            return figures;
        }







        /// <summary>
        /// Computes the specified rubric.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="rubric">The rubric.</param>
        /// <returns>IFigures.</returns>
        public static IFigures Compute(this IFigures figures, MemberRubric rubric)
        {
            IComputation ic = figures.Computations.Where(c => ((Computation)c).ContainsFirst(rubric)).FirstOrDefault();
            if (ic != null)
                ic.Compute();
            return figures;
        }






        /// <summary>
        /// Computes the parallel.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <returns>IFigures.</returns>
        public static IFigures ComputeParallel(this IFigures figures)
        {
            figures.Computations.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }







        /// <summary>
        /// Computes the parallel.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="rubrics">The rubrics.</param>
        /// <returns>IFigures.</returns>
        public static IFigures ComputeParallel(this IFigures figures, IList<MemberRubric> rubrics)
        {
            IComputation[] ic = rubrics.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }







        /// <summary>
        /// Computes the parallel.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="rubricNames">The rubric names.</param>
        /// <returns>IFigures.</returns>
        public static IFigures ComputeParallel(this IFigures figures, IList<string> rubricNames)
        {
            IComputation[] ic = rubricNames.Select(r => figures.Computations.Where(c => ((Computation)c).ContainsFirst(r)).FirstOrDefault()).Where(c => c != null).ToArray();
            ic.AsParallel().Select(c => c.Compute()).ToArray();
            return figures;
        }

        #endregion
    }
}
