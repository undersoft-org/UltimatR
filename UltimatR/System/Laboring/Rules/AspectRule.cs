
// <copyright file="AspectRule.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



using System.Uniques;

/// <summary>
/// The Rules namespace.
/// </summary>
namespace System.Laboring.Rules
{
    /// <summary>
    /// Class Aspect.
    /// Implements the <see cref="System.Laboring.Aspect" />
    /// </summary>
    /// <typeparam name="TAspect">The type of the t aspect.</typeparam>
    /// <seealso cref="System.Laboring.Aspect" />
    public class Aspect<TAspect> : Aspect where TAspect : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Aspect{TAspect}" /> class.
        /// </summary>
        public Aspect() : base(typeof(TAspect).FullName) { }

        /// <summary>
        /// Operations this instance.
        /// </summary>
        /// <typeparam name="TEvent">The type of the t event.</typeparam>
        /// <returns>Labor.</returns>
        public override Labor Operation<TEvent>() where TEvent : class
        {
            return base.Operation<TEvent>();
        }
        /// <summary>
        /// Operations the specified arguments.
        /// </summary>
        /// <typeparam name="TEvent">The type of the t event.</typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns>Labor.</returns>
        public override Labor Operation<TEvent>(Type[] arguments) where TEvent : class
        {
            return base.Operation<TEvent>(arguments);
        }
        /// <summary>
        /// Operations the specified consrtuctor parameters.
        /// </summary>
        /// <typeparam name="TEvent">The type of the t event.</typeparam>
        /// <param name="consrtuctorParams">The consrtuctor parameters.</param>
        /// <returns>Labor.</returns>
        public override Labor Operation<TEvent>(params object[] consrtuctorParams) where TEvent : class
        {
            return base.Operation<TEvent>(consrtuctorParams);
        }
        /// <summary>
        /// Operations the specified arguments.
        /// </summary>
        /// <typeparam name="TEvent">The type of the t event.</typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <param name="consrtuctorParams">The consrtuctor parameters.</param>
        /// <returns>Labor.</returns>
        public override Labor Operation<TEvent>(Type[] arguments, params object[] consrtuctorParams) where TEvent : class
        {
            return base.Operation<TEvent>(arguments, consrtuctorParams);
        }
    }
}
