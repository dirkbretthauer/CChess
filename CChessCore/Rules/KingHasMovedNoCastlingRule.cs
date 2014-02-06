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


namespace CChessCore.Rules
{
    public class KingHasMovedNoCastlingRule : IGameRule
    {
        #region Implementation of IGameRule
        public RuleResult IsMoveLegal(IGame game, IGameBoard board, Move move)
        {
            RuleResult result = RuleResult.Neutral;

            if ((move.Type & MoveType.ShortCastle) == MoveType.ShortCastle)
            {
                bool can = move.PieceMoved.Color == PieceColor.White
                           ? game.Status.CanWhiteCastleShort
                           : game.Status.CanBlackCastleShort;

                result = can ? RuleResult.Neutral : RuleResult.Deny;
            }

            if ((move.Type & MoveType.LongCastle) == MoveType.LongCastle)
            {
                bool can = move.PieceMoved.Color == PieceColor.White
                           ? game.Status.CanWhiteCastleLong
                           : game.Status.CanBlackCastleLong;

                result = can ? RuleResult.Neutral : RuleResult.Deny;
            }


            if (move.PieceMoved.Type == PieceType.King)
            {
                game.SetCastleRight(move.PieceMoved.Color, MoveType.ShortCastle & MoveType.LongCastle, false);
            }

            if (move.PieceMoved.Type == PieceType.Rook)
            {
                if (move.PieceMoved.Color == PieceColor.White)
                {
                    if (move.From.Equals("h1"))
                    {
                        game.SetCastleRight(PieceColor.White, MoveType.ShortCastle, false);
                    }
                    else if (move.From.Equals("a1"))
                    {
                        game.SetCastleRight(PieceColor.White, MoveType.LongCastle, false);
                    }
                }
                if (move.PieceMoved.Color == PieceColor.Black)
                {
                    if (move.From.Equals("h8"))
                    {
                        game.SetCastleRight(PieceColor.Black, MoveType.ShortCastle, false);
                    }
                    else if (move.From.Equals("a8"))
                    {
                        game.SetCastleRight(PieceColor.Black, MoveType.LongCastle, false);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
