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