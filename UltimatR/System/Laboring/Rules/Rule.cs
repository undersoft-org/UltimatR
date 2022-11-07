
// <copyright file="Rule.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Rules namespace.
/// </summary>
namespace System.Laboring.Rules
{
    /// <summary>
    /// Class Rule.
    /// Implements the <see cref="System.Laboring.Case" />
    /// </summary>
    /// <typeparam name="TRule">The type of the t rule.</typeparam>
    /// <seealso cref="System.Laboring.Case" />
    public class Rule<TRule> : Case where TRule : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rule{TRule}" /> class.
        /// </summary>
        public Rule() : base(new LaborCase(typeof(TRule).FullName)) { }

        /// <summary>
        /// Aspects this instance.
        /// </summary>
        /// <typeparam name="TAspect">The type of the t aspect.</typeparam>
        /// <returns>Aspect&lt;TAspect&gt;.</returns>
        public Aspect<TAspect> Aspect<TAspect>() where TAspect : class
        {
            if(!TryGet(typeof(TAspect).FullName, out Aspect aspect))
            {
                aspect = new Aspect<TAspect>();
                Add(aspect);
            }
            return aspect as Aspect<TAspect>;
        }
    }
}
