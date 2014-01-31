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
    public class Rook : IPiece
    {
        private readonly Square[] _validDirections = new Square[] { new Square(1, 0), new Square(-1, 0), new Square(0, 1), new Square(0, -1) };

        public PieceColor Color { get; private set; }
        public PieceType Type { get { return PieceType.Rook; } }

        /// <summary>
        /// Abbreviation used in FEN notation.
        /// </summary>
        public string FenAbbreviation
        {
            get { return Color == PieceColor.White ? Type.PgnCode : Type.PgnCode.ToLower(); }
        }

        public Rook(PieceColor color)
        {
            Color = color;
        }

        public bool CanMove(IGame game, IGameBoard board, Move move)
        {
            Square direction;

            if (move.To.Equals(move.From))
                return false;

            if (move.From.X == move.To.X)
            {
                direction = move.From.Y < move.To.Y ? new Square(0, 1) : new Square(0, -1);
            }
            else if (move.From.Y == move.To.Y)
            {
                direction = move.From.X < move.To.X ? new Square(1, 0) : new Square(-1, 0);
            }
            else
            {
                return false;
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
            if (board[square].Type != PieceType.Rook)
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