
// <copyright file="LaborCase.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Collections.Generic;
    using System.Series;


    /// <summary>
    /// Class LaborCase.
    /// Implements the <see cref="System.Series.Catalog{System.Laboring.Aspect}" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{System.Laboring.Aspect}" />
    public class LaborCase : Catalog<Aspect>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LaborCase" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="notes">The notes.</param>
        public LaborCase(string name = null, LaborNotes notes = null)
        {
            Name = (name != null) ? name : "LaborCase";
            Notes = (Notes != null) ? notes : new LaborNotes();
            Methods = new LaborMethods();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the methods.
        /// </summary>
        /// <value>The methods.</value>
        public LaborMethods Methods { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public LaborNotes Notes { get; set; }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Aspect.</returns>
        public Aspect Get(string key)
        {
            Aspect result = null;
            TryGet(key, out result);
            return result;
        }
        /// <summary>
        /// Adds the specified aspect.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        public override void Add(Aspect aspect)
        {
            aspect.Case = this;
            aspect.Laborator = new Laborator(aspect);
            Put(aspect.Name, aspect);
        }
        /// <summary>
        /// Adds the specified aspects.
        /// </summary>
        /// <param name="aspects">The aspects.</param>
        public override void Add(IEnumerable<Aspect> aspects)
        {
            foreach (var aspect in aspects)
            {
                aspect.Case = this;
                aspect.Laborator = new Laborator(aspect);
                Put(aspect.Name, aspect);
            }          
        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Add(object key, Aspect value)
        {
            value.Case = this;
            value.Laborator = new Laborator(value);
            Put(key, value);
            return true;
        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(object key, IEnumerable<Labor> value)
        {
            Aspect msn = new Aspect(key.ToString(), value);
            msn.Case = this;
            msn.Laborator = new Laborator(msn);
            Put(key, msn);
        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Aspect.</returns>
        public Aspect Add(object key, IEnumerable<IDeputy> value)
        {
            Aspect msn = new Aspect(key.ToString(), value);
            msn.Case = this;
            msn.Laborator = new Laborator(msn);
            Put(key, msn);
            return msn;
        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(object key, IDeputy value)
        {
            List<IDeputy> cml = new List<IDeputy>() { value };
            Aspect msn = new Aspect(key.ToString(), cml);
            msn.Case = this;
            msn.Laborator = new Laborator(msn);
            Put(key, msn);
        }

        /// <summary>
        /// Gets or sets the <see cref="Aspect" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Aspect.</returns>
        public override Aspect this[object key]
        {
            get
            {                
                TryGet(key, out Aspect result);
                return result;
            }
            set
            {                
                value.Case = this;
                value.Laborator = new Laborator(value);
                Put(key, value);
            }
        }
    }
}
