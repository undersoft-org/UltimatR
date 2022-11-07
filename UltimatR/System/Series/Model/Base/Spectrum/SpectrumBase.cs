
// <copyright file="SpectrumBase.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Series namespace.
/// </summary>
namespace System.Series
{
    /// <summary>
    /// Class SpectrumBase.
    /// </summary>
    public abstract class SpectrumBase
    {
        #region Properties

        /// <summary>
        /// Gets the index maximum.
        /// </summary>
        /// <value>The index maximum.</value>
        public abstract int IndexMax { get; }

        /// <summary>
        /// Gets the index minimum.
        /// </summary>
        /// <value>The index minimum.</value>
        public abstract int IndexMin { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public abstract int Size { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified base offset.
        /// </summary>
        /// <param name="baseOffset">The base offset.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        public abstract void Add(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// Adds the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        public abstract void Add(int x);

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="baseOffset">The base offset.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if [contains] [the specified base offset]; otherwise, <c>false</c>.</returns>
        public abstract bool Contains(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if [contains] [the specified x]; otherwise, <c>false</c>.</returns>
        public abstract bool Contains(int x);

        /// <summary>
        /// Firsts the add.
        /// </summary>
        /// <param name="baseOffset">The base offset.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        public abstract void FirstAdd(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// Firsts the add.
        /// </summary>
        /// <param name="x">The x.</param>
        public abstract void FirstAdd(int x);

        /// <summary>
        /// Nexts the specified base offset.
        /// </summary>
        /// <param name="baseOffset">The base offset.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        public abstract int Next(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// Nexts the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        public abstract int Next(int x);

        /// <summary>
        /// Previouses the specified base offset.
        /// </summary>
        /// <param name="baseOffset">The base offset.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        public abstract int Previous(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// Previouses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        public abstract int Previous(int x);

        /// <summary>
        /// Removes the specified base offset.
        /// </summary>
        /// <param name="baseOffset">The base offset.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool Remove(int baseOffset, int offsetFactor, int indexOffset, int x);

        /// <summary>
        /// Removes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool Remove(int x);

        #endregion
    }
}
