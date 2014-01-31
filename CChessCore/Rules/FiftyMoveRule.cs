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
    public class FiftyMoveRule : IGameRule
    {
        #region Implementation of IGameRule
        public RuleResult IsMoveLegal(IGame game, IGameBoard board, Move move)
        {
            if (move.PieceMoved.Type != PieceType.Pawn &&
                (move.Type & MoveType.Capture) != 0)
            {
                game.Status.MoveFifty += 1;
            }
            else
            {
                game.Status.MoveFifty = 0;
            }

            if(game.Status.MoveFifty == 50)
            {
                game.Status.Result = Result.Draw;
                game.Info.Result = GameStatus.GetResult(game.Status.Result);
            }

            return RuleResult.Neutral;
        }
        #endregion
    }
}
