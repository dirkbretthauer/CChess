#region Header
////////////////////////////////////////////////////////////////////////////// 
//    This file is part of $projectname$.
//
//    $projectname$ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    $projectname$ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with $projectname$.  If not, see <http://www.gnu.org/licenses/>.
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