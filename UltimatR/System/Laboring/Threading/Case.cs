
// <copyright file="Case.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



using System.Uniques;

/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using System.Collections.Generic;
    using System.Laboring.Rules;
    using System.Linq;

    /// <summary>
    /// Class Case.
    /// Implements the <see cref="System.Laboring.Case" />
    /// </summary>
    /// <typeparam name="TRule">The type of the t rule.</typeparam>
    /// <seealso cref="System.Laboring.Case" />
    public class Case<TRule> : Case where TRule : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Case{TRule}" /> class.
        /// </summary>
        public Case() : base(new LaborCase(typeof(TRule).FullName)) { }

        /// <summary>
        /// Aspects this instance.
        /// </summary>
        /// <typeparam name="TAspect">The type of the t aspect.</typeparam>
        /// <returns>Aspect&lt;TAspect&gt;.</returns>
        public Aspect<TAspect> Aspect<TAspect>() where TAspect : class
        {
            if (!TryGet(typeof(TAspect).FullName, out Aspect aspect))
            {
                aspect = new Aspect<TAspect>();
                Add(aspect);
            }
            return aspect as Aspect<TAspect>;
        }
    }

    /// <summary>
    /// Class Case.
    /// Implements the <see cref="System.Laboring.Case" />
    /// </summary>
    /// <seealso cref="System.Laboring.Case" />
    public class Case : LaborCase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Case" /> class.
        /// </summary>
        /// <param name="methods">The methods.</param>
        /// <param name="case">The case.</param>
        public Case(IEnumerable<IDeputy> methods, LaborCase @case = null) 
            : base((@case == null) ? $"Case_{Unique.New}" : @case.Name, 
                  (@case == null) ? new LaborNotes(): @case.Notes)
        {
            Add($"Aspect_{Unique.New}", methods);            
            Open();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Case" /> class.
        /// </summary>
        /// <param name="case">The case.</param>
        public Case(LaborCase @case) : base(@case.Name, @case.Notes)
        {
            Add(@case.AsValues());
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Case" /> class.
        /// </summary>
        public Case() : base($"Case_{Unique.New}", new LaborNotes())
        {
        }

        /// <summary>
        /// Aspects the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="aspect">The aspect.</param>
        /// <returns>Aspect.</returns>
        public Aspect Aspect(IDeputy method, Aspect aspect)
        {
            if (aspect != null)
            {
                if(!TryGet(aspect.Name, out Aspect _aspect))
                {
                    Add(_aspect);
                    _aspect.Include(method);                   
                }
                return aspect;
            }
            return null;
        }
        /// <summary>
        /// Aspects the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="name">The name.</param>
        /// <returns>Aspect.</returns>
        public Aspect Aspect(IDeputy method, string name)
        {
            if(!TryGet(name, out Aspect aspect))
            {
                aspect = new Aspect(name);
                Add(aspect);
                aspect.Include(method);              
            }
            return aspect;
        }
        /// <summary>
        /// Aspects the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Aspect.</returns>
        public Aspect Aspect(string name)
        {
            if(!TryGet(name, out Aspect aspect))
            {
                aspect = new Aspect(name);
                Add(aspect);
            }
            return aspect;
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        public void Open()
        {
            Setup();
        }
        /// <summary>
        /// Setups this instance.
        /// </summary>
        public void Setup()
        {
            foreach (Aspect aspect in AsValues())
            {
                if (aspect.Laborator == null)
                {
                    aspect.Laborator = new Laborator(aspect);
                }
                if (!aspect.Laborator.Ready)
                {
                    aspect.Allocate();
                }
            }
        }

        /// <summary>
        /// Runs the specified labor name.
        /// </summary>
        /// <param name="laborName">Name of the labor.</param>
        /// <param name="input">The input.</param>
        public void Run(string laborName, params object[] input)
        {
            Labor[] labors = AsValues()
                .Where(m => m.ContainsKey(laborName))
                    .SelectMany(w => w.AsValues()).ToArray();

            foreach (Labor labor in labors)
                labor.Execute(input);
        }
        /// <summary>
        /// Runs the specified labors and inputs.
        /// </summary>
        /// <param name="laborsAndInputs">The labors and inputs.</param>
        public void Run(IDictionary<string, object[]> laborsAndInputs)
        {
            foreach (KeyValuePair<string, object[]> worker in laborsAndInputs)
            {
                object input = worker.Value;
                string workerName = worker.Key;
                Labor[] workerLabors = AsValues()
                    .Where(m => m.ContainsKey(workerName))
                    .SelectMany(w => w.AsValues()).ToArray();

                foreach (Labor objc in workerLabors)
                    objc.Execute(input);
            }
        }
    }




    /// <summary>
    /// Class InvalidLaborException.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidLaborException : Exception
    {
        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLaborException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidLaborException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
