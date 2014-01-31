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
    public class Bishop : IPiece
    {
        private readonly Square[] _validDirections = new Square[] { new Square(1, 1), new Square(1, -1), new Square(-1, -1), new Square(-1, 1)};

        public PieceColor Color { get; private set; }
        public PieceType Type { get { return PieceType.Bishop; } }

        /// <summary>
        /// Abbreviation used in FEN notation.
        /// </summary>
        public string FenAbbreviation
        {
            get { return Color == PieceColor.White ? Type.PgnCode : Type.PgnCode.ToLower(); }
        }


        public Bishop(PieceColor color)
        {
            Color = color;
        }

        public bool CanMove(IGame game, IGameBoard board, Move move)
        {
            if (move.To.Equals(move.From))
                return false;

            int lengthX = move.To.X - move.From.X;
            int lengthY = move.To.Y - move.From.Y;
            if (Math.Abs(lengthX) != Math.Abs(lengthY))
            {
                return false;
            }

            Square direction = new Square(lengthX > 0 ? 1 : -1, lengthY > 0 ? 1 : -1);

            Square temp = move.From + direction;
            while (temp != null && !temp.Equals(move.To))
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
            if(board[square].Type != PieceType.Bishop)
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