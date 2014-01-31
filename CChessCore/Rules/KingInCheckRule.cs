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
using System.Text;

namespace CChessCore.Rules
{
    public class KingInCheckRule : IGameRule
    {
        #region Implementation of IGameRule
        public RuleResult IsMoveLegal(IGame game, IGameBoard board, Move move)
        {
            board[move.From] = null;
            board[move.To] = move.PieceMoved;

            if (move.PieceMoved.Type != PieceType.King)
            {
                var king = game.Board.FirstOrDefault(x => x.Item2.Color == game.Status.Turn && x.Item2.Type == PieceType.King);
                if(king != null)
                {
                    if ((move.Type & MoveType.EnPassent) == MoveType.EnPassent)
                    {
                        board[game.Movelist.CurrentMove.To] = null;
                    }

                    if(game.IsSquareUnderCheck(king.Item1, board))
                    {
                        return RuleResult.Deny;
                    }
                }
            }
            else if(game.IsSquareUnderCheck(move.To, board))
            {
                return RuleResult.Deny;
            }

            return RuleResult.Neutral;
        }
        #endregion
    }
}
