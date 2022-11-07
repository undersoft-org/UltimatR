
// <copyright file="Summarizer.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Treatments namespace.
/// </summary>
namespace System.Instant.Treatments
{
    using System.Linq;




    /// <summary>
    /// Class Summarizer.
    /// </summary>
    public static class Summarizer
    {
        #region Methods







        /// <summary>
        /// Summarizes the specified only view.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="onlyView">if set to <c>true</c> [only view].</param>
        /// <returns>IFigure.</returns>
        public static IFigure Summarize(this IFigures figures, bool onlyView = false)
        {
            return Result(figures, onlyView);
        }







        /// <summary>
        /// Results the specified figures.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="onlyView">if set to <c>true</c> [only view].</param>
        /// <returns>IFigure.</returns>
        private static IFigure Result(IFigures figures, bool onlyView = true)
        {
            IRubrics summaryRubrics = figures.Treatment.SummaryRubrics;
            if (summaryRubrics.Count > 0)
            {
                object[] result = summaryRubrics.AsValues().AsParallel().SelectMany(s =>
                       new object[]
                       {
                           (!string.IsNullOrEmpty(s.RubricName)) ?
                            (s.SummaryOperand == AggregateOperand.Sum) ?
                                Convert.ChangeType(figures.View

                                .Sum

                                (j => (j[s.SummaryOrdinal] is DateTime) ?
                                ((DateTime)j[s.SummaryOrdinal]).ToOADate() :
                                   Convert.ToDouble(j[s.FieldId])), typeof(object)) :
                                (s.SummaryOperand == AggregateOperand.Min) ?
                                Convert.ChangeType(figures.View

                                .Min

                                (j => (j[s.SummaryOrdinal] is DateTime) ?
                                            ((DateTime)j[s.SummaryOrdinal]).ToOADate() :
                                                Convert.ToDouble(j[s.FieldId])), typeof(object)) :
                                 (s.SummaryOperand == AggregateOperand.Max) ?
                                Convert.ChangeType(figures.View

                                .Max

                                (j => (j[s.SummaryOrdinal] is DateTime) ?
                                            ((DateTime)j[s.SummaryOrdinal]).ToOADate() :
                                                Convert.ToDouble(j[s.FieldId])), typeof(object)) :
                                 (s.SummaryOperand == AggregateOperand.Avg) ?
                               Convert.ChangeType(figures.View

                               .Average

                               (j => (j[s.SummaryOrdinal] is DateTime) ?
                                            ((DateTime)j[s.SummaryOrdinal]).ToOADate() :
                                                Convert.ToDouble(j[s.FieldId])), typeof(object)) :
                                 (s.SummaryOperand == AggregateOperand.Bis) ?
                               Convert.ChangeType(figures.View.Select(j => (j[s.FieldId] != DBNull.Value) ? j[s.FieldId].ToString() : "")

                               .Aggregate((x, y) => x + " " + y), typeof(object)) : null : null
                            }
                 ).ToArray();

                figures.Summary.ValueArray = result;

                return figures.Summary;
            }
            else
                return null;
        }

        #endregion
    }
}
