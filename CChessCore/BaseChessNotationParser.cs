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
using System.IO;
using System.Linq;
using System.Text;
using CChessCore.Pgn;
using CChessCore.Pieces;

namespace CChessCore
{
    public abstract class BaseChessNotationParser
    {
        private readonly IList<char> _pieceLetters = new List<char>() { 'P', 'N', 'B', 'R', 'Q', 'K' };
        protected IGame _currentGame;

        public abstract IGame GetGame();

        protected virtual void DoMove(string pgnMove)
        {
            PieceType promotionTo = null;

            if(pgnMove.EndsWith(Move.CheckNotation) ||
                pgnMove.EndsWith(Move.CheckMateNotation))
            {
                pgnMove = pgnMove.Substring(0, pgnMove.Length - 1);
            }

            if (pgnMove.Contains(Move.PromotionNotation))
            {
                var temp = pgnMove.Split('=');
                pgnMove = temp[0];
                promotionTo = PieceType.GetPieceType(temp[1].Substring(0,1));
            }

            if(pgnMove.Length == 2) //then its a normal pawn move
            {
                TryMove(PieceType.Pawn, pgnMove);
            }
            else if (pgnMove.Length == 3 && _pieceLetters.Contains(pgnMove[0]))
            {
                TryMove(GetPieceType(pgnMove[0]), pgnMove.Substring(1));
            }
            else if (pgnMove.Length == 4 &&
                _pieceLetters.Contains(pgnMove[0]) &&
                pgnMove.Contains(Move.CaptureNotation))
            {
                TryMove(GetPieceType(pgnMove[0]), pgnMove.Substring(2));
            }
            else if (pgnMove.Length == 4 &&
                !_pieceLetters.Contains(pgnMove[0]) &&
                pgnMove.Contains(Move.CaptureNotation))
            {
                Square to = pgnMove.Substring(2);

                TryMoveWithAmbiguity(PieceType.Pawn, to, pgnMove[0], promotionTo);
            }
            else if (pgnMove.Length == 4 &&
                _pieceLetters.Contains(pgnMove[0]) &&
                !pgnMove.Contains(Move.CaptureNotation))
            {
                Square to = pgnMove.Substring(2);
                var pieceType = GetPieceType(pgnMove[0]);

                TryMoveWithAmbiguity(pieceType, to, pgnMove[1], promotionTo);
            }
            else if (pgnMove.Length == 5 &&
                _pieceLetters.Contains(pgnMove[0]) &&
                pgnMove.Contains(Move.CaptureNotation))
            {
                Square to = pgnMove.Substring(3);
                var pieceType = GetPieceType(pgnMove[0]);

                TryMoveWithAmbiguity(PieceType.Pawn, to, pgnMove[1], promotionTo);
            }
            else if(Move.ShortCastleNotation.Equals(pgnMove.ToUpper()))
            {
                if (_currentGame.Status.Turn == PieceColor.White)
                {
                    _currentGame.TryMove("e1", "g1");
                }
                else
                {
                    _currentGame.TryMove("e8", "g8");
                }
            }
            else if(Move.LongCastleNotation.Equals(pgnMove.ToUpper()))
            {
                if (_currentGame.Status.Turn == PieceColor.White)
                {
                    _currentGame.TryMove("e1", "c1");
                }
                else
                {
                    _currentGame.TryMove("e8", "c8");
                }
            }
        }

        private bool CheckAmbiguity(char ambiguity, Square square)
        {
            if (char.IsNumber(ambiguity))
            {
                if (ambiguity == Square.YtoCoordinate(square.Y))
                    return true;
            }
            else if (char.IsLetter(ambiguity))
            {
                if (ambiguity == Square.XtoCoordinate(square.X))
                    return true;
            }

            return false;
        }

        private void TryMoveWithAmbiguity(PieceType pieceType, Square to, char ambiguity, PieceType promotionTo = null)
        {
            var pieces = _currentGame.Board.Where(x => x.Item2.Type == pieceType &&
                                               x.Item2.Color == _currentGame.Status.Turn);

            foreach (var piece in pieces)
            {
                if (!CheckAmbiguity(ambiguity, piece.Item1))
                    continue;

                var move = new Move(piece.Item1, to);
                if (piece.Item2.CanMove(_currentGame, _currentGame.Board.Copy(), move))
                {
                    _currentGame.TryMove(move.From, move.To, promotionTo);
                    break;
                }
            }
        }

        private void TryMove(PieceType pieceType, Square to)
        {
            var pieces = _currentGame.Board.Where(x => x.Item2.Type == pieceType &&
                                               x.Item2.Color == _currentGame.Status.Turn);

            foreach (var piece in pieces)
            {
                var move = new Move(piece.Item1, to);
                if (piece.Item2.CanMove(_currentGame, _currentGame.Board.Copy(), move))
                {
                    _currentGame.TryMove(move.From, move.To);
                    break;
                }
            }
        }

        // maps "1-0", "0-1", "1/2-1/2", "*" to a Result
        protected static Result GetResult(string resultString)
        {
            switch (resultString)
            {
                case "1-0":
                    return Result.WhiteWon;
                case "0-1":
                    return Result.BlackWon;
                case "1/2-1/2":
                    return Result.Draw;
                case "*":
                    return Result.None;
                default:
                    return Result.None;
            }
        }

        protected static PieceType GetPieceType(char pieceNotation)
        {
            if (pieceNotation.Equals('P'))
            {
                return PieceType.Pawn;
            }

            if (pieceNotation.Equals('N'))
            {
                return PieceType.Knight;
            }

            if (pieceNotation.Equals('B'))
            {
                return PieceType.Bishop;
            }

            if (pieceNotation.Equals('R'))
            {
                return PieceType.Rook;
            }

            if (pieceNotation.Equals('Q'))
            {
                return PieceType.Queen;
            }

            if (pieceNotation.Equals('K'))
            {
                return PieceType.King;
            }

            return null;
        }
    }
}