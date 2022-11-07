
// <copyright file="JsonAttributes.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Json namespace.
/// </summary>
namespace System.Instant.Json
{
    #region Enums

    /// <summary>
    /// Enum JsonModes
    /// </summary>
    public enum JsonModes
    {
        /// <summary>
        /// All
        /// </summary>
        All,
        /// <summary>
        /// The key value
        /// </summary>
        KeyValue,
        /// <summary>
        /// The array
        /// </summary>
        Array
    }

    #endregion




    /// <summary>
    /// Class JsonArrayAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.Json.JsonAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.Json.JsonAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonArrayAttribute : JsonAttribute
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArrayAttribute" /> class.
        /// </summary>
        public JsonArrayAttribute()
        {
        }

        #endregion
    }




    /// <summary>
    /// Class JsonAttribute.
    /// Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public class JsonAttribute : Attribute
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="JsonAttribute" /> class.
        /// </summary>
        public JsonAttribute()
        {
        }

        #endregion
    }




    /// <summary>
    /// Class JsonIgnoreAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.Json.JsonAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.Json.JsonAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonIgnoreAttribute : JsonAttribute
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="JsonIgnoreAttribute" /> class.
        /// </summary>
        public JsonIgnoreAttribute()
        {
        }

        #endregion
    }




    /// <summary>
    /// Class JsonMemberAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.Json.JsonAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.Json.JsonAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonMemberAttribute : JsonAttribute
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMemberAttribute" /> class.
        /// </summary>
        public JsonMemberAttribute()
        {
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the serial mode.
        /// </summary>
        /// <value>The serial mode.</value>
        public JsonModes SerialMode { get; set; } = JsonModes.All;

        #endregion
    }




    /// <summary>
    /// Class JsonObjectAttribute. This class cannot be inherited.
    /// Implements the <see cref="System.Instant.Json.JsonAttribute" />
    /// </summary>
    /// <seealso cref="System.Instant.Json.JsonAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class JsonObjectAttribute : JsonAttribute
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="JsonObjectAttribute" /> class.
        /// </summary>
        public JsonObjectAttribute()
        {
        }

        #endregion
    }
}
