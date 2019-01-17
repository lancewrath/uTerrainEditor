// This file is part of libnoise-dotnet.
//
// libnoise-dotnet is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// libnoise-dotnet is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with libnoise-dotnet.  If not, see <http://www.gnu.org/licenses/>.

namespace LibNoise.Demo.Ext.Dotnet
{
    using System;
    using System.Drawing;

    using System.Runtime.InteropServices;
    using UnityEngine;
    using LibNoise.Renderer;

    using Color = LibNoise.Renderer.Color;

    /// <summary>
    /// Implements an image, a 2-dimensional array of color values.
    ///
    /// An image can be used to store a color texture.
    ///
    /// These color values are of type IColor.
    /// 
    /// TODO Implement unimplemented method
    /// TODO Create a dotnet projet for this extension
    /// Utiliser lockbits
    /// http://msdn.microsoft.com/fr-fr/library/5ey6h79d.aspx
    /// </summary>
    public class BitmapAdaptater : IMap2D<IColor>
    {
        #region Fields

        /// <summary>
        /// The bitmap
        /// </summary>
        private Texture2D _adaptatee;

        /// <summary>
        /// The height of the map, internal use
        /// </summary>
        private readonly int _height;

        /// <summary>
        /// The width of the map, internal use
        /// </summary>
        private readonly int _width;

        /// <summary>
        /// Flags that indicates if some bitmap changes need to be applied.
        /// As is an expansive operation, BitmapAdaptater.apply is only called
        /// in the Bitmap accessor if changes have been previously done
        /// </summary>
        private bool _bitsLocked;

        /// <summary>
        /// Bitmap information
        /// </summary>
        private Color[] _bmData;

        /// <summary>
        /// The value used for all positions outside of the map.
        /// </summary>
        private IColor _borderValue;

        /// <summary>
        /// Internal data buffer for performances purpose
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Size in byte of one pixel data
        /// </summary>
        private byte _structSize = 1;

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the adaptated System.Drawing.Bitmap
        /// </summary>
        public Texture2D Bitmap
        {
            get
            {

                _adaptatee.Apply();

                return _adaptatee;
            }
            

        }

        /// <summary>
        /// Gets the width of the map
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Gets the height of the map
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Gets the border value of the map
        /// </summary>
        public IColor BorderValue
        {
            get { return _borderValue; }
            set { _borderValue = value; }
        }

        #endregion

        #region Ctor/Dtor

        /// <summary>
        /// Create a new Bitmap with the given values
        /// </summary>
        /// <param name="bitmap">The bitmap to adapt.</param>
        public BitmapAdaptater(Texture2D bitmap)
        {
            /*
            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Canonical: //RGBA
                case PixelFormat.Format8bppIndexed: //R
                case PixelFormat.Format24bppRgb: // RGB
                case PixelFormat.Format32bppRgb: // RGB_
                case PixelFormat.Format32bppArgb: //RGBA
                    //ok

                    break;
                default:
                    throw new ArgumentException("Unsupported image format : Some shit");
            }
            */
            _adaptatee = bitmap;
            _borderValue = Colors.Black;

            _width = _adaptatee.width;
            _height = _adaptatee.height;

            AllocateBuffer();
        }

        /// <summary>
        /// Create a new Bitmap with the given values
        /// </summary>
        /// <param name="width">The width of the new bitmap.</param>
        /// <param name="height">The height of the new bitmap</param>
        public BitmapAdaptater(int width, int height)
        {
            _adaptatee = new Texture2D(width, height);
            _borderValue = Colors.White;

            _width = _adaptatee.width;
            _height = _adaptatee.height;

            AllocateBuffer();
        }

        #endregion

        #region IMap2D<IColor> Members

        /// <summary>
        /// Gets a value at a specified position in the map.
        ///
        /// This method does nothing if the map object is empty or the
        /// position is outside the bounds of the noise map.
        /// </summary>
        /// <param name="x">The x coordinate of the position</param>
        /// <param name="y">The y coordinate of the position</param>
        public IColor GetValue(int x, int y)
        {
            if (_adaptatee != null
                && (x >= 0 && x < _width)
                && (y >= 0 && y < _height)
                )
            {

                    // Noise.Image start to bottom left
                    // Drawing.Bitmap start to top left
                    UnityEngine.Color32 sysColor = _adaptatee.GetPixel(x, _height - 1 - y);
                    return new Color(sysColor.r, sysColor.g, sysColor.b, sysColor.a);
                
            }

            return _borderValue;
        }

        /// <summary>
        /// Sets the new size for the map.
        /// 
        /// </summary>
        /// <param name="width">width The new width for the bitmap</param>
        /// <param name="height">height The new height for the bitmap</param>
        public void SetSize(int width, int height)
        {
            if (_adaptatee.width != width || _adaptatee.height != height)
                throw new NotImplementedException("System.Drawing.Bitmap does not support resize");
        }

        /// <summary>
        /// Resets the bitmap
        /// </summary>
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears the bitmap to a Color.WHITE value
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets a value at a specified position in the map.
        ///
        /// This method does nothing if the map object is empty or the
        /// position is outside the bounds of the noise map.
        /// </summary>
        /// <param name="x">The x coordinate of the position</param>
        /// <param name="y">The y coordinate of the position</param>
        /// <param name="value">The value to set at the given position</param>
        public void SetValue(int x, int y, IColor value)
        {
            if (_adaptatee != null
                && (x >= 0 && x < _width)
                && (y >= 0 && y < _height)
                )
            {
                
                    // Noise.Image start to bottom left
                    // Drawing.Bitmap start to top left
                    _adaptatee.SetPixel(
                        x,
                        _adaptatee.height - 1 - y,
                        
                        new UnityEngine.Color32(value.Red, value.Green, value.Blue, value.Alpha)
                        );
                
            }
        }

        /// <summary>
        /// Clears the bitmap to a specified value.
        /// This method is a O(n) operation, where n is equal to width * height.
        /// </summary>
        /// <param name="color">The color that all positions within the bitmap are cleared to.</param>
        public void Clear(IColor color)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find the lowest and highest value in the map
        /// </summary>
        /// <param name="min">the lowest value</param>
        /// <param name="max">the highest value</param>
        public void MinMax(out IColor min, out IColor max)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal

        /// <summary>
        /// Allocate a buffer.
        /// </summary>
        protected void AllocateBuffer()
        {


        


                // Lock memory region
                _adaptatee = new Texture2D(_width, _height);
                


        }

        /// <summary>
        /// 
        /// </summary>
        protected void Apply()
        {
            _adaptatee.Apply();
        }

        #endregion
    }
}