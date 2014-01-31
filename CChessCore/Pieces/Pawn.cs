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
using System.Linq;

namespace CChessCore
{
    public class Pawn : IPiece
    {
        private readonly Square[] _captureWhiteMoves = new Square[] { new Square(1, 1), new Square(-1, 1) };
        private readonly Square[] _captureBlackMoves = new Square[] { new Square(-1, -1), new Square(1, -1) };

        public PieceColor Color { get; private set; }
        public PieceType Type { get { return PieceType.Pawn; } }

        /// <summary>
        /// Abbreviation used in FEN notation.
        /// </summary>
        public string FenAbbreviation
        {
            get { return Color == PieceColor.White ? "P" : "p"; }
        }

        public Pawn(PieceColor color)
        {
            Color = color;
        }

        public bool CanMove(IGame game, IGameBoard board, Move move)
        {
            if (move.To.Equals(move.From))
                return false;

            var simepleMoveVector = Color == PieceColor.White ? new Square(0, 1) : new Square(0, -1);
            var doubleMoveVector = Color == PieceColor.White ? new Square(0, 2) : new Square(0, -2);

            if (move.To.Equals(move.From + simepleMoveVector) &&
               board[move.To] == null)
            {
                var promotionField = Color == PieceColor.White ? 7 : 0;
                if (move.To.Y == promotionField)
                {
                    move.Type |= MoveType.Promotion;
                }
                return true;
            }

            int moveYFrom = Color == PieceColor.White ? 1 : 6;
            if (move.From.Y == moveYFrom &&
                move.To.Equals(move.From + doubleMoveVector) &&
                board[move.To] == null && board[move.From + simepleMoveVector] == null)
            {
                move.Type |= MoveType.DoublePawn;
                return true;
            }

            var captureMoves = Color == PieceColor.White ? _captureWhiteMoves : _captureBlackMoves;
            foreach (var captureMove in captureMoves)
            {
                if (move.To.Equals(move.From + captureMove))
                {
                    var captured = board[move.To];
                    if (captured != null && captured.Color != Color)
                    {
                        var promotionField = Color == PieceColor.White ? 7 : 0;
                        if (move.To.Y == promotionField)
                        {
                            move.Type |= MoveType.Promotion;
                        }
                        return true;
                    }
                    else if(game.Status.EnPassentPosition != null &&
                            game.Status.EnPassentPosition.Equals(move.To))
                    {
                        move.Type = MoveType.EnPassent;
                        return true;
                    }
                }
            }

            return false;
        }

        public IEnumerable<Move> GetValidMoves(IGame game, IGameBoard board, Square square)
        {
            Square[] _moves = Color == PieceColor.White
                                  ? new [] {new Square(1, 1), new Square(-1, 1), new Square(0, 1), new Square(0, 2)}
                                  : new [] {new Square(-1, -1), new Square(1, -1), new Square(0, -1), new Square(0, -2)};

            var result = new List<Move>();

            if (board[square].Type != PieceType.Pawn)
            {
                return result;
            }

            foreach (var direction in _moves)
            {
                Square moveTo = square + direction;

                Move move = new Move(square, moveTo);
                if (CanMove(game, board, move))
                {
                    result.Add(move);
                }
            }

            return result;
        }
    }
}