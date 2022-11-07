
// <copyright file="TetraValue.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Spectrum namespace.
/// </summary>
namespace System.Series.Spectrum
{
    /// <summary>
    /// Class TetraValue.
    /// Implements the <see cref="System.Series.SpectrumBase" />
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.Series.SpectrumBase" />
    /// <seealso cref="System.IDisposable" />
    public class TetraValue : SpectrumBase, IDisposable
    {
        #region Fields

        /// <summary>
        /// The x value
        /// </summary>
        public int[] xValue;
        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TetraValue" /> class.
        /// </summary>
        /// <param name="null_key">The null key.</param>
        public TetraValue(int null_key)
        {
            xValue = new int[] { null_key, null_key, null_key, null_key };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the index maximum.
        /// </summary>
        /// <value>The index maximum.</value>
        public override int IndexMax
        {
            get { return xValue[3]; }
        }

        /// <summary>
        /// Gets the index minimum.
        /// </summary>
        /// <value>The index minimum.</value>
        public override int IndexMin
        {
            get { return xValue[0]; }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override int Size
        {
            get { return 4; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified base offset.
        /// </summary>
        /// <param name="offsetBase">The offset base.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        public override void Add(int offsetBase, int offsetFactor, int indexOffset, int x)
        {
            if(xValue[x] == x)
            {
                return;
            }

            xValue[x] = x;
            if(x > xValue[3])
            {
                xValue[3] = x;
                return;

            }
            if(x < xValue[0])
            {
                xValue[0] = x;
                return;
            }
        }

        /// <summary>
        /// Adds the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        public override void Add(int x)
        {
            if(xValue[x] == x)
            {
                return;
            }

            xValue[x] = x;
            if(x > xValue[3])
            {
                xValue[3] = x;
                return;
            }

            if(x < xValue[0])
            {
                xValue[0] = x;
                return;
            }
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="offsetBase">The offset base.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if [contains] [the specified base offset]; otherwise, <c>false</c>.</returns>
        public override bool Contains(int offsetBase, int offsetFactor, int indexOffset, int x)
        {
            if(xValue[x] == x)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if [contains] [the specified x]; otherwise, <c>false</c>.</returns>
        public override bool Contains(int x)
        {
            if(xValue[x] == x)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Firsts the add.
        /// </summary>
        /// <param name="offsetBase">The offset base.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        public override void FirstAdd(int offsetBase, int offsetFactor, int indexOffset, int x)
        {
            xValue[0] = x;
            xValue[3] = x;
            xValue[x] = x;
        }

        /// <summary>
        /// Firsts the add.
        /// </summary>
        /// <param name="x">The x.</param>
        public override void FirstAdd(int x)
        {
            xValue[0] = x;
            xValue[3] = x;
            xValue[x] = x;
        }

        /// <summary>
        /// Nexts the specified base offset.
        /// </summary>
        /// <param name="offsetBase">The offset base.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        public override int Next(int offsetBase, int offsetFactor, int indexOffset, int x)
        {
            if(x >= xValue[3]) return -1; 
            if(xValue[x + 1] != -1) return xValue[x + 1];    
            if(xValue[x + 2] != -1) return xValue[x + 2];    
            return xValue[3]; 
        }

        /// <summary>
        /// Nexts the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        public override int Next(int x)
        {
            if(x >= xValue[3]) return -1; 
            if(xValue[x + 1] != -1) return xValue[x + 1];    
            if(xValue[x + 2] != -1) return xValue[x + 2];    
            return xValue[3]; 
        }

        /// <summary>
        /// Previouses the specified base offset.
        /// </summary>
        /// <param name="offsetBase">The offset base.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        public override int Previous(int offsetBase, int offsetFactor, int indexOffset, int x)
        {
            if(x <= xValue[0]) return -1; 
            if(xValue[x - 1] != -1) return xValue[x - 1];    
            if(xValue[x - 2] != -1) return xValue[x - 2];    
            return xValue[0];   
        }

        /// <summary>
        /// Previouses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        public override int Previous(int x)
        {
            if(x <= xValue[0]) return -1; 
            if(xValue[x - 1] != -1) return xValue[x - 1];
            if(xValue[x - 2] != -1) return xValue[x - 2];
            return xValue[0];
        }

        /// <summary>
        /// Removes the specified base offset.
        /// </summary>
        /// <param name="offsetBase">The offset base.</param>
        /// <param name="offsetFactor">The offset factor.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Remove(int offsetBase, int offsetFactor, int indexOffset, int x)
        {
            if(xValue[x] != x) return false; 

            
            if(xValue[0] == x)
            {
                xValue[x] = -1;
                if(xValue[1] != -1) { xValue[0] = xValue[1]; return true; }
                if(xValue[2] != -1) { xValue[0] = xValue[2]; return true; }
                if(xValue[3] != -1 && xValue[3] != x) { xValue[0] = xValue[3]; return true; } 
                xValue[0] = -1;
                xValue[3] = -1;
                return true;
            }
            
            if(xValue[3] == x)
            {
                xValue[x] = -1;
                if(xValue[2] != -1) { xValue[3] = xValue[2]; return true; }
                if(xValue[1] != -1) { xValue[3] = xValue[1]; return true; }
                
                xValue[3] = xValue[0];
                return true;
            }
            xValue[x] = -1;
            return true;
        }

        /// <summary>
        /// Removes the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool Remove(int x)
        {
            if(xValue[x] != x) return false; 

            
            

            
            if(xValue[0] == x)
            {
                xValue[x] = -1;
                if(xValue[1] != -1) { xValue[0] = xValue[1]; return true; }
                if(xValue[2] != -1) { xValue[0] = xValue[2]; return true; }
                if(xValue[3] != -1 && xValue[3] != x) { xValue[0] = xValue[3]; return true; } 
                xValue[0] = -1;
                xValue[3] = -1;
                return true;
            }
            
            if(xValue[3] == x)
            {
                xValue[x] = -1;
                if(xValue[2] != -1) { xValue[3] = xValue[2]; return true; }
                if(xValue[1] != -1) { xValue[3] = xValue[1]; return true; }
                
                xValue[3] = xValue[0];
                return true;
            }
            xValue[x] = -1;
            return true;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    xValue = null;
                }

                disposedValue = true;
            }
        }

        #endregion
    }
}
