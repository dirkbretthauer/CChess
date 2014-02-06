#region Header
////////////////////////////////////////////////////////////////////////////// 
//The MIT License (MIT)

//Copyright (c) 2013 Dirk Bretthauer

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;

namespace CChessCore
{
    public class Square : IEquatable<Square>
    {
        public int Y { get; private set; }

        public int X { get; private set; }

        /// <summary>
        /// Instantiate a Square.
        /// </summary>
        /// <param name="x">zero based index of the x coordinate.</param>
        /// <param name="y">zero based index of the y coordinate.</param>
        public Square(int x, int y)
        {
            Y = y;
            X = x;
        }

        public static Square operator +(Square first, Square second)
        {
            Square result = null;

            if (IsValidSquare(first.X + second.X, first.Y + second.Y))
            {
                result = new Square(first.X + second.X, first.Y + second.Y);
            }

            return result;
        }

        public static Square operator -(Square first, Square second)
        {
            Square result = null;

            if (IsValidSquare(first.X - second.X, first.Y - second.Y))
            {
                result = new Square(first.X - second.X, first.Y - second.Y);
            }

            return result;
        }

        private static bool IsValidSquare(int x, int y)
        {
            if(x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                return true;
            }

            return false;
        }

        public static implicit operator Square(string str)
        {
            if(str.Length != 2)
            {
                throw new ArgumentException("Invalid Square");
            }
            var x = char.ToLower(str[0]);
            var y = int.Parse(str.Substring(1));
            
            if (y < 1 || y > 8 || x < 'a' || x > 'h')
            {
                throw new ArgumentException("Invalid Square");
            }

            return new Square(x - 'a', y - 1);
        }

        public static char XtoCoordinate(int x)
        {
            return (char)('a' + x);
        }

        public static char YtoCoordinate(int y)
        {
            return Char.Parse("" + ++y);
        }
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Concat((char)('a' + X), Y + 1);
        }

        #region Implements IEquatable<Square>
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Square other)
        {
            bool result = false;

            if(other != null &&
                X == other.X &&
                Y == other.Y)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            return Equals(obj as Square);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash*37 + X;
            hash = hash*37 + Y;
            return hash;
        }
        #endregion
    }
}