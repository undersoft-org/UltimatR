
// <copyright file="PropertyExtractExtensions - Copy.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



using System.Reflection;

/// <summary>
/// The Extract namespace.
/// </summary>
namespace System.Extract
{



    /// <summary>
    /// Class PropertyInfoExtractExtenstion.
    /// </summary>
    public static class PropertyInfoExtractExtenstion
    {
        #region Methods

        /// <summary>
        /// Gets the name of the backing field.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.String.</returns>
        public static string GetBackingFieldName(string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }

        /// <summary>
        /// Haves the backing field.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool HaveBackingField(string propertyName)
        {
            return (GetBackingFieldName(propertyName) != null);
        }

        /// <summary>
        /// Haves the backing field.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool HaveBackingField(this PropertyInfo property)
        {
            return (GetBackingFieldName(property.Name) != null);
        }

        /// <summary>
        /// Gets the backing field.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>FieldInfo.</returns>
        private static FieldInfo GetBackingField(Type type, string propertyName)
        {
            return type.GetField(GetBackingFieldName(propertyName), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Gets the backing field.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>FieldInfo.</returns>
        private static FieldInfo GetBackingField(object obj, string propertyName)
        {
            return obj.GetType().GetField(GetBackingFieldName(propertyName), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        #endregion
    }
}
