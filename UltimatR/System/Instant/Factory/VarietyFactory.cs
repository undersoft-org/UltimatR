
// <copyright file="VarietyFactory.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    /// <summary>
    /// Class VarietyFactory.
    /// </summary>
    public static class VarietyFactory
    {
        #region Methods

        /// <summary>
        /// Patches to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="target">The target.</param>
        /// <param name="traceChanges">The trace changes.</param>
        /// <returns>E.</returns>
        public static E PatchTo<T, E>(this T item, E target, IDeputy traceChanges = null) where T : class where E : class
        {
            return new Variety<T>(item).Patch(target, traceChanges);
        }
        /// <summary>
        /// Patches to.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="target">The target.</param>
        /// <param name="traceChanges">The trace changes.</param>
        /// <returns>System.Object.</returns>
        public static object PatchTo(this object item, object target, IDeputy traceChanges = null)
        {
            return new Variety(item).Patch(target, traceChanges);
        }
        /// <summary>
        /// Patches to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="traceChanges">The trace changes.</param>
        /// <returns>E.</returns>
        public static E PatchTo<T, E>(this T item, IDeputy traceChanges = null) where T : class where E : class
        {
            return new Variety<T>(item).Patch<E>(traceChanges);
        }
        /// <summary>
        /// Patches the self.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="traceChanges">The trace changes.</param>
        /// <returns>System.Object.</returns>
        public static object PatchSelf(this object item, IDeputy traceChanges = null)
        {
            return new Variety(item).PatchSelf();
        }

        /// <summary>
        /// Puts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="target">The target.</param>
        /// <param name="traceChanges">The trace changes.</param>
        /// <returns>E.</returns>
        public static E PutTo<T, E>(this T item, E target, IDeputy traceChanges = null) where T : class where E : class
        {
            return new Variety<T>(item).Put(target, traceChanges);
        }
        /// <summary>
        /// Puts to.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="target">The target.</param>
        /// <param name="traceChanges">The trace changes.</param>
        /// <returns>System.Object.</returns>
        public static object PutTo(this object item, object target, IDeputy traceChanges = null)
        {
            return new Variety(item).Put(target, traceChanges);
        }
        /// <summary>
        /// Puts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="traceChanges">The trace changes.</param>
        /// <returns>E.</returns>
        public static E PutTo<T, E>(this T item, IDeputy traceChanges = null) where T : class where E : class
        {
            return new Variety<T>(item).Put<E>(traceChanges);
        }

        #endregion
    }
}
