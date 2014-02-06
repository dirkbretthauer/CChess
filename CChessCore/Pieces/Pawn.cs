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