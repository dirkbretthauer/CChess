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
using System.Collections.Generic;

namespace CChessCore.Pieces
{
    public class Knight : IPiece
    {
        private readonly Square[] _validMoves = new Square[] { new Square(-2, 1), new Square(-1, 2), new Square(1, 2), new Square(2, 1),
                                                      new Square(2, -1), new Square(1, -2), new Square(-1, -2), new Square(-2, -1)};

        public PieceColor Color { get; private set; }
        public PieceType Type { get { return PieceType.Knight; } }

        /// <summary>
        /// Abbreviation used in FEN notation.
        /// </summary>
        public string FenAbbreviation
        {
            get { return Color == PieceColor.White ? Type.PgnCode : Type.PgnCode.ToLower(); }
        }

        public Knight(PieceColor color)
        {
            Color = color;
        }

        public bool CanMove(IGame game, IGameBoard board, Move move)
        {
            if (move.To.Equals(move.From))
                return false;

            foreach (var validMove in _validMoves)
            {
                if (move.To.Equals(move.From + validMove))
                {
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
            }

            return false;
        }

        public IEnumerable<Move> GetValidMoves(IGame game, IGameBoard board, Square square)
        {
            var result = new List<Move>();

            if (board[square].Type != PieceType.Knight)
            {
                return result;
            }

            foreach (var validMove in _validMoves)
            {
                var move = new Move(square, square + validMove);
                if (CanMove(game, board, move))
                {
                    result.Add(move);
                }
            }

            return result;
        }
    }
}