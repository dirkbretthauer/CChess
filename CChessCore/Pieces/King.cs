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

namespace CChessCore.Pieces
{
    public class King : IPiece
    {
        private readonly Square[] _validMoves = new Square[] { new Square(0, 1), new Square(1, 1), new Square(1, 0), new Square(1, -1),
                                                                     new Square(0, -1), new Square(-1, -1), new Square(-1, 0), new Square(-1, 1)};

        private readonly Move _whiteCastlingShort = new Move("e1", "g1");
        private readonly Move _whiteCastlelingLong = new Move("e1", "c1");
        private readonly Move _blackCastlelingShort = new Move("e8", "g8");
        private readonly Move _blackCastlelingLong = new Move("e8", "c8");

        public PieceColor Color { get; private set; }
        public PieceType Type { get { return PieceType.King; } }

        /// <summary>
        /// Abbreviation used in FEN notation.
        /// </summary>
        public string FenAbbreviation
        {
            get { return Color == PieceColor.White ? Type.PgnCode : Type.PgnCode.ToLower(); }
        }

        public King(PieceColor color)
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


            if (Color == PieceColor.White && move.From.Equals(_whiteCastlingShort.From))
            {
                if (move.To.Equals(_whiteCastlingShort.To))
                {
                    if (!game.IsSquareUnderCheck("f1", board) &&
                        !game.IsSquareUnderCheck("g1", board))
                    {
                        move.Type = MoveType.ShortCastle;
                        return true;
                    }
                }
                else if (move.To.Equals(_whiteCastlelingLong.To))
                {
                    if (!game.IsSquareUnderCheck("d1", board) &&
                        !game.IsSquareUnderCheck("c1", board))
                    {
                        move.Type = MoveType.LongCastle;
                        return true;
                    }
                }
            }
            else if (Color == PieceColor.Black && move.From.Equals(_blackCastlelingShort.From))
            {
                if (move.To.Equals(_blackCastlelingShort.To))
                {
                    if (!game.IsSquareUnderCheck("f8", board) &&
                        !game.IsSquareUnderCheck("g8", board))
                    {
                        move.Type = MoveType.ShortCastle;
                        return true;
                    }
                }
                else if (move.To.Equals(_blackCastlelingLong.To))
                {
                    if (!game.IsSquareUnderCheck("d8", board) &&
                        !game.IsSquareUnderCheck("c8", board))
                    {
                        move.Type = MoveType.LongCastle;
                        return true;
                    }
                }
            }

            return false;
        }

        public IEnumerable<Move> GetValidMoves(IGame game, IGameBoard board, Square square)
        {
            var result = new List<Move>();

            if (board[square].Type != PieceType.King)
            {
                return result;
            }

            foreach (var validMove in _validMoves)
            {
                var moveTo = square + validMove;
                if(moveTo == null)
                {
                    continue;
                }

                var move = new Move(square, moveTo);
                if(CanMove(game, board, move))
                {
                    result.Add(move);
                }
            }

            return result;
        }
    }
}