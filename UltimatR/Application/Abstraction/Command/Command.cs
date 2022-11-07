
// <copyright file="Command.cs" company="UltimatR.Framework">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

using FluentValidation.Results;
using System;
using System.Text.Json.Serialization;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class Command.
    /// Implements the <see cref="UltimatR.ICommand" />
    /// </summary>
    /// <seealso cref="UltimatR.ICommand" />
    public abstract class Command : ICommand
    {
        /// <summary>
        /// The entity
        /// </summary>
        private Entity entity;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public virtual long Id { get; set; }

        /// <summary>
        /// Gets or sets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public object[] Keys { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>The entity.</value>
        [JsonIgnore] public virtual Entity Entity 
        { 
            get => entity; 
            set 
            {
                entity = value;
                if (Id == 0 && entity.Id != 0)
                    Id = entity.Id;
            }  
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [JsonIgnore] public virtual object Data { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [JsonIgnore] public ValidationResult Result { get; set; }

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>The error messages.</value>
        public string ErrorMessages => Result.ToString();

        /// <summary>
        /// Gets or sets the command mode.
        /// </summary>
        /// <value>The command mode.</value>
        public CommandMode CommandMode { get; set; }

        /// <summary>
        /// Gets or sets the publish mode.
        /// </summary>
        /// <value>The publish mode.</value>
        public PublishMode PublishMode { get; set; }

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <value>The input.</value>
        public virtual object Input => Data;

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <value>The output.</value>
        public virtual object Output => (IsValid) ? Id : ErrorMessages;

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid => Result.IsValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        protected Command()
        {
            Result = new ValidationResult();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="commandMode">The command mode.</param>
        /// <param name="publishMode">The publish mode.</param>
        protected Command(CommandMode commandMode, PublishMode publishMode) : this()
        {
            CommandMode = commandMode;
            PublishMode = publishMode;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="entryData">The entry data.</param>
        /// <param name="commandMode">The command mode.</param>
        /// <param name="publishMode">The publish mode.</param>
        protected Command(object entryData, CommandMode commandMode, PublishMode publishMode) : this(commandMode, publishMode)
        {
            Data = entryData;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="entryData">The entry data.</param>
        /// <param name="commandMode">The command mode.</param>
        /// <param name="publishMode">The publish mode.</param>
        /// <param name="keys">The keys.</param>
        protected Command(object entryData, CommandMode commandMode, PublishMode publishMode, params object[] keys) : this(commandMode, publishMode, keys)
        {
            Data = entryData;            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="commandMode">The command mode.</param>
        /// <param name="publishMode">The publish mode.</param>
        /// <param name="keys">The keys.</param>
        protected Command(CommandMode commandMode, PublishMode publishMode, params object[] keys) : this(commandMode, publishMode)
        {        
            Keys = keys;
        }
    }
}