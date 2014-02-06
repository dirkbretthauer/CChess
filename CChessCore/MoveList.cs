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
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace CChessCore
{
    public class MoveList : IMoveList
    {
        private readonly List<Move> _moveList;
        private int _currentHalfMoveNumber;

        public Move LastMove
        {
            get
            {
                if(_currentHalfMoveNumber > 1)
                {
                    return _moveList[_currentHalfMoveNumber - 2];
                }

                return null;
            }
        }

        public Move CurrentMove
        {
            get
            {
                if (_currentHalfMoveNumber > 0)
                {
                    return _moveList[_currentHalfMoveNumber - 1];
                }

                return null;
            }
        }

        public int CurrentHalfMoveNumber { get { return _currentHalfMoveNumber; } }
        


        public MoveList()
        {
            _moveList = new List<Move>();
        }

        public bool TryGoBackward(out Move move)
        {
            if (_currentHalfMoveNumber > 1)
            {
                move = CurrentMove;
                _currentHalfMoveNumber--;
                return true;
            }

            move = null;
            return false;
        }

        public bool TryGoForward(out Move move)
        {
            if (_currentHalfMoveNumber < _moveList.Count)
            {
                _currentHalfMoveNumber++;
                move = CurrentMove;
                return true;
            }

            move = null;
            return false;
        }

        public bool GoTo(int halfMoveNumber)
        {
            if(halfMoveNumber <= _moveList.Count &&
                halfMoveNumber > 0)
            {
                _currentHalfMoveNumber = halfMoveNumber;
                return true;
            }

            return false;
        }

        public void AddComment(int halfMoveNumber, string comment)
        {
            if(halfMoveNumber <= _moveList.Count &&
                halfMoveNumber > 0)
            {
                _moveList[halfMoveNumber - 1].Comment = comment;
            }
        }

        public string ToPgn()
        {
            StringBuilder result = new StringBuilder();
            int moveNumber = 1;
            int halfMoveNumber = 1;

            foreach(var move in _moveList)
            {
                if(halfMoveNumber % 2 == 1)
                {
                    result.Append(moveNumber).Append(". ").Append(move);
                }
                else
                {
                    result.Append(' ').Append(move);
                    moveNumber++;
                }

                halfMoveNumber++;
            }

            return result.ToString();
        }

        #region Implementation of ICollection<Move>
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(Move item)
        {
            _moveList.Add(item);
            _currentHalfMoveNumber++;

            item.HalfMoveNumber = _currentHalfMoveNumber;
            item.TurnNumber = (int)Math.Ceiling(_currentHalfMoveNumber / 2.0);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            _moveList.Clear();
            _currentHalfMoveNumber = 0;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(Move item)
        {
            return _moveList.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(Move[] array, int arrayIndex)
        {
            _moveList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(Move item)
        {
            _currentHalfMoveNumber--;
            return _moveList.Remove(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _moveList.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Implementation of IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Move> GetEnumerator()
        {
            return _moveList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
