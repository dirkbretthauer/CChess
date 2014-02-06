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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CChessCore.Pieces
{
    public class Queen : IPiece
    {
        private readonly Square[] _validDirections = new Square[] { new Square(1, 1), new Square(1, -1), new Square(-1, -1), new Square(-1, 1),
                                                                    new Square(1, 0), new Square(-1, 0), new Square(0, 1), new Square(0, -1)};

        public PieceColor Color { get; private set; }
        public PieceType Type { get { return PieceType.Queen; } }

        /// <summary>
        /// Abbreviation used in FEN notation.
        /// </summary>
        public string FenAbbreviation
        {
            get { return Color == PieceColor.White ? Type.PgnCode : Type.PgnCode.ToLower(); }
        }

        public Queen(PieceColor color)
        {
            Color = color;
        }

        public bool CanMove(IGame game, IGameBoard board, Move move)
        {
            Square direction;

            if (move.To.Equals(move.From))
                return false;

            int lengthX = move.To.X - move.From.X;
            int lengthY = move.To.Y - move.From.Y;
            if (Math.Abs(lengthX) != Math.Abs(lengthY))
            {
                if (lengthX == 0)
                {
                    direction = new Square(0, lengthY > 0 ? 1 : -1);
                }
                else if (lengthY == 0)
                {
                    direction = new Square(lengthX > 0 ? 1 : -1, 0);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                direction = new Square(lengthX > 0 ? 1 : -1, lengthY > 0 ? 1 : -1);
            }

            Square temp = move.From + direction;
            while (!temp.Equals(move.To))
            {
                if (board[temp] != null)
                {
                    return false;
                }

                temp = temp + direction;
            }

            var captured = board[move.To];
            if (captured != null)
            {
                if (captured.Color == Color)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<Move> GetValidMoves(IGame game, IGameBoard board, Square square)
        {
            var result = new List<Move>();
            if (board[square].Type != PieceType.Queen)
            {
                return result;
            }

            foreach (var direction in _validDirections)
            {
                Square moveTo = square + direction;
                
                while (moveTo != null)
                {
                    Move move = new Move(square, moveTo);
                    if (CanMove(game, board, move))
                    {
                        result.Add(move);
                    }

                    if (board[moveTo] != null)
                    {
                        break;
                    }

                    moveTo = moveTo + direction;
                }
            }

            return result;
        }
    }
}