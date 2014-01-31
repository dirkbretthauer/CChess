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
