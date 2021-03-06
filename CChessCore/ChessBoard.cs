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
using CChessCore.Pieces;


namespace CChessCore
{
    public class ChessBoard : IGameBoard
    {
        private readonly IPiece[,] _board;

        public IPiece this[Square square]
        {
            get
            {
                return _board[square.X, square.Y];
            }
            set
            {
                IPiece oldPiece = _board[square.X, square.Y];

                if (oldPiece == value)
                {
                    return;
                }

                _board[square.X, square.Y] = value;

                OnPositionChanged(new PositionChangedEventArgs(square, oldPiece, value));
            }
        }

        public bool IsMutable
        {
            get { return true; }
        }

        public IGameBoard Copy()
        {
            IGameBoard board = new ChessBoard();
            board.SetPosition(_board);

            return board;
        }

        public void ClearBoard()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    _board[x, y] = null;
                }
            }
        }

        public event EventHandler<PositionChangedEventArgs> PositionChanged;

        public ChessBoard()
        {
            _board = new IPiece[8, 8];

            SetToInitialPosition();
        }

        public void SetToInitialPosition()
        {
            Array.Clear(_board, 0, _board.Length);

            _board[0, 0] = new Rook(PieceColor.White);
            _board[1, 0] = new Knight(PieceColor.White);
            _board[2, 0] = new Bishop(PieceColor.White);
            _board[3, 0] = new Queen(PieceColor.White);
            _board[4, 0] = new King(PieceColor.White);
            _board[5, 0] = new Bishop(PieceColor.White);
            _board[6, 0] = new Knight(PieceColor.White);
            _board[7, 0] = new Rook(PieceColor.White);

            _board[0, 7] = new Rook(PieceColor.Black);
            _board[1, 7] = new Knight(PieceColor.Black);
            _board[2, 7] = new Bishop(PieceColor.Black);
            _board[3, 7] = new Queen(PieceColor.Black);
            _board[4, 7] = new King(PieceColor.Black);
            _board[5, 7] = new Bishop(PieceColor.Black);
            _board[6, 7] = new Knight(PieceColor.Black);
            _board[7, 7] = new Rook(PieceColor.Black);

            for (int i = 0; i < 8; i++)
            {
                _board[i, 1] = new Pawn(PieceColor.White);
                _board[i, 6] = new Pawn(PieceColor.Black);
            }

            OnPositionChanged(PositionChangedEventArgs.Reset);
        }

        public void SetPosition(IPiece[,] position)
        {
            if (position.GetLength(0) != 8 || position.GetLength(1) != 8)
            {
                throw new ArgumentException("position: Incorrect dimensions.");
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    _board[x, y] = position[x, y];
                }
            }

            OnPositionChanged(PositionChangedEventArgs.Reset);
        }

        public IPiece[,] GetPosition()
        {
            return _board;
        }

        private void OnPositionChanged(PositionChangedEventArgs args)
        {
            EventRaiser.TryRaiseEvent(PositionChanged, this, args);
        }

        #region Implementation of IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Tuple<Square, IPiece>> GetEnumerator()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    IPiece piece = _board[i, k];
                    if (piece != null)
                    {
                        yield return new Tuple<Square, IPiece>(new Square(i, k), piece);
                    }
                }
            }
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